using UnityEngine;

public class ConveyorShaderController : MonoBehaviour
{
    private string ConveyorSpeedProperty = "_ScrollSpeed";
    public GameObject conveyorBelt;
    private ConveyorMechanics conveyorMechanics;
    
    private MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    { 
        renderer = GetComponent<MeshRenderer>();
        conveyorMechanics = conveyorBelt.GetComponent<ConveyorMechanics>();
    }

    // Update is called once per frame
    void Update()
    {
        if (renderer != null)
        {
            if (renderer.sharedMaterial.HasProperty(ConveyorSpeedProperty))
            {
                if (conveyorMechanics.GetbeltIsOn())
                {
                    renderer.sharedMaterial.SetVector(ConveyorSpeedProperty, new Vector2(conveyorMechanics.speed/10, 0.0f));
                }
                else
                {
                    renderer.sharedMaterial.SetVector(ConveyorSpeedProperty, new Vector2(0.0f, 0.0f));
                }
            }
        }
        
    }
}
