using UnityEngine;

public class InfoPanels : MonoBehaviour
{
    public float panelAnimTime;
    public GameObject panel;
    public GameObject grabQuad;

    public Texture2D openTex;
    public Texture2D hoverTex;
    public Texture2D closeTex;
    public Texture2D hoverCloseTex;
    public OnGrab onGrab;
    public bool isOpen;

    float originalY;
    CanvasGroup canvasGroup;

    private void OnEnable()
    {
        EventManager.OnPlayerTeleportBeginning += CloseOnTeleport;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerTeleportBeginning -= CloseOnTeleport;
    }

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        onGrab.normalTex = openTex;
        onGrab.selectedTex = hoverTex;
        canvasGroup.LeanAlpha(0, 0);
    }

    public void OpenPanel()
    {
        if (!panel.LeanIsTweening())
        {
            if (!isOpen)
            {
                canvasGroup.LeanAlpha(1,panelAnimTime).setEaseInBounce();
                isOpen = true;
                onGrab.normalTex = closeTex;
                onGrab.selectedTex = hoverCloseTex;
            }
            else
            {
                canvasGroup.LeanAlpha(0, panelAnimTime).setEaseOutBounce();
                isOpen = false;
                onGrab.normalTex = openTex;
                onGrab.selectedTex = hoverTex;
            }
        }
        else
        {
            return;
        }
    }

    void CloseOnTeleport()
    {
        if (isOpen)
        {
            OpenPanel();
        }
    }
}
