using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject Gate;
    
    private Vector3 upPosition;
    private Vector3 downPosition;
    private float speed = 1.0f; // Speed of the movement

    private int ObjectCounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        downPosition = Gate.transform.position;
        upPosition = new Vector3(Gate.transform.position.x, Gate.transform.position.y + 1, Gate.transform.position.z);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        ObjectCounter++;
        
        StopAllCoroutines(); // Stop any ongoing movement
        StartCoroutine(MoveGate(Gate.transform.position, upPosition)); // Start moving the gate up
    }

    // This method is called when an object leaves the trigger
    private void OnTriggerExit(Collider other)
    {
        ObjectCounter--;
        
        if (ObjectCounter < 1)
        {
            StopAllCoroutines();
            StartCoroutine(MoveGate(Gate.transform.position, downPosition));
        }
    }

    IEnumerator MoveGate(Vector3 start, Vector3 end)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            Gate.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
}
