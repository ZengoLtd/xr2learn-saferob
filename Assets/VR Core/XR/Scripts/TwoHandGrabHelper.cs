using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class TwoHandGrabHelper : MonoBehaviour
{
    
    private Grabbable grabbable;
    private Quaternion rotation;
    private Vector3 position;
    bool pickedup = false;

    public AudioSource pickUpAudio;
    private bool picked;

    void Awake(){
        grabbable = GetComponent<Grabbable>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!grabbable.BeingHeldWithTwoHands && grabbable.BeingHeld){
            //grabbed with one hand
            if(!pickedup){
                transform.rotation = rotation;
                transform.position = position;
            }else{
                grabbable.DropItem(grabbable.HeldByGrabbers[0], false, true);
                grabbable.DropItem(grabbable.HeldByGrabbers[0], false, true);
            }
            
        }
        if(grabbable.BeingHeldWithTwoHands && grabbable.BeingHeld){
            //grabbed with two hand
            if (picked)
            {
                pickUpAudio.Play();
            picked = false;
            }
            pickedup = true;
        }
        if(!grabbable.BeingHeldWithTwoHands && !grabbable.BeingHeld){
            
            //grabbed with zero hand
            rotation = transform.rotation;
            position = transform.position;
            pickedup = false;
            picked = true;
        }

    }
}
