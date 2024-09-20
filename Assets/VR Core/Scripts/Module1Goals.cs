using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEM;
using System;
using Zengo.Inventory;
using UnityEngine.Localization;

public class Module1Goals : ModuleGoals
{

    // ++ visit storage (nessecary items)
    // ++ safely visit roof (safe roof movement)
    //optimal item use (no unnecessary items)
    // ++ turn off inverter 
    // ++ step 
    // ++ time

    public List<UnityEvent> safeFailunityEvents = new List<UnityEvent>();
    public RopeConnectionCounter ropeConnectionCounter;

    public GameObject emeloFent;
    public GameObject emeloLent;

    public GameObject kiskocsi;


    int steps = 0;
    float starttime = 0;
    bool inverterOff = false;
    bool failedRoof = false;
    bool visitedRoof = false;
    public bool? optimalUse = null;


    void OnEnable()
    {
        EventManager.OnBadItemUsed += OnBadItemUsed;
    }
    void OnBadItemUsed()
    {
        failedRoof = true;
        optimalUse = false;
    }
    void Start()
    {

        starttime = Time.time;
        EventManager.OnPlayerTeleportBeginning += IncreaseStep;
        ModuleEventManager.OnEvent += DangerCheck;
    }

    public void TurnOffInverter()
    {
        inverterOff = true;
    }

    void OnDestroy()
    {
        Instance = null;
        EventManager.OnPlayerTeleportBeginning -= IncreaseStep;
        EventManager.OnBadItemUsed -= OnBadItemUsed;
        ModuleEventManager.OnEvent -= DangerCheck;
    }
    public string GetTime()
    {
        TimeSpan t = TimeSpan.FromSeconds(Time.time - starttime);
        string answer = string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds);

        return answer;
    }
    public void RoofCheck()
    {
        visitedRoof = true;
        CheckDangerZone();
    }
    bool inDangerzone = false;
    public void InZone()
    {
        Debug.Log("Entered danger zone");
        inDangerzone = true;
    }
    public void OutOfZone()
    {
        Debug.Log("Exited danger zone");
        inDangerzone = false;
    }
    public void CheckDangerZone()
    {
        if (!inDangerzone)
        {
            Debug.Log("Not in danger zone");
            return;
        }
        //if no rope, failed
        if (ropeConnectionCounter.GetConnectionCount() == 0)
        {
            Debug.Log("In danger zone and not connected to rope");
            failedRoof = true;
            optimalUse = false;
            foreach (UnityEvent e in safeFailunityEvents)
            {
                e.Invoke();
            }
        }

    }
    void DangerCheck(string eventName, object value)
    {

        if (eventName == "RoofZone")
        {
            visitedRoof = true;
            return;
        }
        if (failedRoof)
        {
            return;
        }
        if (eventName == "dangerFinished")
        {
            failedRoof = true;
            optimalUse = false;
            return;
        }

    }
    bool SafeMovement()
    {
        if (visitedRoof && !failedRoof)
        {
            return true;
        }
        return false;
    }

    bool checkCart()
    {
        return !kiskocsi.activeInHierarchy;
    }

    bool CheckStorage()
    {
        //check if user have all items
        bool valid = InventoryManager.Instance != null
                    && InventoryManager.Instance.HasItem(ItemType.Helmet)
                    && InventoryManager.Instance.HasItem(ItemType.Gloves)
                    && InventoryManager.Instance.HasItem(ItemType.Shoes)
                    && InventoryManager.Instance.HasItem(ItemType.Glasses)
                    && InventoryManager.Instance.HasItem(ItemType.Harness);
        return valid;
    }



    bool checkEmelo()
    {
        if (emeloFent.gameObject.activeInHierarchy)
        {
            return false;
        }
        if (emeloLent.gameObject.activeInHierarchy)
        {
            return true;
        }
        //erre nem futhat elvileg
        Debug.LogError("Something went wrong with emelo check");
        return false;
    }
    bool CheckInverter()
    {
        return inverterOff;
    }

    void IncreaseStep()
    {
        steps++;
    }

    void Validate()
    {
        CheckStorage();
        CheckInverter();
    }

    public override List<ModuleTask> Report()
    {

        if (optimalUse == null)
        {
            optimalUse = false;
        }
        List<ModuleTask> tasks = new List<ModuleTask>();
        tasks.Add(new ModuleTask(CheckStorage(), GetLocalizedString(_equipment).GetLocalizedString()));
        tasks.Add(new ModuleTask(SafeMovement(), GetLocalizedString(_safeRoof).GetLocalizedString()));
        tasks.Add(new ModuleTask((optimalUse == true), GetLocalizedString(_optimalUse).GetLocalizedString()));
        tasks.Add(new ModuleTask(CheckInverter(), GetLocalizedString(_inverter).GetLocalizedString()));
        tasks.Add(new ModuleTask(checkEmelo(), GetLocalizedString(_hoist).GetLocalizedString()));
        tasks.Add(new ModuleTask(checkCart(), GetLocalizedString(_cart).GetLocalizedString()));
        tasks.Add(new ModuleTask(true, GetTime(), false));
        tasks.Add(new ModuleTask(true, steps.ToString(), false));

        return tasks;
    }

    private string _equipment = "task_equipment";
    private string _safeRoof = "task_ascent_descent";
    private string _optimalUse = "task_protection";
    private string _inverter = "task_inverteroff";
    private string _hoist = "task_hoist";
    private string _cart = "task_laddercart";

    private LocalizedString GetLocalizedString(string key)
    {
        return new LocalizedString("Protfall", key);
    }


}
