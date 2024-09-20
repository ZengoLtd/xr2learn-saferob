
// editor crash counter : 5
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[CustomEditor(typeof(BlockLogicBase), true, isFallback = true)]

public class BlockLogicBaseInspector : Editor
{
    public VisualElement myInspector;
    public VisualTreeAsset blockLogicBaseXML;
    public VisualTreeAsset labelButtonXML;
    public VisualTreeAsset blockBindingsXML;
    public VisualTreeAsset conditionalLogicXML;

    private Button bindingsTabButton;
    private Button logicTabButton;
    private Button listenersTabButton;
    private VisualElement bindingsContent;
    private VisualElement logicContent;
    private VisualElement listenersContent;
    private VisualElement activeContent;

    //get Listeners
    List<BlockLogicBase> GetListeners()
    {
        var returndata = new List<BlockLogicBase>();
        foreach (BlockLogicBase other in GetBlocks())
        {
            if (other.ActivateOn.Find(x => x.block == target) != null)
            {
                returndata.Add(other);
            }
        }
        return returndata;
    }

    List<BlockLogicBase> GetBlocks()
    {
        var returndata = new List<BlockLogicBase>();

        var blocks = FindObjectsOfType<BlockLogicBase>();
        foreach (var block in blocks)
        {
            returndata.Add(block);
        }
        return returndata;
    }


