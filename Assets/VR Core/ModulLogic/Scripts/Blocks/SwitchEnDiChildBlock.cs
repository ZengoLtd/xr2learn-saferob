using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEnDiChildBlock : BlockLogicBase
{
    [HideInInspector]
    public BlockEvent OnChildStateSwitched;


    void ChildStateSwitched()
    {
        OnChildStateSwitched.Invoke(null);
    }

    public override void Logic(object data)
    {
        Debug.Log("Switched enabled/disabled state on [" + transform.name + "] Logic called");
        //foreach child enable it
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
        ChildStateSwitched();
        base.Logic(data);
    }
}
