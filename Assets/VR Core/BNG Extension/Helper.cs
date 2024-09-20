using System;
using System.Reflection;


namespace BNGExtension{
internal static class Helper{
    
    //THIS METHOD IS ONLY FOR EXTENSION OF THE FRAMEWORK
    //DO NOT CALL IT IN ANY UPDATE 
    //This could be optimized if needed https://www.youtube.com/watch?v=er9nD-usM1A
    
    internal static object ReflectionValue(Type type, string filed, object obj){
        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        FieldInfo field = type.GetField(filed, bindFlags);
        return field.GetValue(obj);
    }  
}
}