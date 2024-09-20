
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;


namespace DS.Elements{
    
    public class DSNode : Node
    {
        public string Name;

        public List<NodeComponent> components = new List<NodeComponent>();

        public List<string> ListeningVariables = new List<string>();
        public List<string> TriggerVariables = new List<string>();
        
        public Port outputPort;
        public Port inputPort;
        public Vector2 Position;
    
        private GameObject gameObject;
        public void Initialize(Vector2 position,string name, GameObject gameObject){
            this.gameObject = gameObject;
            Name = name;
            
            Position = position;

            
            /*outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Input";
            outputPort.portName = "Output";
*/

            var singleclick = new Clickable(OnSingleClick);
            singleclick.activators.Clear();
            singleclick.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, clickCount = 1 });
            
            this.AddManipulator(singleclick);   
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("ModuleGraph/GraphViewStyles.uss");
            styleSheets.Add(styleSheet);

        }
        public void AddComponent(NodeComponent component){
            components.Add(component);
            Draw();
        }
        void OnSingleClick(){
             ShowGameobject();
      
        }
        public static EditorWindow[] GetAllOpenEditorWindows()
        {
            return Resources.FindObjectsOfTypeAll<EditorWindow>();
        }
        public static EditorWindow GetCurrentWindow(){
            foreach (EditorWindow window in GetAllOpenEditorWindows())
            {
                if(window.hasFocus){
                    
                   return window;
                }
               
            }
            return null;
        }
        private void ShowGameobject(){
            Selection.objects = new Object[] { gameObject };
        }
       
        public void Draw(){
            titleContainer.Clear();
            inputContainer.Clear();
            outputContainer.Clear();
            extensionContainer.Clear();

            TextElement nameField = new TextElement(){
                text = Name
            };
            /*
            if(TriggerVariables.Count>0){
                extensionContainer.Add(
                    new Label(){
                        text = "Triggers"
                    }
                );

                VisualElement TriggerContainer = new VisualElement();

                foreach(var variable in TriggerVariables){
                    var text = new Label(){
                        text = variable
                    };
                    TriggerContainer.Insert(0,text);
                }
                TriggerContainer.style.borderTopWidth = 1;
                TriggerContainer.style.borderTopColor = Color.black;
                TriggerContainer.style.color = Color.black;
                TriggerContainer.style.unityFontStyleAndWeight = FontStyle.Bold;
                TriggerContainer.style.backgroundColor = new Color(254/255.0f,229/255.0f,173/255.0f);
                extensionContainer.Add(TriggerContainer);
            }
            

            if(ListeningVariables.Count>0){
                extensionContainer.Add(
                    new Label(){
                        text = "Listeners"
                    }
                );
                VisualElement ListenerContainer = new VisualElement();
                ListenerContainer.style.borderTopWidth = 1;
                ListenerContainer.style.borderTopColor = Color.black;
                ListenerContainer.style.color = Color.black;
                ListenerContainer.style.unityFontStyleAndWeight = FontStyle.Bold;
                ListenerContainer.style.backgroundColor = new Color(172/255.0f,206/255.0f,192/255.0f);
            
                foreach(var variable in ListeningVariables){
                    var text = new Label(){
                        text = variable
                    };
                    ListenerContainer.Insert(0,text);
                }
                

            }
*/
            
       
           // inputContainer.Add(inputPort);
           // outputContainer.Add(outputPort);
            nameField.style.fontSize = 14;
            titleContainer.Insert(0,nameField);
            titleContainer.style.alignContent = Align.Center;
            titleContainer.style.alignItems = Align.Center;
            titleContainer.style.justifyContent = Justify.Center;
            titleContainer.style.unityFontStyleAndWeight = FontStyle.Bold;
            foreach(var component in components){
               extensionContainer.Add(component.GetContainer());
            }  
            RefreshExpandedState();
           
        }
    }
 }
