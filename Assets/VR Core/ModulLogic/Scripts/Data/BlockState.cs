
using UnityEngine;
using UnityEngine.Events;
using System;
using Zengo.Inventory;

[Serializable]
public class BlockState
{
    [SerializeField]
    private object _state;
    //state getter
    public object state
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            OnStateChange?.Invoke(value);
        }
    }

    [SerializeField]
    public UnityAction<object> OnStateChange;

    public bool CompareState(Condition condition, string _other){
       
        if(this.state.GetType() == typeof(bool)){
            return CompareAsBool(condition, _other);
        }
        else if(this.state.GetType() == typeof(int) || this.state.GetType() == typeof(float) || this.state.GetType() == typeof(double)){
            return CompareAsNumeric(condition, _other);
        }
        else if(this.state.GetType() == typeof(string)){
            return CompareAsString(condition, _other);
        }
        else{
            Debug.LogError("Type not supported");
        }
        return false;
    }
    private bool CompareAsBool(Condition condition, string _other){
       
        bool other = false;
        if(_other.ToLower() == "true"){
            other = true;
        }

        bool _this = (bool)this.state;
        switch (condition)
        {
            case Condition.Equal:
                return _this == other;
            case Condition.NotEqual:
                return _this != other;
            default:
                Debug.LogError("Condition not supported");
                return false;
        }
    }
    private bool CompareAsNumeric(Condition condition, string _other){
        Debug.Log("comparing CompareAsNumeric");
        double other = double.Parse(_other);
        double _this = Convert.ToDouble(this.state);
        switch (condition)
        {
            case Condition.Equal:
                return _this == other;
            case Condition.NotEqual:
                return _this != other;
            case Condition.Greater:
                return _this > other;
            case Condition.Less:
                return _this < other;
            case Condition.GreaterEqual:
                return _this >= other;
            case Condition.LessEqual:
                return _this <= other;
            default:
                Debug.LogError("Condition not supported");
                return false;
        }
    }
    private bool CompareAsString(Condition condition, string _other){
        Debug.Log("comparing CompareAsString");
        string other = _other;
        string _this = (string)this.state;
        switch (condition)
        {
            case Condition.Equal:
                return _this == other;
            case Condition.NotEqual:
                return _this != other;
            default:
                Debug.LogError("Condition not supported");
                return false;
        }
    }


}
