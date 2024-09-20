using UnityEngine;
using UnityEngine.Events;
public class SaferobEvents:MonoBehaviour 
{
    public static SaferobEvents Instance;
    void Awake(){
        Instance = this;
    }

    // hiba ha nincs nálad szenzor
    // kicserélte e a szenzort
    
    #region Actions

    public  event UnityAction OnMainSwitchDisabled;
    public  event UnityAction OnDangerousRobotZoneEntered;
    
    public  event UnityAction OnRobotMaintananceModeEnabled;
    public  event UnityAction OnRobotMaintananceModeDisabled;

    public  event UnityAction OnSensorReplaceWithoutPickup;

    public  event UnityAction OnSensorReplaced;
    public  event UnityAction OnSensorFixed;
    public  event UnityAction OnGateSensorFixed;
    #endregion


    #region Events
    public void SensorFixed() => OnSensorFixed?.Invoke();
    public void GateSensorFixed() => OnGateSensorFixed?.Invoke();
    public  void MainSwitchDisable() => OnMainSwitchDisabled?.Invoke();
    public  void DangerousRobotZoneEntered() => OnDangerousRobotZoneEntered?.Invoke();

    public  void RobotMaintananceModeEnabled() => OnRobotMaintananceModeEnabled?.Invoke();
    public  void RobotMaintananceModeDisabled() => OnRobotMaintananceModeDisabled?.Invoke();

    public  void SensorReplaceWithoutPickup() => OnSensorReplaceWithoutPickup?.Invoke();
    public  void SensorReplaced() => OnSensorReplaced?.Invoke();
    #endregion
}
