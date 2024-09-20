
/*using System;
using UnityEngine;
using Unity.VisualScripting;


using System.Collections.Generic;

public enum CustomEvents{
    None,
    TeleporterAimStart,
    UINextButtonPressed,
}

namespace Unity.VisualScripting
{
    [UnitCategory("Events")]
    [UnitOrder(0)]
    public sealed class OnCustomEvent : GameObjectEventUnit<CustomEvents>
    {
        public override Type MessageListenerType => null;
        protected override string hookName => EventHooks.Custom;

        [SerializeAs(nameof(argumentCount))]
        private int _argumentCount;

        [DoNotSerialize]
        [Inspectable, UnitHeaderInspectable("Arguments")]
        public int argumentCount
        {
            get => _argumentCount;
            set => _argumentCount = Mathf.Clamp(value, 0, 10);
        }

        /// <summary>
        /// The name of the event.
        /// </summary>
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput name { get; private set; }
    
        [DoNotSerialize]
        public List<ValueOutput> argumentPorts { get; } = new List<ValueOutput>();

        protected override void Definition()
        {
            base.Definition();

            name = ValueInput<CustomEvents>("name",  CustomEvents.None);

            argumentPorts.Clear();

            for (var i = 0; i < argumentCount; i++)
            {
                argumentPorts.Add(ValueOutput<object>("argument_" + i));
            }
        }

        protected override bool ShouldTrigger(Flow flow, CustomEvents args)
        {
            return MyCompareNames(flow, name, args);
        }

        protected override void AssignArguments(Flow flow, CustomEvents args)
        {
            //flow.SetValue(argumentPorts[0], args);
        }
        public bool MyCompareNames(Flow flow, ValueInput namePort, CustomEvents calledName)
        {
            Ensure.That(nameof(calledName)).IsNotNull(calledName);
            return (calledName == flow.GetValue<CustomEvents>(namePort));
        }
        public static void Trigger(GameObject target, string name, params object[] args)
        {
            EventBus.Trigger(EventHooks.Custom, target, new CustomEventArgs(name, args));
        }
    }
}*/