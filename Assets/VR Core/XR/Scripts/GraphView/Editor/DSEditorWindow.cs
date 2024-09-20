using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace DS.Windows{

public class DSEditorWindow : EditorWindow
{

     UnityEditor.UIElements.Toolbar m_Toolbar;
      protected VisualElement m_ToolbarContainer;
       DSGraphView grapView;
     [MenuItem("Window/DS/GraphView")]
     public static void Open(){
          GetWindow<DSEditorWindow>("Module Logic");
}
     void OnEnable(){
          AddGraphView();  
          AddToolBar();
          

     }
     void AddToolBar(){
          m_Toolbar = new UnityEditor.UIElements.Toolbar();
          m_Toolbar.style.flexGrow = 1;
          m_Toolbar.style.overflow = Overflow.Hidden;
          m_ToolbarContainer = new VisualElement();
          m_ToolbarContainer.style.flexDirection = FlexDirection.Row;
          m_ToolbarContainer.Add(m_Toolbar);
          rootVisualElement.Add(m_ToolbarContainer);
          var b = new UnityEditor.UIElements.ToolbarButton();
          b.text = "Save";
          b.clickable.clicked += () => {
               Debug.Log("SaveGraph");
          };
          
          m_Toolbar.Add(b);
          b = new UnityEditor.UIElements.ToolbarButton();
          b.text = "Arrange";
          b.clickable.clicked += () => {
              grapView.Arrange();
          };
          
          m_Toolbar.Add(b);
     }
     void AddGraphView(){
          grapView = new DSGraphView(); 

          grapView.StretchToParentSize();
          rootVisualElement.Add(grapView); 
     }
}


}
