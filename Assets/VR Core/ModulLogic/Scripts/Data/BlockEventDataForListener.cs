using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class BlockEventDataForListener
{
    [SerializeField]
    public GameObject gameObject;

    [SerializeField]
    public BlockLogicBase block;
    public List<string> actions = new List<string>();
    public List<string> states = new List<string>();

}


