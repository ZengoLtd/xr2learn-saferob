using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Version : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<TMP_Text>().text = SystemInfo.deviceUniqueIdentifier+"\n Version: "+Application.version;
    }

   
}
