using System;
using System.Collections.Generic;
using UnityEngine;


public class ConditionalTrigger : BlockLogicBase
{
    [SerializeField]
    public List<ConditionData> conditions = new List<ConditionData>();

    void Awake()
    {
        foreach (ConditionData condition in conditions)
        {
            BlockLogicBase block = ActivateOn.Find(x => x.block == condition.block && x.states.Contains(condition.state)).block; //TODO: refactor for instace id 
            BlockState a = GetPublicState(block, condition.state);
            Debug.Log(a.state);
        }
    }

    public bool EvaluateState(ConditionData data, BlockState state)
    {
        if (state == null)
        {
            Debug.LogError("Conditional Trigger, State not found");
            return false;
        }
        if (state.state == null)
        {
            Debug.LogError("Conditional Trigger, State.state not found");
            return false;
        }
        if (data == null)
        {
            Debug.LogError("Conditional Trigger, ConditionData not found");
            return false;
        }

        try
        {

            var ret = state.CompareState(data.condition, data.value);
            Debug.Log("Comparing: " + state.state + " " + data.condition + " " + data.value + " = " + ret);
            return ret;
        }
        catch (Exception ex)
        {
            Debug.LogError("Conditional trigger Error: " + ex.Message);
            return false;
        }
    }

    bool EvaluateBlock()
    {
        bool totalResult = false;

        for (int i = 0; i < conditions.Count; i++)
        {
            ConditionData condition = conditions[i];
            BlockLogicBase block = ActivateOn.Find(x => x.block == condition.block && x.states.Contains(condition.state)).block;
            if (i == 0)
            {
                totalResult = EvaluateState(condition, GetPublicState(block, condition.state));
                continue;
            }
            bool result = EvaluateState(condition, GetPublicState(block, condition.state));
            switch (conditions[i - 1].logic)
            {
                case ConditionLogic.And:
                    totalResult = totalResult && result;
                    break;
                case ConditionLogic.Or:
                    totalResult = totalResult || result;
                    if (totalResult)
                    {
                        return true;
                    }
                    break;
            }
        }
        return totalResult;
    }

    public override void Logic(object data)
    {
        Debug.Log("Evaluating block [" + transform.name + "]");

        if (EvaluateBlock())
        {
            Debug.Log("Evaluated as true [" + transform.name + "]");

            OnTriggered.Invoke(null);
        }
    }

}
