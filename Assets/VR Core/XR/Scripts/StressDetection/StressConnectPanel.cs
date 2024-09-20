using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressConnectPanel : MonoBehaviour
{
    public TMPro.TMP_InputField urlInput;   
    void Start()
    {
        if(StressManager.Instance == null)
        {
           gameObject.SetActive(false);
        }
        
        string url = "";
        //get url from playerprefs
        if (PlayerPrefs.HasKey("url"))
        {
            url = PlayerPrefs.GetString("url");
        }
        else
        {
            url = StressManager.Instance.providerURL;
        }
        urlInput.text = url;
    }

    public void SetURL()
    {
        string url = urlInput.text;
        PlayerPrefs.SetString("url", url);
        StressManager.Instance.Seturl(url);
    }
    
}
