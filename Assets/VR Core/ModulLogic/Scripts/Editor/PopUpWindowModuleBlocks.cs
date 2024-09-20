using CodiceApp.EventTracking.Plastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpWindowModuleBlocks : EditorWindow
{
    private Label statusLabel;
    private VisualTreeAsset radioToggleXML;
    private VisualTreeAsset visualTreeXML;
    private ObjectField selectedGameObjectField;
    private ObjectField selectedEndGameObjectField;
    private Button lockButton;
    private bool isLocked = false;

    public static event Action<GameObject> OnComponentAddedToGameObject;
    private string selectedEventName;
    private List<string> addedEventNames = new List<string>();

    private RadioButtonGroup addedBlocklogicFoldout;
    private string selectedBlockLogicName;
    private List<string> addedBlockLogicNames = new List<string>();

    [MenuItem("Zengo/Module Block Editor Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PopUpWindowModuleBlocks), false, "Module Blocks");
    }

    public void CreateGUI()
    {
        visualTreeXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VR Core/ModulLogic/UI/PopUpWindowEditor.uxml");
        radioToggleXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VR Core/ModulLogic/UI/RadioToggle.uxml");

        VisualElement root = visualTreeXML.CloneTree();
        rootVisualElement.Add(root);

        statusLabel = root.Q<Label>("StatusLabel");
        InitializeUIElements(root);

        Selection.selectionChanged += OnSelectionChanged;

        selectedGameObjectField.value = Selection.activeGameObject;

        var addStartButton = root.Q<Button>("AddStartButton");
        addStartButton.clicked += OnAddStartButtonClicked;

        var addEndButton = root.Q<Button>("AddEndButton");
        addEndButton.clicked += OnAddEndButtonClicked;

        addedBlocklogicFoldout = root.Q<RadioButtonGroup>("AddedBlocklogicFoldout");
        if (addedBlocklogicFoldout == null)
        {
            addedBlocklogicFoldout = new RadioButtonGroup();
            addedBlocklogicFoldout.name = "AddedBlocklogicFoldout";
            root.Add(addedBlocklogicFoldout);
        }
    }

    private void InitializeUIElements(VisualElement root)
    {
        var componentDropdown = root.Q<DropdownField>("ComponentDropdown");
        var endComponentDropdown = root.Q<DropdownField>("EndComponentDropdown");

        InitializeComponentDropdown(componentDropdown);
        InitializeEndComponentDropdown(endComponentDropdown);

        selectedGameObjectField = root.Q<ObjectField>("SelectedGameObjectField");
        selectedGameObjectField.objectType = typeof(GameObject);
        selectedGameObjectField.allowSceneObjects = true;

        selectedEndGameObjectField = root.Q<ObjectField>("SelectedEndGameObjectField");
        selectedEndGameObjectField.objectType = typeof(GameObject);
        selectedEndGameObjectField.allowSceneObjects = true;

        lockButton = root.Q<Button>("LockButton");
        if (lockButton == null)
        {
            lockButton = new Button(ToggleLock);
            root.Add(lockButton);
        }
        else
        {
            lockButton.clicked += ToggleLock;
        }
        UpdateLockButtonState();
    }

    private void ToggleLock()
    {
        isLocked = !isLocked;
        selectedGameObjectField.SetEnabled(!isLocked);
        UpdateLockButtonState();
    }

    private void UpdateLockButtonState()
    {
        if (isLocked)
        {
            lockButton.RemoveFromClassList("lockButtonActive");
            lockButton.AddToClassList("lockButton");
        }
        else
        {
            lockButton.RemoveFromClassList("lockButton");
            lockButton.AddToClassList("lockButtonActive");
        }
    }

    private void OnSelectionChanged()
    {
        if (!isLocked && selectedGameObjectField.enabledSelf)
        {
            selectedGameObjectField.value = Selection.activeGameObject;
            UpdateAddedEventsFoldoutFromGameObject(Selection.activeGameObject);
        }
        if (selectedEndGameObjectField.enabledSelf)
        {
            selectedEndGameObjectField.value = Selection.activeGameObject;
            UpdateAddedBlocklogicFoldoutFromGameObject(Selection.activeGameObject);
        }
    }

    private void OnAddStartButtonClicked()
    {
        GameObject selectedGameObject = (GameObject)selectedGameObjectField.value;
        var componentDropdown = rootVisualElement.Q<DropdownField>("ComponentDropdown");
        if (selectedGameObject != null && !string.IsNullOrEmpty(componentDropdown.value))
        {
            AddComponentToSelectedGameObjectByName(componentDropdown.value, selectedGameObject);
            UpdateAddedEventsFoldoutFromGameObject(selectedGameObject);
        }
        else
        {
            statusLabel.text = "Status: Please select a GameObject and a component type.";
            EditorUtility.DisplayDialog("Selection Missing", "Please select a GameObject and a component type.", "OK");
        }
    }

    private void OnAddEndButtonClicked()
    {
        GameObject selectedEndGameObject = (GameObject)selectedEndGameObjectField.value;
        var endComponentDropdown = rootVisualElement.Q<DropdownField>("EndComponentDropdown");
        if (selectedEndGameObject != null && !string.IsNullOrEmpty(endComponentDropdown.value))
        {
            string selectedComponentName = endComponentDropdown.value;
            Type componentType = GetBlockLogicTypesWithLogicMethod().FirstOrDefault(t => t.Name == selectedComponentName);

            if (componentType != null)
            {
                Component existingComponent = selectedEndGameObject.GetComponent(componentType);
                if (existingComponent == null)
                {
                    Component addedComponent = selectedEndGameObject.AddComponent(componentType);
                    statusLabel.text = $"Status: {selectedComponentName} added to {selectedEndGameObject.name}";
                    Debug.Log($"Component '{selectedComponentName}' added to '{selectedEndGameObject.name}'.");

                    if (addedComponent is BlockLogicBase logicSandbox)
                    {
                        BlockEventDataForListener newListener = new BlockEventDataForListener();
                        GameObject selectedStartGameObject = (GameObject)selectedGameObjectField.value;
                        if (selectedStartGameObject != null)
                        {
                            newListener.gameObject = selectedStartGameObject;

                            string selectedEvent = GetSelectedEventName();
                            if (!string.IsNullOrEmpty(selectedEvent))
                            {
                                BlockLogicBase startLogic = selectedStartGameObject.GetComponent<BlockLogicBase>();
                                if (startLogic != null)
                                {
                                    FieldInfo eventField = startLogic.GetType().GetField(selectedEvent, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (eventField != null && eventField.FieldType == typeof(BlockEvent))
                                    {
                                        newListener.block = startLogic;
                                        newListener.actions.Add(selectedEvent);
                                    }
                                }
                            }
                        }

                        logicSandbox.ActivateOn.Add(newListener);
                        EditorUtility.SetDirty(logicSandbox);
                        Undo.RecordObject(logicSandbox, "Changed");
                        PrefabUtility.RecordPrefabInstancePropertyModifications(logicSandbox);
                    }
                    UpdateAddedBlocklogicFoldoutFromGameObject(selectedEndGameObject);
                }
                else
                {
                    statusLabel.text = $"Status: {selectedComponentName} already exists on {selectedEndGameObject.name}";
                }
            }
            else
            {
                statusLabel.text = "Status: Component type not found.";
            }
        }
        else
        {
            statusLabel.text = "Status: Please select a GameObject and a component type.";
            EditorUtility.DisplayDialog("Selection Missing", "Please select a GameObject and a component type.", "OK");
        }
    }

    private List<Type> GetBlockLogicTypesWithLogicMethod()
    {
        var blockLogicTypes = new List<Type>();

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(BlockLogicBase).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        MethodInfo logicMethod = type.GetMethod("Logic", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (logicMethod != null)
                        {
                            blockLogicTypes.Add(type);
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                continue;
            }
        }

        return blockLogicTypes;
    }

    private void AddComponentToSelectedGameObjectByName(string componentName, GameObject selectedGameObject)
    {
        System.Type componentType = GetComponentTypeByName(componentName);
        if (componentType != null && selectedGameObject != null)
        {
            Component existingComponent = selectedGameObject.GetComponent(componentType);
            Component addedComponent = selectedGameObject.AddComponent(componentType);
            statusLabel.text = $"Status: {componentName} added to {selectedGameObject.name}";
            Debug.Log($"Component '{componentName}' added to '{selectedGameObject.name}'.");
            UpdateAddedEventsFoldout(componentType);
            OnComponentAddedToGameObject?.Invoke(selectedGameObject);
        }
        else
        {
            statusLabel.text = "Status: Invalid component type or no GameObject selected.";
            EditorUtility.DisplayDialog("Error", "Invalid component type or no GameObject selected.", "OK");
        }
    }

    private List<string> GetBlockEventNames(Type type)
    {
        List<string> eventNames = new List<string>();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(BlockEvent))
            {
                eventNames.Add(field.Name);
            }
        }

        return eventNames;
    }

    private void UpdateAddedBlocklogicFoldoutFromGameObject(GameObject gameObject)
    {
        addedBlockLogicNames.Clear();

        if (gameObject == null)
        {
            return;
        }

        BlockLogicBase[] components = gameObject.GetComponents<BlockLogicBase>();

        foreach (BlockLogicBase component in components)
        {
            string componentName = component.GetType().Name;
            if (!addedBlockLogicNames.Contains(componentName))
            {
                addedBlockLogicNames.Add(componentName);
            }
        }

        UpdateAddedBlocklogicFoldoutUI();
    }

    private void UpdateAddedBlocklogicFoldoutUI()
    {
        addedBlocklogicFoldout.Clear();

        foreach (var componentName in addedBlockLogicNames)
        {
            VisualElement radioToggleInstance = radioToggleXML.CloneTree();
            RadioButton radioButton = radioToggleInstance.Q<RadioButton>("RadioButton");
            if (radioButton != null)
            {
                radioButton.label = componentName;
                radioButton.RegisterValueChangedCallback(evt =>
                {
                    if (evt.newValue)
                    {
                        selectedBlockLogicName = componentName;
                    }
                });
                addedBlocklogicFoldout.Add(radioButton);
            }
            else
            {
                Debug.LogError($"RadioButton not found in RadioToggle.uxml for component: {componentName}");
            }
        }

        if (addedBlockLogicNames.Count == 0)
        {
            addedBlocklogicFoldout.Add(new Label("No BlockLogic components found."));
        }
    }

    private System.Type GetComponentTypeByName(string componentName)
    {
        Type type = GetComponentTypeFromSwitch(componentName);

        if (type == null)
        {
            type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == componentName && typeof(Component).IsAssignableFrom(t));
        }

        return type;
    }

    private System.Type GetComponentTypeFromSwitch(string componentName)
    {
        switch (componentName)
        {
            case "Disable Child Block": return typeof(DisableChildBlock);
            case "Enable Child Block": return typeof(EnableChildBlock);
            case "On Grab": return typeof(OnGrab);
            case "Animate Blocks": return typeof(AnimateBlock);
            case "Block Action To Unity Action": return typeof(BlockActionToUnityAction);
            case "Conditional Trigger": return typeof(ConditionalTrigger);
            case "State Holder": return typeof(StateHolder);
            case "Teleport Block": return typeof(TeleportBlock);
            case "Trigger Block": return typeof(TriggerBlock);
            case "Zone Block": return typeof(ZoneBlock);
            case "Unity To Trigger Block": return typeof(UnityToTriggerBlock);
            case "ModulLogicOnclickSample": return typeof(ModulLogicOnclickSample);
            default: return null;
        }
    }

    private void UpdateAddedEventsFoldout(Type componentType)
    {
        List<string> newEventNames = GetBlockEventNames(componentType);

        foreach (var eventName in newEventNames)
        {
            if (!addedEventNames.Contains(eventName))
            {
                addedEventNames.Add(eventName);
            }
        }

        UpdateAddedEventsFoldoutUI();
    }

    private void UpdateAddedEventsFoldoutFromGameObject(GameObject gameObject)
    {
        addedEventNames.Clear();

        if (gameObject == null)
        {
            return;
        }

        Component[] components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
            List<string> componentEventNames = GetBlockEventNames(component.GetType());
            foreach (var eventName in componentEventNames)
            {
                if (!addedEventNames.Contains(eventName))
                {
                    addedEventNames.Add(eventName);
                }
            }
        }

        UpdateAddedEventsFoldoutUI();
    }

    private void UpdateAddedEventsFoldoutUI()
    {
        RadioButtonGroup addedEventsFoldout = rootVisualElement.Q<RadioButtonGroup>("AddedEventsFoldout");
        addedEventsFoldout.Clear();

        foreach (var eventName in addedEventNames)
        {
            VisualElement radioToggleInstance = radioToggleXML.CloneTree();
            RadioButton radioButton = radioToggleInstance.Q<RadioButton>("RadioButton");
            if (radioButton != null)
            {
                radioButton.label = eventName;
                radioButton.RegisterValueChangedCallback(evt =>
                {
                    if (evt.newValue)
                    {
                        selectedEventName = eventName;
                        UpdateSelectedEventUI(eventName);
                    }
                });
                addedEventsFoldout.Add(radioButton);
            }
            else
            {
                Debug.LogError($"RadioButton not found in RadioToggle.uxml for event: {eventName}");
            }
        }
    }

    private void UpdateSelectedEventUI(string eventName)
    {
        Debug.Log($"Selected event: {eventName}");
        // Update other UI elements as needed
    }

    private string GetSelectedEventName()
    {
        return selectedEventName;
    }

    private void InitializeComponentDropdown(DropdownField dropdown)
    {
        var componentNames = new List<string>
        {
            "Disable Child Block",
            "Enable Child Block",
            "On Grab",
            "Animate Blocks",
            "Block Action To Unity Action",
            "Conditional Trigger",
            "State Holder",
            "Teleport Block",
            "Trigger Block",
            "Zone Block",
            "Unity To Trigger Block",
            "ModulLogicOnclickSample"
        };

        dropdown.choices.Clear();
        dropdown.choices.AddRange(componentNames);

        if (dropdown.choices.Count > 0)
        {
            dropdown.value = dropdown.choices[0];
        }
    }

    private void InitializeEndComponentDropdown(DropdownField dropdown)
    {
        var blockLogicTypes = GetBlockLogicTypesWithLogicMethod();

        dropdown.choices.Clear();
        dropdown.choices.AddRange(blockLogicTypes.Select(t => t.Name));

        if (dropdown.choices.Count > 0)
        {
            dropdown.value = dropdown.choices[0];
        }
    }
}