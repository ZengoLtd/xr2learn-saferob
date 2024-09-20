
public class ModulLogicOnclickSample : BlockLogicBase
{

    public BlockEvent OnClicked;


    void OnMouseDown()
    {
        OnClicked.Invoke(null);
    }
    public override void Logic(object data)
    {
        //
    }
}
