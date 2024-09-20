
using MEM;
using System.Collections.Generic;
using UnityEngine.Events;

public class RopeConnectionCounter : ModuleEventTriggerBase
{
    public List<UnityEvent> UnityEvents = new List<UnityEvent>();

    int connectionCount = 0;

    public int GetConnectionCount()
    {
        return connectionCount;
    }


    void OnEnable()
    {
        EventManager.OnRopeConnected += AddConnection;
        EventManager.OnRopeDisconnected += RemoveConnection;
    }

    void OnDisable()
    {
        EventManager.OnRopeConnected -= AddConnection;
        EventManager.OnRopeDisconnected -= RemoveConnection;
    }

    void AddConnection()
    {
        connectionCount++;
        TriggerAll();
        foreach (var action in UnityEvents)
        {
            action.Invoke();
        }
    }
    void RemoveConnection()
    {
        connectionCount--;
        TriggerAll();
        foreach (var action in UnityEvents)
        {
            action.Invoke();
        }
    }
}
