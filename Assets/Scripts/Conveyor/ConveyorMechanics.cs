using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum RelativeDirection
{
    Up,
    Down,
    Left,
    Right,
    Forward,
    Backward
}

public class ConveyorMechanics : MonoBehaviour
{
    [SerializeField] private RelativeDirection direction = RelativeDirection.Down;

    public float speed, baseSpeed, slowDuration;
    [SerializeField]
    private List<Rigidbody> onConveyor;
    
    private bool beltIsOn = true;

    // Fixed update for physics
    void FixedUpdate()
    {
        if (beltIsOn)
        {
            // For every item on the belt, add force to it in the direction given
            foreach (var rb in onConveyor)
            {
                rb.transform.position += Vector3.back * speed * Time.deltaTime;
            }
        }
    }

    public void SwitchTurnBelt()
    {
        beltIsOn = !beltIsOn;
    }

    public void SlowConveyorForTime(float newSpeed)
    {
        StartCoroutine(SlowConveyor(newSpeed));
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ResetSpeed()
    {
       speed = baseSpeed;
    }

    // When something collides with the belt
    private void OnCollisionEnter(Collision collision)
    {
        onConveyor.Add(collision.gameObject.GetComponent<Rigidbody>());
    }

    // When something leaves the belt
    private void OnCollisionExit(Collision collision)
    {
        onConveyor.Remove(collision.gameObject.GetComponent<Rigidbody>());
    }

    IEnumerator SlowConveyor(float slowingSpeedTo)
    {
        speed = slowingSpeedTo;
        yield return new WaitForSeconds(slowDuration);
        speed = baseSpeed;
    }
    
    public Vector3 GetDirection()
    {
        switch (this.direction)
        {
            case RelativeDirection.Up:
                return transform.up;
            case RelativeDirection.Down:
                return -transform.up;
            case RelativeDirection.Left:
                return -transform.right;
            case RelativeDirection.Right:
                return transform.right;
            case RelativeDirection.Forward:
                return transform.forward;
            case RelativeDirection.Backward:
                return -transform.forward;
        }
    
        return transform.forward;
    }

    public void SwitchConveyorState()
    {
        if (beltIsOn)
        {
            beltIsOn = false;
        }
        else
        {
            beltIsOn = true;
        }
    }

    public bool GetbeltIsOn()
    {
        return beltIsOn;
    }
}
