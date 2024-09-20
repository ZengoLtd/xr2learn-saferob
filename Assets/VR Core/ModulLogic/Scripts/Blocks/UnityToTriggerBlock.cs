
using UnityEngine;

public class UnityToTriggerBlock : BlockLogicBase
{
    [HideInInspector]
    public BlockEvent OnUnityTrigger;

    public void UnityToTrigger()
    {
        OnUnityTrigger.Invoke(null);
    }

   
    
}
