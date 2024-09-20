using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ConditionalTrigger), true, isFallback = true)]
public class ConditionalLogic : BlockLogicBaseInspector
{
    public VisualTreeAsset conditionalBase;
    public VisualTreeAsset conditionalButtonLogic;
    public ListView conditionalLogicContainer; // Updated to ListView

    private void AddLogicButtonCallback(ClickEvent evt)
    {
        ConditionalTrigger logicSandbox = (ConditionalTrigger)target;
        logicSandbox.conditions.Add(new ConditionData());

        EditorUtility.SetDirty(logicSandbox);
        Undo.RecordObject(logicSandbox, "Added Conditional Logic");
        PrefabUtility.RecordPrefabInstancePropertyModifications(logicSandbox);
        RedrawUI();
    }

    protected void RedrawUI()
    {
        ConditionalTrigger logicSandbox = (ConditionalTrigger)target;
        base.RedrawUI();

        // Load UXML and instantiate the ListView
        conditionalLogicContainer = (ListView)myInspector.Q("conditionalLogic");

        // Setting up the ListView
        conditionalLogicContainer.itemsSource = logicSandbox.conditions;
        conditionalLogicContainer.makeItem = () => {
            return conditionalLogicXML.CloneTree(); // Create a new instance of the UI template for each item
        };

        conditionalLogicContainer.bindItem = (element, index) => {
            ConditionData data = logicSandbox.conditions[index];
            BindConditionElement(element, data, logicSandbox, index);
        };

        conditionalLogicContainer.selectionType = SelectionType.None;

        // Register callback for add button
        Button addLogicButton = myInspector.Q<Button>("AddLogicButton");
        addLogicButton.style.display = DisplayStyle.Flex;
        addLogicButton.UnregisterCallback<ClickEvent>(AddLogicButtonCallback); // Ensure no duplicate callbacks
        addLogicButton.RegisterCallback<ClickEvent>(AddLogicButtonCallback);
    }

    private void BindConditionElement(VisualElement element, ConditionData data, ConditionalTrigger logicSandbox, int index)
    {
        // Dropdown for state selection
        DropdownField stateSelector = element.Q<DropdownField>("StateSelector");
        stateSelector.choices.Clear();

        foreach (var action in ((BlockLogicBase)target).ActivateOn)
        {
            foreach (var state in action.states)
            {
                stateSelector.choices.Add(action.block.gameObject.name + "." + action.block.ToString() + "." + state);
            }
        }

        if (data.block != null && data.state != null && data.state != "")
        {
            stateSelector.SetValueWithoutNotify(data.block.gameObject.name + "." + data.block.ToString() + "." + data.state);
        }

        stateSelector.RegisterValueChangedCallback(evt =>
        {
            string[] split = evt.newValue.Split('.');
            data.block = ((BlockLogicBase)target).ActivateOn.Find(x => x.block.gameObject.name == split[0] && x.states.Contains(split[2])).block;
            data.state = split[2];
            UpdateLogicSandbox(logicSandbox);
        });

        // Text field for value
        TextField valueField = element.Q<TextField>("ValueField");
        valueField.SetValueWithoutNotify(data.value);
        valueField.RegisterValueChangedCallback(evt =>
        {
            data.value = evt.newValue;
            UpdateLogicSandbox(logicSandbox);
        });

        // Enum fields for condition and logic
        EnumField conditionField = element.Q<EnumField>("ConditionEnum");
        conditionField.SetValueWithoutNotify(data.condition);
        conditionField.RegisterValueChangedCallback(evt =>
        {
            data.condition = (Condition)evt.newValue;
            UpdateLogicSandbox(logicSandbox);
        });

        EnumField logicField = element.Q<EnumField>("ConditionLogic");
        logicField.SetValueWithoutNotify(data.logic);
        logicField.RegisterValueChangedCallback(evt =>
        {
            data.logic = (ConditionLogic)evt.newValue;
            UpdateLogicSandbox(logicSandbox);
        });

        // Remove button
        Button removeLogicButton = element.Q<Button>("RemoveLogicButton");
        removeLogicButton.RegisterCallback<ClickEvent>(evt =>
        {
            logicSandbox.conditions.RemoveAt(index);
            UpdateLogicSandbox(logicSandbox);
            RedrawUI();
        });

        // Hide logic field for the last condition
        if (index == logicSandbox.conditions.Count - 1)
        {
            logicField.style.display = DisplayStyle.None;
        }
        else
        {
            logicField.style.display = DisplayStyle.Flex;
        }
    }

    private void UpdateLogicSandbox(ConditionalTrigger logicSandbox)
    {
        EditorUtility.SetDirty(logicSandbox);
        Undo.RecordObject(logicSandbox, "Updated Conditional Logic");
        PrefabUtility.RecordPrefabInstancePropertyModifications(logicSandbox);
    }

    public override VisualElement CreateInspectorGUI()
    {
        base.CreateInspectorGUI();
        conditionalLogicContainer = (ListView)myInspector.Q("conditionalLogic");
        RedrawUI();
        return myInspector;
    }
}
