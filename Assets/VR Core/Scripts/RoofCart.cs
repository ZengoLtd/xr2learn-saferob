using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class RoofCart : MonoBehaviour
{

    
    public GameObject target;
    public bool moveable = true;
  
    
    private  PathFollower pathFollower;

    public float stoppingDistanceTop;
    public float stoppingDistanceBottom;
    public float startDistance;
    public bool isLadderCart;
    public float ladderCartDownPosModifier;
    public float ladderCartDownSpeed;


    bool locked = false;
    PlayerClimbing playerClimbing;

    void OnEnable(){
        if(isLadderCart){
            EventManager.OnPlayerFalling+=LockCartOnSpeed;
        }
       
       
    }
    void OnDisable(){
        if(isLadderCart){
            EventManager.OnPlayerFalling-=LockCartOnSpeed;
        }
         
    }

    void LockCartOnSpeed(float speed){

        if(speed <= -1.5f){
            locked = true;
            
        }
    }

   
    void Update(){
        if(playerClimbing == null){
            playerClimbing = PersistentManager.Instance?.GetPlayer()?.transform.parent.GetComponentInChildren<PlayerClimbing>();
        }
        if(locked && playerClimbing.GrippingClimbable){
          locked = false;
        }
    }
    void Start()
    {
        pathFollower = GetComponent<PathFollower>();
        pathFollower.speed  = 0;
        if (PersistentManager.Instance != null)
        {
            target = PersistentManager.Instance.GetPlayer();
        }
        Teleport();
        if (!isLadderCart)
        {
            EventManager.OnTeleporterAreaEntered += Teleport;
        }
    }

    public void PutAwayCart(GameObject kisKocsi)
    {
        if (kisKocsi != null)
        {
            if (kisKocsi.activeSelf)
            {
                kisKocsi.SetActive(false);
            }
            else
            {
                kisKocsi.SetActive(true);
            }
        }
    }

    public void CartHandle(GameObject handle)
    {
        if ((bool)GetComponent<RopeConnector>().ropeConnected.state)
        {
            handle.SetActive(true);
        }
        else
        {
            handle.SetActive(false);
        }
    }

    public void ToggleCart(){
        moveable = !moveable;
    }

    //TODO: Rename function
    public void MoveCartTurnOff()
    {
        moveable = false;
        locked = false;
    }

    //TODO: Rename function
    public void MoveCartTurnOn()
    {
        moveable = true;
        locked = false;
    }


    public void TeleportTo(Transform t){
        
       pathFollower.Teleport(t.position);
        pathFollower.speed  = 0;
    }
    void Teleport(){
        pathFollower.distanceTravelled = startDistance;  
        pathFollower.speed  = 0;
        locked = false;
    }
    void LateUpdate()
    {
        if(locked){
            pathFollower.speed  = 0;
            return;
        }
        if (!moveable)
        {
            pathFollower.speed  = 0;
            return;
        }
        if(target == null)
        {
            pathFollower.speed  = 0;
            Debug.LogError("Target cannot be null! "+ DevelopmentUtilities.GetGameObjectPath(this.gameObject));
            this.enabled = false;
        }
        Vector3 targetDir;
        Vector3 faceing = transform.forward;
        if (isLadderCart)
        {
            targetDir = (target.transform.position- new Vector3(0, ladderCartDownPosModifier, 0)) - transform.position;
        }
        else
        {
            targetDir = target.transform.position - transform.position;
            
        }
        Vector3 projected = Vector3.Project(targetDir,faceing);
        if (isLadderCart)
        {
            if (pathFollower.distanceTravelled >= stoppingDistanceTop)
            {
                pathFollower.speed = -0.0015f; 
            }
            if (pathFollower.distanceTravelled <= stoppingDistanceBottom)
            {
                pathFollower.speed = 0.0015f;
            }
            else
            {
              
                if (pathFollower.speed < 0)
                {
                    pathFollower.speed = ((Mathf.RoundToInt(Vector3.Angle(projected, faceing) / 180) * -2 + 1) * projected.magnitude) - ladderCartDownSpeed;
                }
                else
                {
                    pathFollower.speed = (Mathf.RoundToInt(Vector3.Angle(projected, faceing) / 180) * -2 + 1) * projected.magnitude;
                }
            }
            
        }
        else
        {
            if (pathFollower.distanceTravelled <= stoppingDistanceTop)
            {
                pathFollower.speed = 0.0015f; 
            }
            if (pathFollower.distanceTravelled >= stoppingDistanceBottom)
            {
                pathFollower.speed = -0.0015f;
            }
            else
            {
                pathFollower.speed = (Mathf.RoundToInt(Vector3.Angle(projected, faceing) / 180) * -2 + 1) * projected.magnitude;
            }
        }
       
       
    }
}
