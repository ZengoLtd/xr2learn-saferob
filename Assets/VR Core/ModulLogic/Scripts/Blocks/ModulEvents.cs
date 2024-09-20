
using UnityEngine;

public class ModulEvents : BlockLogicBase
{
    [HideInInspector]
    public BlockEvent OnModulStart;

    public void ModulStart()
    {
        OnModulStart.Invoke(null);
        Debug.Log("Modul Start");
    }

    new void OnEnable()
    {
        base.OnEnable();
        EventManager.OnSceneStart += ModulStart;
        Debug.Log("Scene start subscribed");
    }




}
