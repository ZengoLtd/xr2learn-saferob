
using UnityEngine;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;  
#endif

public class SceneChanger : MonoBehaviour
{
    public string moduleName;
    bool sceneLoading = false;

    public void ChangeModuleName(string newName)
    {
        moduleName = newName;
    }

    public void ChangeScene() {

        if(sceneLoading)
        {
            return;
        }
        sceneLoading = true;
       
#if PHOTON_UNITY_NETWORKING

        if(ZNetworkManager.Instance != null && PhotonNetwork.InRoom){
            ZNetworkManager.Instance.LoadScene(moduleName);
        }else{
            SceneLoadManager.Instance.LoadScene(moduleName);
        }

#else
        SceneLoadManager.Instance.LoadScene(moduleName);
#endif
    }
}
