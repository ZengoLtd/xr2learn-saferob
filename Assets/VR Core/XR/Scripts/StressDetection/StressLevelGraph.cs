using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using TMPro;

public class StressLevelGraph : Graphic
{
    public RectTransform graphContainer;
    public Sprite circleSprite;
    public Color lineColor = new Color(0.73f, 0.15f, 0.88f);  // #BA27E1 in RGB format
    public TMP_FontAsset labelFont;

    public Color gridLineColor = new Color(1f, 1f, 1f, 0.2f); // Semi-transparent white for grid lines
    public Color gradientTopColor = new Color(0.73f, 0.15f, 0.88f, 0.6f); // Top gradient color
    public Color gradientBottomColor = new Color(0.73f, 0.15f, 0.88f, 0f); // Bottom gradient color

    // Public variables for Y positions (percentage of graph height)
    [Range(0, 1)] public float lowYPosition = 0.2f;
    [Range(0, 1)] public float medYPosition = 0.5f;
    [Range(0, 1)] public float highYPosition = 0.8f;

    private void Start()
    {
        ClearPrevGraph();
        // Example data
       
        DrawGrid();
        CreateYAxisLabels();
       
        ShowGraph();
    }

    void ClearPrevGraph()
    {
       foreach (Transform child in graphContainer.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void DrawGrid()
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        int gridSize = 5; // Number of grid lines

        // Draw horizontal grid lines
        for (int i = 0; i <= gridSize; i++)
        {
            float yPos = i / (float)gridSize * graphHeight;
            CreateLine(new Vector2(0, yPos), new Vector2(graphWidth, yPos), gridLineColor);
        }

        // Draw vertical grid lines
        for (int i = 0; i <= gridSize; i++)
        {
            float xPos = i / (float)gridSize * graphWidth;
            CreateLine(new Vector2(xPos, 0), new Vector2(xPos, graphHeight), gridLineColor);
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        Image image = gameObject.GetComponent<Image>();
        image.sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);  // Adjust size as needed
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph()
    {
        List<StressSaveData> stressSaveData = StressManager.Instance.stressData;
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        GameObject lastCircle = null;

        Vector2[] points = new Vector2[stressSaveData.Count];

        for (int i = 0; i < stressSaveData.Count; i++)
        {
            float xPosition = i / (stressSaveData.Count - 1f) * graphWidth;
            float yPosition = stressSaveData[i].StressLevel * graphHeight;

            points[i] = new Vector2(xPosition, yPosition);

            // Create the line before the circle
            if (lastCircle != null)
            {
                CreateLine(lastCircle.GetComponent<RectTransform>().anchoredPosition, new Vector2(xPosition, yPosition), lineColor);
            }

            // Create and position the circle after the line
            GameObject circle = CreateCircle(new Vector2(xPosition, yPosition));

            lastCircle = circle;

            // Create the time label
            CreateLabel(stressSaveData[i].TimeStamp, new Vector2(xPosition, -20f));
        }

        // Draw the gradient under the graph line
        DrawGradient(points);
    }

    private void DrawGradient(Vector2[] points)
    {
        // Create a new UI Image for the gradient
        GameObject gradientImageObject = new GameObject("Gradient", typeof(Image));
        gradientImageObject.transform.SetParent(graphContainer, false);
        Image gradientImage = gradientImageObject.GetComponent<Image>();
        RectTransform rect = gradientImageObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);  // Bottom-left corner
        rect.anchorMax = new Vector2(0, 0);  // Bottom-left
        rect.anchoredPosition = new Vector2(0, 0);
        // Use a material with a gradient shader (you need to create this shader or use a default one)
        Material gradientMaterial = new Material(Shader.Find("UI/Default"));
        gradientImage.material = gradientMaterial;
        gradientImage.enabled = false;
        // Create a mesh for the gradient
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[points.Length * 2];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[(points.Length - 1) * 6];

        for (int i = 0; i < points.Length; i++)
        {
            vertices[i * 2] = points[i];
            vertices[i * 2 + 1] = new Vector3(points[i].x, 0, 0); // Bottom of the graph

            colors[i * 2] = gradientTopColor;
            colors[i * 2 + 0] = gradientBottomColor;
        }

        for (int i = 0; i < points.Length - 1; i++)
        {
            triangles[i * 6] = i * 2;
            triangles[i * 6 + 1] = i * 2 + 1;
            triangles[i * 6 + 2] = i * 2 + 2;

            triangles[i * 6 + 3] = i * 2 + 2;
            triangles[i * 6 + 4] = i * 2 + 1;
            triangles[i * 6 + 5] = i * 2 + 3;
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;

        gradientImageObject.AddComponent<MeshFilter>().mesh = mesh;
        gradientImageObject.AddComponent<MeshRenderer>().material = gradientMaterial;
    }

    private float GetYPositionForStressLevel(string level, float graphHeight)
    {
        switch (level)
        {
            case "Low":
                return lowYPosition * graphHeight;  // Use the adjustable lowYPosition
            case "Med":
                return medYPosition * graphHeight;  // Use the adjustable medYPosition
            case "High":
                return highYPosition * graphHeight;  // Use the adjustable highYPosition
            default:
                return 0f;
        }
    }

    private void CreateLine(Vector2 startPos, Vector2 endPos, Color lineColor)
    {
        GameObject gameObject = new GameObject("line", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        Image image = gameObject.GetComponent<Image>();
        image.color = lineColor;  // Use the provided color

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 direction = (endPos - startPos).normalized;
        float distance = Vector2.Distance(startPos, endPos);
        rectTransform.sizeDelta = new Vector2(distance, 3f);  // Adjust thickness here
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchoredPosition = startPos + direction * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(direction));
    }

    private float GetAngleFromVectorFloat(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    private void CreateLabel(string labelText, Vector2 anchoredPosition)
    {
        // Create a new GameObject and attach TextMeshProUGUI component
        GameObject label = new GameObject("Label", typeof(TextMeshProUGUI));
        label.transform.SetParent(graphContainer, false);

        // Retrieve the TextMeshProUGUI component
        TextMeshProUGUI labelTextComponent = label.GetComponent<TextMeshProUGUI>();

        // Set the text properties
        labelTextComponent.text = labelText;
        labelTextComponent.font = labelFont;
        labelTextComponent.fontSize = 14;
        labelTextComponent.color = Color.white;  // Adjust the color if necessary
        labelTextComponent.alignment = TextAlignmentOptions.Center;

        // Set the RectTransform properties
        RectTransform rectTransform = label.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(60, 20);  // Adjust size if necessary
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private void CreateYAxisLabels()
    {
        float graphHeight = graphContainer.sizeDelta.y;

        CreateLabel("L", new Vector2(-30f, lowYPosition * graphHeight));
        CreateLabel("M", new Vector2(-30f, medYPosition * graphHeight));
        CreateLabel("H", new Vector2(-30f, highYPosition * graphHeight));
    }
}