
using UnityEngine;

public class DisableChildBlock : BlockLogicBase
{
    
    [HideInInspector]
    public BlockEvent OnChildDisabled; 

    void ChildDisabled()
    {
        OnChildDisabled.Invoke(null);
    }

    public override void Logic(object data)
    {
        Debug.Log("DisableChildBlock ["+transform.name+"] Logic called");
        //foreach child enable it
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        ChildDisabled();
        base.Logic(data);
    }

}
