using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform startPosition;

    public RobotAnimSpeed robotAnimSpeed;

    MeshRenderer meshRenderer;
    Quaternion startRotation;
    bool isAlreadyHit = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        startRotation = transform.rotation;
    }
    public void Hit()
    {
        if(!isAlreadyHit)
        {        
            isAlreadyHit = true;
        }

    }

    void ResetToStartPosition()
    {
        transform.rotation = startRotation;
        gameObject.transform.position = startPosition.position;
        if(gameObject.transform.position == startPosition.position)
        {
            meshRenderer.enabled = true;
          
        }
        isAlreadyHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "RobotCollider")
        {
            robotAnimSpeed.PickUPAnim();
        }
        if(other.gameObject.name == "Robothead")
        {
            meshRenderer.enabled = false;
            ResetToStartPosition();
        }
        if(other.gameObject.name == "ResetBox")
        {
            meshRenderer.enabled = false;
            ResetToStartPosition();
        }
    }
    

}
