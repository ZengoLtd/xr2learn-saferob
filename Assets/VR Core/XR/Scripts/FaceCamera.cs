using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public bool lockVertical = false;
    
    void OnEnable(){
        faceCamera();
    }

    void LateUpdate()
    {

       faceCamera();
        
    }

    void faceCamera(){
        if(Camera.main != null){
            Vector3 direction = 2 * transform.position - Camera.main.transform.position;
            if(lockVertical){
                direction.y = transform.position.y;
            }
            transform.LookAt(direction);
        }
        
    }
}
