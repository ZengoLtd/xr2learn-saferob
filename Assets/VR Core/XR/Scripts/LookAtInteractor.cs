using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtInteractor : MonoBehaviour
{

   
    [Tooltip("Raycast layers to use when determining collision")]
    public LayerMask CollisionLayers;

    public float RayDistance = 100.0f;
       

    List<LookAtInteractable> interactables = new List<LookAtInteractable>();

    void OnEnable()
    {
        EventManager.OnRegisterLookAtInteractable += AddInteractable;
        EventManager.OnUnregisterLookAtInteractable += RemoveInteractable;
    }

    void AddInteractable(LookAtInteractable interactable)
    {
        interactables.Add(interactable);
    }
    void RemoveInteractable(LookAtInteractable interactable)
    {
        interactables.Remove(interactable);
    }

    void OnDisable()
    {
        EventManager.OnRegisterLookAtInteractable -= AddInteractable;
        EventManager.OnUnregisterLookAtInteractable -= RemoveInteractable;
    }

    void LateUpdate()
    {
        

        if(interactables.Count>0)
        {
            //cast ray from camera and get hit location
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, RayDistance,CollisionLayers)){
                LookAtInteractable closest = interactables[0];
                float closestDistance = float.MaxValue;
                foreach (var interactable in interactables)
                {
                    float distance = Vector3.Distance(hit.point, interactable.transform.position);

                    if(distance<closestDistance){
                        closest = interactable;
                        closestDistance = distance;
                    }
                }
                closest.Trigger(hit.point);
            }

        }

    }
}