    public static Dictionary<string, BlockEvent> GetPublicActions(object obj)
    {
        var actions = new Dictionary<string, BlockEvent>();
        if (obj == null)
        {
            return actions;
        }
        var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(BlockEvent))
            {
                if(actions.ContainsKey(field.Name)){
                    continue;
                }
                actions.Add(field.Name, (BlockEvent)field.GetValue(obj));

            }
        }
        return actions;
    }

    public static Dictionary<string, BlockState> GetPublicStates(object obj)
    {
        var states = new Dictionary<string, BlockState>();
        if (obj == null)
        {
            return states;
        }
        var fields = obj?.GetType()?.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(BlockState))
            {
                states.Add(field.Name, (BlockState)field.GetValue(obj));

            }
        }
        return states;
    }

    void RenderEventDropdown(BlockEventDataForListener data, DropdownField dropdown)
    {

        Debug.Log("RenderEventDropdown");
        //no object to get actions from
        if (data.block == null)
        {
            return;
        }
        BlockLogicBase logicSandbox = (BlockLogicBase)target;

        var OtherObjectactions = GetPublicActions(data.block);
        dropdown.choices.Add("None");

        //get data from logicsandbox.activateon where gameobject equals to data.gameobject
        BlockEventDataForListener listender = logicSandbox.ActivateOn.Find(x => x.block == data.block);
        if (listender != null)
        {
            foreach (var action in OtherObjectactions)
            {
                if (listender.actions.Contains(action.Key))
                {
                    continue;
                }
                else
                {
                    dropdown.choices.Add(action.Key);
                }
            }
        }
    }

    void RenderStateDropdown(BlockEventDataForListener data, DropdownField dropdown)
    {
        //no object to get actions from
        if (data.block == null)
        {
            return;
        }
         Debug.Log("RenderStateDropdown not null");
        BlockLogicBase logicSandbox = (BlockLogicBase)target;

        var OtherObjectactions = GetPublicStates(data.block);
        dropdown.choices.Add("None");

        //get data from logicsandbox.activateon where gameobject equals to data.gameobject
        BlockEventDataForListener listener = logicSandbox.ActivateOn.Find(x => x.block == data.block);
        if (listener != null)
        {
            foreach (var action in OtherObjectactions)
            {
                if (listener.actions.Contains(action.Key))
                {
                    continue;
                }
                else
                {
                    dropdown.choices.Add(action.Key);
                }
            }
        }
    }

    void RemoveEventFromSubscribedList(BlockEventDataForListener data, string action)
    {
        BlockLogicBase logicSandbox = (BlockLogicBase)target;

        data.actions.Remove(action);
        EditorUtility.SetDirty(logicSandbox);
        Undo.RecordObject(logicSandbox, "Changed");
        PrefabUtility.RecordPrefabInstancePropertyModifications(logicSandbox);
        RedrawUI();
    }

    void RenderStateList(VisualElement container, BlockEventDataForListener data)
    {
        foreach (string state in data.states)
        {
            //Debug.Log("action:"+action);
            labelButtonXML.CloneTree(container);
            Label label = (Label)container.Q("label");
            Button button = (Button)container.Q("button");
            label.name += System.Guid.NewGuid();
            button.name += System.Guid.NewGuid();
            label.text = state;
            button.RegisterCallback<ClickEvent>(evt =>
            {
                //RemoveEventFromSubscribedList(data,state);
            });
        }
        //Debug.Log("RenderEventList END");
    }

    void RenderEventList(VisualElement container, BlockEventDataForListener data)
    {
       

        foreach (string action in data.actions)
        {
            //Debug.Log("action:"+action);
            labelButtonXML.CloneTree(container);
            Label label = (Label)container.Q("label");
            Button button = (Button)container.Q("button");
            label.name += System.Guid.NewGuid();
            button.name += System.Guid.NewGuid();
            label.text = action;
            button.RegisterCallback<ClickEvent>(evt =>
            {
                RemoveEventFromSubscribedList(data, action);
            });
        }
        //Debug.Log("RenderEventList END");
    }

    void BlockListGameObjectFieldValueChanged(BlockLogicBase block, BlockEventDataForListener data)
    {

        data.block = block;
        EditorUtility.SetDirty(target);
        Undo.RecordObject(target, "Changed");
        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
        RedrawUI();
    }

    private void SetupTabSystem()
    {
        bindingsTabButton = myInspector.Q<Button>("bindingsButton");
        logicTabButton = myInspector.Q<Button>("logicTabButton");
        listenersTabButton = myInspector.Q<Button>("listenersTabButton");

        bindingsContent = myInspector.Q<VisualElement>("bindingsContent");
        logicContent = myInspector.Q<VisualElement>("logicContent");
        listenersContent = myInspector.Q<VisualElement>("listenersContent");

        bindingsTabButton.clicked += () => SwitchTab(bindingsContent);
        logicTabButton.clicked += () => SwitchTab(logicContent);
        listenersTabButton.clicked += () => SwitchTab(listenersContent);

        // Set default active tab if not already set
        if (activeContent == null)
        {
            SwitchTab(bindingsContent);
        }
    }

    private void SwitchTab(VisualElement newActiveContent)
    {
        bindingsContent.style.display = DisplayStyle.None;
        logicContent.style.display = DisplayStyle.None;
        listenersContent.style.display = DisplayStyle.None;

        newActiveContent.style.display = DisplayStyle.Flex;
        activeContent = newActiveContent;

        // Update button styles
        UpdateTabButtonStyle(bindingsTabButton, newActiveContent == bindingsContent);
        UpdateTabButtonStyle(logicTabButton, newActiveContent == logicContent);
        UpdateTabButtonStyle(listenersTabButton, newActiveContent == listenersContent);
    }

    private void UpdateTabButtonStyle(Button button, bool isActive)
    {
        if (isActive)
        {
            button.AddToClassList("active-tab");
        }
        else
        {
            button.RemoveFromClassList("active-tab");
        }
    }

    public void StatedropdownValueChanged(ChangeEvent<string> evt, BlockEventDataForListener data, BlockLogicBase _gameobject)
    {

        BlockLogicBase logicSandbox = (BlockLogicBase)target;
       
        data.states.Add(evt.newValue);
        EditorUtility.SetDirty(logicSandbox);
        Undo.RecordObject(logicSandbox, "Changed");
        PrefabUtility.RecordPrefabInstancePropertyModifications(logicSandbox);
        RedrawUI();
    }

    public void EventdropdownValueChanged(ChangeEvent<string> evt, BlockEventDataForListener data)
    {
        BlockLogicBase logicSandbox = (BlockLogicBase)target;
       
        data.actions.Add(evt.newValue);
        EditorUtility.SetDirty(logicSandbox);
        Undo.RecordObject(logicSandbox, "Changed");
        PrefabUtility.RecordPrefabInstancePropertyModifications(logicSandbox);
        RedrawUI();
    }

    void CreateBlockLogic(VisualElement myInspector, BlockEventDataForListener data)
    {
        BlockLogicBase logicSandbox = (BlockLogicBase)target;
        VisualElement blockBindigsContainer = (VisualElement)myInspector.Q("blockLogic");

        blockBindingsXML.CloneTree(blockBindigsContainer);
        VisualElement EventHolderContainer = (VisualElement)blockBindigsContainer.Q("EventHolder");
        VisualElement StateHolderContainer = (VisualElement)blockBindigsContainer.Q("StateHolder");


        Button blockBindigsRemoveButton = (Button)blockBindigsContainer.Q("removeButton");
        DropdownField eventdropdown = (DropdownField)myInspector.Q("eventdropdown");
        DropdownField statedropdown = (DropdownField)myInspector.Q("statedropdown");
        ObjectField gamebojectContainer = (ObjectField)blockBindigsContainer.Q("gameobjectfield");

        EventHolderContainer.name += System.Guid.NewGuid();
        blockBindigsRemoveButton.name += System.Guid.NewGuid();
        eventdropdown.name += System.Guid.NewGuid();
        statedropdown.name += System.Guid.NewGuid();
        gamebojectContainer.name += System.Guid.NewGuid();
        gamebojectContainer.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue.GetType() == typeof(BlockLogicBase))
            {
                BlockListGameObjectFieldValueChanged((BlockLogicBase)evt.newValue, data);
            }
            else
            {
                if(evt.newValue as GameObject != null){
                    BlockListGameObjectFieldValueChanged(((GameObject)evt.newValue).GetComponent<BlockLogicBase>(), data);
                }else{
                    BlockListGameObjectFieldValueChanged((BlockLogicBase)evt.newValue, data);
                }
            }

        });
        var _gameobject = data.block;
        if (_gameobject != null)
        {

            gamebojectContainer.SetValueWithoutNotify(_gameobject);
            RenderEventList(EventHolderContainer, data);
            RenderStateList(StateHolderContainer, data);
            RenderEventDropdown(data, eventdropdown);
            RenderStateDropdown(data, statedropdown);
            eventdropdown.RegisterValueChangedCallback(evt =>
            {
                EventdropdownValueChanged(evt, data);
            });
            statedropdown.RegisterValueChangedCallback(evt =>
            {
                Debug.Log("statedropdownValueChanged");
                Debug.Log(evt.newValue);
                Debug.Log(data);
                Debug.Log(_gameobject);
                
                if(data.gameObject == _gameobject){
                   
                }
                Debug.Log("=======================");
                Debug.Log(data.gameObject);
                Debug.Log(_gameobject);
                data.gameObject = _gameobject.gameObject;
                Debug.Log("=======================");
                StatedropdownValueChanged(evt, data,_gameobject);
            });

        }

        blockBindigsRemoveButton.RegisterCallback<ClickEvent>(evt => RemoveGameobjectBlock(evt, data));

    }

    private void RemoveGameobjectBlock(ClickEvent evt, BlockEventDataForListener data)
    {
        EditorUtility.SetDirty(target);
        Undo.RecordObject((BlockLogicBase)target, "Changed");
        PrefabUtility.RecordPrefabInstancePropertyModifications((BlockLogicBase)target);

        ((BlockLogicBase)target).ActivateOn.Remove(data);

        RedrawUI();
    }

    private void AddGameObjectButtonCallback(ClickEvent evt)
    {
        BlockLogicBase logicSandbox = (BlockLogicBase)target;
        logicSandbox.ActivateOn.Add(new BlockEventDataForListener());
        RedrawUI();
    }


    protected void RedrawUI()
    {
        VisualElement previousActiveContent = activeContent;
        myInspector.Clear();
        BlockLogicBase logicSandbox = (BlockLogicBase)target;
        blockLogicBaseXML.CloneTree(myInspector);


        bool showFixButton = false;
        foreach (BlockEventDataForListener data in logicSandbox.ActivateOn)
        {
            if (data.gameObject != null && data.block == null)
            {
                showFixButton = true;
            }
        }

        Button fixbutton = (Button)myInspector.Q("FIXBUTTON");
        fixbutton.RegisterCallback<ClickEvent>(evt => FixObject());
        if (showFixButton)
        {
            fixbutton.style.display = DisplayStyle.Flex;
        }
        else
        {
            fixbutton.style.display = DisplayStyle.None;
        }

        SetupTabSystem();

        if (previousActiveContent != null)
        {
            string contentName = previousActiveContent.name;
            VisualElement newActiveContent = myInspector.Q(contentName);
            if (newActiveContent != null)
            {
                SwitchTab(newActiveContent);
            }
        }

        foreach (var data in logicSandbox.ActivateOn)
        {
            CreateBlockLogic(myInspector, data);
        }

        ((Button)myInspector.Q("AddGameObjectButton")).RegisterCallback<ClickEvent>(AddGameObjectButtonCallback);

        ListView eventlistView = (ListView)myInspector.Q("eventlistView");
        ListView statelistView = (ListView)myInspector.Q("statelistView");

        var actions = GetPublicActions(logicSandbox);

        foreach (var action in actions)
        {
            Label actionlabel = new Label();
            actionlabel.text = "  " + action.Key + ":";
            eventlistView.hierarchy.Add(actionlabel);
            actionlabel.style.unityFontStyleAndWeight = FontStyle.Bold;

            foreach (var listener in GetListeners())
            {
                foreach (var listenerAction in listener.ActivateOn)
                {
                    if (listenerAction.actions.Contains(action.Key))
                    {

                        ObjectField gamebojectContainer = new ObjectField();
                        gamebojectContainer.name += System.Guid.NewGuid();
                        gamebojectContainer.objectType = typeof(BlockLogicBase);
                        gamebojectContainer.value = listener?.gameObject;
                        gamebojectContainer.SetEnabled(false);
                        gamebojectContainer.style.marginLeft = 20;
                        eventlistView.hierarchy.Add(gamebojectContainer);
                    }
                }
            }
        }
        var states = GetPublicStates(logicSandbox);

        foreach (var state in states)
        {
            Label statelabel = new Label();
            statelabel.text = "  " + state.Key + ":";
            statelistView.hierarchy.Add(statelabel);
            statelabel.style.unityFontStyleAndWeight = FontStyle.Bold;

            foreach (var listener in GetListeners())
            {
                foreach (var listenerAction in listener.ActivateOn)
                {
                    if (listenerAction.states.Contains(state.Key))
                    {

                        ObjectField gamebojectContainer = new ObjectField();
                        gamebojectContainer.name += System.Guid.NewGuid();
                        gamebojectContainer.objectType = typeof(GameObject);
                        gamebojectContainer.value = listener.gameObject;
                        gamebojectContainer.SetEnabled(false);
                        gamebojectContainer.style.marginLeft = 20;
                        statelistView.hierarchy.Add(gamebojectContainer);
                    }
                }
            }
        }
    }
   
    public override VisualElement CreateInspectorGUI()
    {
        BlockLogicBase logicSandbox = (BlockLogicBase)target;
        // Create a new VisualElement to be the root of our Inspector UI.
        myInspector = new VisualElement();
        blockLogicBaseXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VR Core/ModulLogic/UI/BlockLogicBase.uxml");
        labelButtonXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VR Core/ModulLogic/UI/LabelButton.uxml");
        blockBindingsXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VR Core/ModulLogic/UI/BlockBindings.uxml");
        conditionalLogicXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VR Core/ModulLogic/UI/ConditionalLogic.uxml");

        RedrawUI();
        SetupTabSystem();

        var defaultInspector = myInspector.Q<IMGUIContainer>();
        defaultInspector.onGUIHandler = () => DrawDefaultInspector();

      
        
        return myInspector;
    }


    void OnSceneGUI()
    {
        Vector3 center = ((BlockLogicBase)target).transform.position;
        foreach (BlockEventDataForListener data in ((BlockLogicBase)target).ActivateOn)
        {
            if (data.block)
            {
                Handles.color = Color.red;
                Handles.DrawLine(center, data.block.gameObject.transform.position);
            }
        }
    }


    void FixObject()
    {
        BlockLogicBase logicSandbox = (BlockLogicBase)target;

        foreach (BlockEventDataForListener data in logicSandbox.ActivateOn)
        {
            BlockLogicBase logicBase = data.block;
            data.block = data.gameObject.GetComponent<BlockLogicBase>();
        }
        EditorUtility.SetDirty(logicSandbox);
        Undo.RecordObject(logicSandbox, "Changed");
        PrefabUtility.RecordPrefabInstancePropertyModifications(logicSandbox);
        RedrawUI();
    }

    

}


