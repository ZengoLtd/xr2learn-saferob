using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WristMenu : MonoBehaviour
{
    public InputActionReference MenuToggleAction;
    public GameObject prefabPanel;

    bool visible = false;
    public bool isRotatingAround;
    GameObject wristPanel;
    Canvas canvas;
    void OnEnable() {
        if (prefabPanel != null)
        {
            wristPanel = Instantiate(prefabPanel, transform);
            canvas = wristPanel.GetComponent<Canvas>();
        }
        if(canvas == null){
            Debug.LogError("No canvas found "+ DevelopmentUtilities.GetGameObjectPath(this.gameObject));
            this.enabled = false;
            return;
        }
        canvas.enabled = visible;
        if(MenuToggleAction) {
            MenuToggleAction.action.Enable();
            MenuToggleAction.action.performed += OnToggleMenu;
        }
    }

    public void BackToMenu(){
        ToggleMenu();
        SceneLoadManager.Instance.LoadMenu();    
    }

    void OnDisable() {
        if (MenuToggleAction) {
            MenuToggleAction.action.Disable();
            MenuToggleAction.action.performed -= OnToggleMenu;
        }
    }
    public void ToggleMenu(){
        visible = !visible;
        canvas.enabled = visible;
    }
    
    public void OnToggleMenu(InputAction.CallbackContext context) {

        if (SceneManager.GetActiveScene().name != "SceneSelector")
        {
            ToggleMenu();
        }
    }

    private void LateUpdate()
    {
        if (isRotatingAround)
        {
            transform.LookAt(PersistentManager.Instance.GetPlayer().transform);
        }
    }

}
