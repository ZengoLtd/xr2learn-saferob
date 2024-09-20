
public class StateHolder : BlockLogicBase
{
    public BlockState SampleState;

    public float Testvalue;
    

    void Awake(){
        SampleState.state = 12.0f;  
    }


    void Update(){
        SampleState.state = Testvalue;
    }

}
