using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MEM{
public class ModuleEventManager : MonoBehaviour
{


    private static Dictionary<string,object> eventDictionary = new Dictionary<string, object>();

    public static UnityAction<string,object> OnEvent;

    public static object GetEventLastValue(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            return eventDictionary[eventName];
        }
        return null;
    }
   
    public static void TriggerEvent(string key, object value) {
        
       
        if (eventDictionary.ContainsKey(key))
        {
            eventDictionary[key] = value;
        }
        else
        {
            eventDictionary.Add(key, value);
        }
        OnEvent?.Invoke(key,value);
    } 
    
    public static void TriggerMultiple(string[] list){
        foreach(string key in list){
            TriggerEvent(key,GetEventLastValue(key));
        }
    }

}
}