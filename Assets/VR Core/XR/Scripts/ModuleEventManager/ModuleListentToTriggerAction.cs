
using UnityEngine.Events;


namespace MEM{

//triggers unity event if the listened event is fired

public class ModuleListentToTriggerAction : ModuleEventListenerBase
{
    
    public UnityEvent TriggerEvent;
    
    protected override void OnEvent(string eventName, object value) {
       
       //if(!variables.declarations.IsDefined(eventName)){
       //    return;
       //}
       //
       //if((variables.declarations.Get(eventName).Equals(value))){
       //    TriggerEvent.Invoke();
       //}

        
    }
    
}
}
