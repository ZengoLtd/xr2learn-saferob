
using System.Collections.Generic;
using UnityEngine;
using Zengo.Inventory;
public class RopeManagerBlock : BlockLogicBase
{
    [HideInInspector]
    public BlockEvent OnRopeConnected;
    [HideInInspector]
    public BlockEvent OnRopeDisconnected;

    public BlockState ropeConnectedCount;


     
    public static RopeManagerBlock Instance;
    public GameObject ropeLimiter;
    public List<GameObject>RopeInstances;
    List<GameObject> ActiveRopes = new List<GameObject>();
    List<GameObject> ActiveHarnesses = new List<GameObject>();
    GameObject ObjectPool;


    public void AddHarness(GameObject harness){
        ActiveHarnesses.Add(harness);
        ropeConnectedCount.state = (ActiveRopes.Count+ ActiveHarnesses.Count);
        OnRopeConnected.Invoke(ropeConnectedCount);
    }
    public void RemoveHarness(GameObject harness){
        ActiveHarnesses.Remove(harness);
        ropeConnectedCount.state = (ActiveRopes.Count+ ActiveHarnesses.Count);
        OnRopeDisconnected.Invoke(ropeConnectedCount);
    }
    void OnEnable(){

        ObjectPool = GameObject.Find("ObjectPool");
        if(Instance == null){
            Instance = this;
        }
        else{
            Debug.LogError("Too many RopeManager Instances");
            Destroy(this);
        }
        while(RopeInstances.Count>0){
            FreeUpRope(RopeInstances[0]);
            RopeInstances.Remove(RopeInstances[0]);
        }
        
        ropeConnectedCount.state = (ActiveRopes.Count+ ActiveHarnesses.Count);

    }
    void FixedUpdate(){
        if( ropeLimiter == null){
            return;
        }
        if((ActiveRopes.Count+ ActiveHarnesses.Count) == 0 ){

            ropeLimiter.SetActive(false);
            return;
        }
        
        ropeLimiter.SetActive(true);
        float bestdistance = 0;
        GameObject bestrope = null;
        Vector3 playerposition = PersistentManager.Instance.GetPlayer().transform.position;
        foreach(var rope in ActiveRopes){
            float distance = Vector3.Distance(rope.transform.position, playerposition);
            if(distance > bestdistance){
                bestdistance = distance;
                bestrope = rope;
            }
        }
        foreach(var harness in ActiveHarnesses){
            float distance = Vector3.Distance(harness.transform.position, playerposition);
            if(distance > bestdistance){
                bestdistance = distance;
                bestrope = harness;
            }
        }
        ropeLimiter.transform.position = bestrope.transform.position;
        
    }
    public bool isConnected(){
        return (ActiveRopes.Count+ ActiveHarnesses.Count) > 0;
    }
    
    public void FreeUpRope(GameObject rope){
        if(rope == null){
            return;
        }
        rope.transform.parent = ObjectPool.transform;
        rope.transform.position = ObjectPool.transform.position;
        
        ActiveRopes.Remove(rope);
        Destroy(rope.gameObject);
        ropeConnectedCount.state = (ActiveRopes.Count+ ActiveHarnesses.Count);

        OnRopeDisconnected.Invoke(ropeConnectedCount);
        
    }

    public int FreeRopesAmount()
    {
        return ((Ropes)InventoryManager.Instance.GetitemByType(ItemType.Ropes)).allowedRopeCount - ActiveRopes.Count;
    }

    public GameObject GetRope(){
        if(FreeRopesAmount() == 0){
            Debug.Log("No more ropes");
            return null;
        }

        if (InventoryManager.Instance.HasItem(ItemType.Ropes))
        {
            Ropes inventoryRope = (Ropes)InventoryManager.Instance.GetitemByType(ItemType.Ropes);
           
            
            if (ActiveRopes.Count < inventoryRope.allowedRopeCount)
            {
                if(inventoryRope.descriptor.prefab == null){
                    Debug.LogError("Rope prefab not set in scriptable object! ["+inventoryRope.name+"]");
                    return null;
                    
                }
                var rope = Instantiate(inventoryRope.descriptor.prefab, ObjectPool.transform);
                rope.transform.position = ObjectPool.transform.position;
                
                ActiveRopes.Add(rope);
                ropeConnectedCount.state = (ActiveRopes.Count+ ActiveHarnesses.Count);

                OnRopeConnected.Invoke(ropeConnectedCount);
              
                return rope;
            }
        }
        else
        {
            return null;
        }
        return null;
    }


    void OnDisable(){
        if(Instance == this){
            Instance = null;
        }
    }
}
