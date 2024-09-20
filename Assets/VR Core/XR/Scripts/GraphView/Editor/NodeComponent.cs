
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace DS.Elements{

    public class NodeComponent : Node
    {
        private GameObject gameObject;
        public List<Connector> connectors = new List<Connector>();

        public string componentType;
        
        public Port AddConnector( string variable, bool input){
            Port port;
            if(input){
                port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            }else{
                port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            
            this.connectors.Add(new Connector(port, input, variable));
            return port;
        }
        public VisualElement GetContainer(){
            VisualElement container = new VisualElement();
            container.style.backgroundColor = Color.gray;
            VisualElement inputholder = new VisualElement();
            inputholder.style.borderTopWidth = 1;
            inputholder.style.borderTopColor = Color.black;
          
            VisualElement outputholder = new VisualElement();
         
            foreach(Connector connector in connectors){
                if(connector.input){
                    inputholder.Add(connector.port);
                    inputholder.style.color = Color.black;
                    inputholder.style.unityFontStyleAndWeight = FontStyle.Bold;
                }else{
                    outputholder.Add(connector.port);
                }
            }
            TextElement nameField = new TextElement(){
                text = componentType
            };
            container.Add(nameField);
            container.Add(inputholder);
            container.Add(outputholder);


            return container;
        }
    }
}
