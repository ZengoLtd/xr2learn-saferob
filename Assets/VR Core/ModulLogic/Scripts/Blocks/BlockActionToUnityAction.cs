
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockActionToUnityAction : BlockLogicBase
{
    public List<UnityEvent> UnityEvents = new List<UnityEvent>();

    public override void Logic(object data)
    {
      
        foreach (var action in UnityEvents)
        {
            action.Invoke();
        }  
        base.Logic(data);
    }

    
}

