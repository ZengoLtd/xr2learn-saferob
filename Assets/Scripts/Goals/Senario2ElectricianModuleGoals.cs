
using System.Collections.Generic;
using UnityEngine;
using System;
using Zengo.Inventory;
using UnityEngine.Localization;

public class Senario2ElectricianModuleGoals : ModuleGoals
{
    //++ visit storage (nessecary items)
    //++ number of teleports
    //++ time
    //++ bad teleport
    //++ menet közbe belép a robothoz
    //++ főkapcsoló hiba

    //++ hiba ha karbantartó mode a robotot (IT)
    //++ hiba ha nem veszi ki karbantartóból a robotot(IT)
    //++ megigazitotta e a szenzort (Electrical)
    //++ megigazitotta e a kapuszenzort (Electrical)

    float starttime = 0;
    bool badteleported = false;
    int steps = 0;
    bool mainSwitchDisabled = false;

    bool sensorFixed = false;
    bool gatesensorFixed = false;
    bool dangeroisRobotZoneEntered = false;
    bool robotMaintananceModeEnabled = false;
    bool robotMaintananceModeDisabled = false;

    protected override void Awake()
    {
        //empty for a reason
    }
    void Update()
    {
        Instance = this;
    }
    void Start()
    {
        if (RoleSelector.Instance.playerRole != Role.Electrician)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        starttime = Time.time;
        EventManager.OnPlayerTeleportBeginning += () => steps++;
        EventManager.OnBadTeleport += () => badteleported = true;
        SaferobEvents.Instance.OnMainSwitchDisabled += () => mainSwitchDisabled = true;
        SaferobEvents.Instance.OnSensorFixed += () => sensorFixed = true;
        SaferobEvents.Instance.OnGateSensorFixed += () => gatesensorFixed = true;
        SaferobEvents.Instance.OnDangerousRobotZoneEntered += () => dangeroisRobotZoneEntered = true;
        SaferobEvents.Instance.OnRobotMaintananceModeEnabled += () => robotMaintananceModeEnabled = true;
        SaferobEvents.Instance.OnRobotMaintananceModeDisabled += () => robotMaintananceModeDisabled = true;
    }

    void OnDestroy()
    {
        Instance = null;
        EventManager.OnPlayerTeleportBeginning -= () => { };
        EventManager.OnBadTeleport -= () => { };
        SaferobEvents.Instance.OnMainSwitchDisabled -= () => { };
        SaferobEvents.Instance.OnSensorFixed -= () => { };
        SaferobEvents.Instance.OnGateSensorFixed -= () => { };
        SaferobEvents.Instance.OnDangerousRobotZoneEntered -= () => { };
        SaferobEvents.Instance.OnRobotMaintananceModeEnabled -= () => { };
        SaferobEvents.Instance.OnRobotMaintananceModeDisabled -= () => { };
    }

    public string GetTime()
    {
        TimeSpan t = TimeSpan.FromSeconds(Time.time - starttime);
        string answer = string.Format("{0:D2}:{1:D2}",
                 t.Minutes,
                 t.Seconds);

        return answer;
    }
    bool CheckStorage()
    {
        //check if user have all items
        bool valid = InventoryManager.Instance != null
                    && InventoryManager.Instance.HasItem(ItemType.Helmet)
                    && InventoryManager.Instance.HasItem(ItemType.Gloves)
                    && InventoryManager.Instance.HasItem(ItemType.Shoes)
                    && InventoryManager.Instance.HasItem(ItemType.Glasses);
        return valid;
    }

    public override List<ModuleTask> Report()
    {
        List<ModuleTask> tasks = new List<ModuleTask>();
        tasks.Add(new ModuleTask(true, GetTime(), false));
        tasks.Add(new ModuleTask(true, steps.ToString(), false));
        tasks.Add(new ModuleTask(CheckStorage(), GetLocalizedString(_equipment).GetLocalizedString()));
        tasks.Add(new ModuleTask(!badteleported, GetLocalizedString(_teleport).GetLocalizedString()));
        tasks.Add(new ModuleTask(!mainSwitchDisabled, GetLocalizedString(_inverteroff).GetLocalizedString()));
        tasks.Add(new ModuleTask(!dangeroisRobotZoneEntered, GetLocalizedString(_dangerouszone).GetLocalizedString()));
        tasks.Add(new ModuleTask(!robotMaintananceModeEnabled, GetLocalizedString(_maintanancemodeon).GetLocalizedString()));
        tasks.Add(new ModuleTask(!robotMaintananceModeDisabled, GetLocalizedString(_maintanancemodedisabled).GetLocalizedString()));
        tasks.Add(new ModuleTask(sensorFixed, GetLocalizedString(_sensorfix).GetLocalizedString()));
        tasks.Add(new ModuleTask(gatesensorFixed, GetLocalizedString(_lightgatefix).GetLocalizedString()));
        return tasks;
    }

    private string _equipment = "task_picked_up_safety";
    private string _teleport = "task_correct_teleporters";
    private string _inverteroff = "task_inverter_not_turned_off";
    private string _dangerouszone = "task_danger_zone";
    private string _maintanancemodeon = "task_mainentance_enabled";
    private string _maintanancemodedisabled = "task_maintenance_disabled";
    private string _sensorfix = "sensor_fix";
    private string _lightgatefix = "lightgate_fix";

    private LocalizedString GetLocalizedString(string key)
    {
        return new LocalizedString("SafeRob", key);
    }
}
