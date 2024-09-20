using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BNG;

public class DebugTextDisplay : MonoBehaviour
{
    private static DebugTextDisplay instance;
    private TextMeshProUGUI debugText;
    private Image backgroundImage;

    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField] private Color backgroundColor = new Color(0, 0, 0, 0.5f); // Semi-transparent black

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetupDebugText();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupDebugText()
    {
        // Create a Canvas
        GameObject canvasObj = new GameObject("DebugCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = GameObject.Find("UICam").GetComponent<Camera>();
        canvas.planeDistance = 0.44f;
        canvasObj.AddComponent<CanvasScaler>();
        

        // Create a background image
        GameObject backgroundObj = new GameObject("DebugTextBackground");
        backgroundObj.transform.SetParent(canvasObj.transform, false);
        backgroundImage = backgroundObj.AddComponent<Image>();
        backgroundImage.color = backgroundColor;

        // Create a TextMeshPro - Text (UI) object
        GameObject textObj = new GameObject("DebugText");
        textObj.transform.SetParent(backgroundObj.transform, false);
        debugText = textObj.AddComponent<TextMeshProUGUI>();

        if (fontAsset != null)
        {
            debugText.font = fontAsset;
        }

        debugText.fontSize = 20;
        debugText.color = Color.white;
        debugText.alignment = TextAlignmentOptions.TopRight;

        // Position the background in the top right corner
        RectTransform backgroundTransform = backgroundImage.rectTransform;
        backgroundTransform.anchorMin = new Vector2(1, 1);
        backgroundTransform.anchorMax = new Vector2(1, 1);
        backgroundTransform.anchoredPosition = new Vector2(-5, -5);
        backgroundTransform.pivot = new Vector2(1, 1);

        // Set the text to fill the background
        RectTransform textTransform = debugText.rectTransform;
        textTransform.anchorMin = Vector2.zero;
        textTransform.anchorMax = Vector2.one;
        textTransform.sizeDelta = Vector2.zero;
        textTransform.anchoredPosition = Vector2.zero;
    }

    public static void SetDebugText(string text)
    {
        if (instance != null && instance.debugText != null)
        {
            instance.debugText.text = text;
            instance.AdjustBackgroundSize();
        }
    }

    private void AdjustBackgroundSize()
    {
        if (backgroundImage != null && debugText != null)
        {
            // Update the background size to fit the text
            Vector2 textSize = debugText.GetPreferredValues();
            backgroundImage.rectTransform.sizeDelta = textSize + new Vector2(20, 10); // Add some padding
        }
    }
}