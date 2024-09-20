using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.VisualScripting;
using MEM;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace DS.Windows{

    using Elements;
    struct EdgePoints{
    public string variable;
    public Port port;

    }
public class DSGraphView : GraphView
{
     List<DSNode> nodes ;
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach(port =>
        {
            if (startPort == port)
            {
                return;
            }
            if(startPort.node == port.node)
            {
                return;
            }
            if(startPort.direction == port.direction)
            {
                return;
            }
            compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }
    DSNode CreateNode(Vector2 position,string name, GameObject gameObject){
        DSNode node = new DSNode();
        node.Initialize(position,name, gameObject);
        node.Draw();
        AddElement(node);
        return node;
    }
    void AddStyles(){
        StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("ModuleGraph/GraphViewStyles.uss");
        styleSheets.Add(styleSheet);
    }
    void AddManipulators(){
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new SelectionDragger()); 
    }


  void ConnectPorts(Port from, Port to){
        var edge = new Edge();
        edge.output = from;
        edge.input = to;
        from.Connect(edge);
        to.Connect(edge);
        from.ConnectTo<Edge>(to);
        AddElement(edge);
    }

    public DSGraphView(){
        AddGridBackground();
        AddManipulators();
        AddStyles();
        GetPossibleObjects();
    }

    List<ModuleEventBase> GetEventbases(GameObject gameObject){
        List<ModuleEventBase> eventBases = new List<ModuleEventBase>();
        List<GameObject> checkables = new List<GameObject>();
        checkables.Add(gameObject);
        while(checkables.Count > 0){
            GameObject checkable = checkables[0];
            if(checkable.transform.childCount>0){
                for(int i = 0; i < checkable.transform.childCount; i++){
                    checkables.Add(checkable.transform.GetChild(i).gameObject);
                }
            }
            if(checkable.GetComponent<ModuleEventBase>() != null){
                var bases = checkable.GetComponents<ModuleEventBase>();
                foreach(var basee in bases){
                    eventBases.Add(basee);
                }
            }
            checkables.RemoveAt(0);
            
        }
        return eventBases;
    }

    void GetPossibleObjects(){
        
       Transform root = GameObject.Find("ModuleLogic").transform;
       nodes = new List<DSNode>();
       List<EdgePoints> inputPoints = new List<EdgePoints>();
       List<EdgePoints> outputPoints = new List<EdgePoints>();
        /*
            Get all root objects, create nodes from them, get components and add ports based on variables.
        */
        foreach(Transform child in root){
           var node = CreateNode(new Vector2(0,50),child.name,child.gameObject);

           node.Draw();
           nodes.Add(node);
           
           foreach(var componentBase in GetEventbases(child.gameObject)){

                NodeComponent component = new NodeComponent();
                if(componentBase is ModuleEventListenerBase){
                    component.componentType = "[L]"+componentBase.GetType().ToString();
             
                 //  foreach(var variable in GetVariables(componentBase.variables)){
                 //     var port = component.AddConnector(variable,true);
                 //     inputPoints.Add(new EdgePoints{variable = variable, port = port});
                 //  }
                    
                }
                if(componentBase is ModuleEventTriggerBase){
                    component.componentType = "[T]"+componentBase.GetType().ToString();
                    
                   // foreach(var variable in GetVariables(componentBase.variables)){
                   //    var port = component.AddConnector(variable,false);
                   //    outputPoints.Add(new EdgePoints{variable = variable, port = port});
                   // }
                }

                node.AddComponent(component);
                
           }
        } 
      
        /*
            Connect nodes based on the connections in the components
        */
     
        foreach(var output in outputPoints){
            foreach(var input in inputPoints){
                if(output.variable == input.variable){
                    ConnectPorts(output.port,input.port);
                }
            }
        }
        Arrange();


        
    }
    public void Arrange(){
        float offset = 0;
        for(int i =0; i<nodes.Count;i++){
            offset+=nodes[i].layout.width + 10.0f;
            nodes[i].SetPosition(new Rect(new Vector2(offset,50),Vector2.zero));
            
        }
      
    }
// List<string> GetVariables(Variables variable){
//     if(variable == null){
//         return new List<string>();
//     }
//     
//     List<string> variables = new List<string>();
//     var declarations = variable.declarations;
//     //this should be only one element in most cases
//     foreach (var declaration in declarations)
//     {
//         variables.Add(declaration.name); 
//     }
//     return variables;
// }
//
    void AddGridBackground(){
       GridBackground gridBackground = new GridBackground();
       gridBackground.StretchToParentSize();
       Insert(0,gridBackground);
    }
}
}