
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimateBlock : BlockLogicBase
{
    public string AnimationName;
    

    public override void Logic(object data)
    {
        base.Logic(data);
        //foreach child enable it
        GetComponent<Animator>().Play(AnimationName);
    }
}
