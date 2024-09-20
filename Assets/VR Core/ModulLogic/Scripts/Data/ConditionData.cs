

using System;

[Serializable]
public class ConditionData{
    public BlockLogicBase block;
    public string state;
    public Condition condition;
    public string value;
    public ConditionLogic logic; 
}