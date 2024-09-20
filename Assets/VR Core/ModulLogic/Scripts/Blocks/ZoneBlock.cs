
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneBlock : BlockLogicBase
{
    [HideInInspector]
    public BlockEvent OnZoneEnter;
    [HideInInspector]
    public BlockEvent OnZoneExit;

    // This method is called when the script is added to a GameObject
    private void Reset()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;  // Set the BoxCollider to be a trigger
    }
    void OnEnable(){
        EventManager.OnPlayerTeleport += Teleported;
    }
    void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position + boxCollider.center, Vector3.Scale(boxCollider.size, transform.lossyScale));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnZoneEnter?.Invoke(null);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnZoneExit?.Invoke(null);
        }
    }
    void Teleported(Vector3 positions,Quaternion quaternion, bool b, GameObject go){
        if(inZone){
            teleported = true;
        }
        
    }
    bool inZone = true;
    bool teleported = false;
    void FixedUpdate(){
        inZone = false;
    }

    void LateUpdate(){
        if(!inZone && teleported){
            teleported = false;
            OnZoneExit?.Invoke(null);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inZone = true;
        }
    }


}
