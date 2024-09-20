using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSelectorUI : MonoBehaviour
{
    bool sceneLoading = false;
    public void LoadModule(string moduleName)
    {
        if(sceneLoading)
        {
            return;
        }
        sceneLoading = true;
        SceneLoadManager.Instance.LoadScene(moduleName);
        LightProbes.TetrahedralizeAsync();
        
    }
}
