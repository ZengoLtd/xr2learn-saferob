
using UnityEngine;

public class EnableChildBlock : BlockLogicBase
{
    [HideInInspector]
    public BlockEvent OnChildEnabled; 

    void ChildEnabled()
    {
        OnChildEnabled.Invoke(null);
    }

    public override void Logic(object data)
    {
        Debug.Log("EnableChildBlock ["+transform.name+"] Logic called");
        //foreach child enable it
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        ChildEnabled();
        base.Logic(data);
    }

}
