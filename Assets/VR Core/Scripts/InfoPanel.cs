using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public GameObject smallPanel;
    public GameObject bigPanel;

   public void OpenPanel()
   {
       smallPanel.SetActive(false);
         bigPanel.SetActive(true);
   }
   public void ClosePanel()
   {
       
            bigPanel.SetActive(false);
            smallPanel.SetActive(true);
    }
    void Start()
    {
        bigPanel.SetActive(false);
     
   }
}
