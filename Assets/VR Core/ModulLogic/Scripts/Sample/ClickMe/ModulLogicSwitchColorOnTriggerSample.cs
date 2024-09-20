
using UnityEngine;

public class ModulLogicSwitchColorOnTriggerSample : BlockLogicBase
{
    public override void Logic(object data)
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}
