using UnityEngine;


namespace MEM{
public class ModuleEventTriggerBase : ModuleEventBase
{
    public string LogEvent = "";
   
    protected void TriggerEvent(string eventName, object value) {
        
     //  if(!variables.declarations.IsDefined(eventName)){
     //      Debug.LogError("Unregistered event was called!");
     //      return;
     //  }
     //  if(LogEvent != ""){
     //     CommunicationManager.Instance?.Log(LogEvent);
     //  }
     //  ModuleEventManager.TriggerEvent(eventName,value);
    }

    public void TriggerAll(){
      // if(variables==null){
      //     return;
      // }
      // foreach (var declaration in variables.declarations)
      // {
      //     TriggerEvent(declaration.name, declaration.value);
      // }
    }

    protected virtual void OnEvent(string eventName, object value) {
      // if((variables.declarations.Get(eventName).Equals(value))){
      //     return;
      // }
    }
    void OnDisable(){
        ModuleEventManager.OnEvent -= OnEvent;
    }


}
}