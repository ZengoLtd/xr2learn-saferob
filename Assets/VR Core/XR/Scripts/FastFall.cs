using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using System.Reflection;
public class FastFall : MonoBehaviour
{
    PlayerGravity playerGravity;
    CharacterController characterController;
    FieldInfo movementY_field;

    bool isFalling = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerGravity = GetComponent<PlayerGravity>();
        movementY_field = typeof(PlayerGravity).GetField("_movementY", BindingFlags.NonPublic | BindingFlags.Instance);
       
    }

    Vector3? startDistance = null;
    void Update()
    {
        float movementY = (float)movementY_field.GetValue(playerGravity);
       
        if(startDistance == null){
            startDistance = transform.position;
        } 
        if(movementY < -3.5f) { 
            if(Vector3.Distance(transform.position, startDistance.Value) > 1.0f){
                if (!isFalling)
                {
                    isFalling = true;
                    EventManager.PlayerFallingTooFastEvent();
                }
            }
        }
        if(movementY<0){
            EventManager.PlayerFalling(movementY);
        }
        if (characterController.isGrounded) {
            startDistance = null;
            EventManager.PlayerFallEnded();
            isFalling = false;
        }
    }
}
