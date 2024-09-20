
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class BlockEvent
{
    [SerializeField]
    public UnityEvent<object> unityEvent;
    
  

    internal void Invoke(object value)
    {
        unityEvent?.Invoke(value);
    }
}
