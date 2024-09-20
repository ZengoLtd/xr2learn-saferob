using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using RootMotion;
using Photon.Pun;


#if UNITY_EDITOR
using UnityEditor;
#endif

[SelectionBase]
public abstract class BlockLogicBase : MonoBehaviour
{

    public bool networked = false;

    [HideInInspector]
    public BlockEvent OnTriggered;
    public BlockEvent GetPublicAction(object obj, string actionname)
    {
        if (obj == null) return null;
        var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(BlockEvent) && field.Name == actionname)
            {
                return (BlockEvent)field.GetValue(obj);
            }
        }
        return null;
    }

    public BlockState GetPublicState(object obj, string statename)
    {
        if (obj == null) return null;
        var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(BlockState) && field.Name == statename)
            {
                return (BlockState)field.GetValue(obj);
            }
        }
        return null;
    }


    [HideInInspector]
    [SerializeField]
    public List<BlockEventDataForListener> ActivateOn = new List<BlockEventDataForListener>();

    public void SubscribeToEvents(UnityEngine.Events.UnityAction<object> call)
    {
        foreach (BlockEventDataForListener data in ActivateOn)
        {
            if (data.block == null)
            {
                continue;
            }
            foreach (string action in data.actions)
            {
                BlockEvent bevent = GetPublicAction(data.block, action);
                if (bevent != null)
                {
                    bevent.unityEvent.AddListener(call);
                    //Debug.Log("BlockEvent found");
                }
                else
                {
                    Debug.LogError("[" + this.GetType().Name + "] {" + transform.name + "} is looking for BlockEvent not found " + data.block.gameObject.name + " " + action);
                }

            }
        }
    }

    public void UnsubscribeToEvents(UnityEngine.Events.UnityAction<object> call)
    {
        foreach (BlockEventDataForListener data in ActivateOn)
        {
            if (data.block == null) continue;
            foreach (string action in data.actions)
            {
                BlockEvent bevent = GetPublicAction(data.block, action);
                bevent.unityEvent.RemoveListener(call);
            }
        }
    }
    public void OnEnable()
    {
        SubscribeToEvents(Logic);
        if(GetComponent<PhotonView>() == null && networked){
           Debug.LogError("["+gameObject.name+"]Networked block without PhotonView");
           
        }
    }

    public void OnDisable()
    {
        UnsubscribeToEvents(Logic);
    }

    [PunRPC]
    public virtual void NetworkLogic(object data){
        OnTriggered?.Invoke(null);
    }


    public virtual void Logic(object data){
        OnTriggered.Invoke(data);
        if(networked){
            
            PhotonView photonView = PhotonView.Get(this);
            if(photonView != null){
                photonView?.RPC("NetworkLogic", RpcTarget.Others, data);
            }
           
        }
    }
    void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        Gizmos.DrawIcon(transform.position, "Assets/VR Core/ModulLogic/UI/stapler-solid.png", true);
    #endif
    }

    #if UNITY_EDITOR
    void Reset()
    {
       
        ActivateOn.Clear();
        EditorUtility.SetDirty(this);

    }
    #endif
}
 
