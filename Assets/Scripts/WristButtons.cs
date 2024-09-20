
using UnityEngine;

public class WristButtons : MonoBehaviour
{
    WristMenu wristMenu;

    private void OnEnable()
    {
        wristMenu = GetComponentInParent<WristMenu>();
    } 


    public void BackToMenu()
    {
        wristMenu.BackToMenu();
    }

    public void ToggleMenu()
    {
        wristMenu.ToggleMenu();
    }
}
