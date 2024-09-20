
using UnityEngine;

public class MoverBlock : BlockLogicBase
{
    public Vector3 targetPosition;
    

    public override void Logic(object data)
    {
        transform.position = targetPosition;
        base.Logic(data);
    }
}
