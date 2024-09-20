using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class Connector
{
    public Port port;
    public bool input;
    public string variableName;

    public Connector(Port port, bool input, string variableName)
    {
        this.port = port;
        port.portName = variableName;
        this.input = input;
        this.variableName = variableName;
    }
}
