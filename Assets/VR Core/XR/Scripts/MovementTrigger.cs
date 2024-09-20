using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MEM{
public class MovementTrigger : ModuleEventTriggerBase
{
    bool watching = true;
    Vector3 startPos;
    void OnEnable()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if(!watching)
            return;
        if (Vector3.Distance(transform.position, startPos) > 0.1f)
        {
            if (watching)
            {
                watching = false;
                TriggerAll();
            }
        }
        
    }


}
}