using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailScreen : MonoBehaviour
{
    public void End(){
        Module1Goals goals = GameObject.Find("ModuleLogic").GetComponent<Module1Goals>();
        var report = goals.Report();
        report.Add(new ModuleTask(false,"Kritikus hibát vétett.", true));
        CommunicationManager.Instance.EndModule(report,true);
        SceneLoadManager.Instance.LoadMenu();
    }
}
