
public class ModuleTask 
{
    public ModuleTask(){}
    public ModuleTask(bool success, string desctription){
        this.success = success;
        this.taskDescription = desctription;
    }
    public ModuleTask(bool success, string desctription, bool isSuccessStateVisible){
        this.success = success;
        this.taskDescription = desctription;
        this.isSuccessStateVisible = isSuccessStateVisible;
    }
    public bool success = false;
    public bool isSuccessStateVisible = true;
    public string taskDescription;
}
