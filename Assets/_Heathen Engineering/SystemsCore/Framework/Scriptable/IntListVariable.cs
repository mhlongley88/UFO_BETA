using System.Collections.Generic;
using UnityEngine;

namespace HeathenEngineering.Scriptable
{
    [CreateAssetMenu(menuName = "Variables/Int List")]
    public class IntListVariable : DataVariable<List<int>>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public List<int> Value;
        [HideInInspector]
        public List<ChangeEventListener<List<int>>> Listeners = new List<ChangeEventListener<List<int>>>();

        public override List<int> DataValue
        {
            get
            {
                return Value;
            }

            set
            {
                SetValue(value);
            }
        }

        public override object ObjectValue
        {
            get
            {
                return Value;
            }

            set
            {
                if (value.GetType() == typeof(List<int>))
                    SetValue((List<int>)value);
            }
        }

        public override void SetValue(List<int> value)
        {
            if (Value != value)
            {
                Value = value;
                Raise();
            }
        }

        public override void SetValue(DataVariable<List<int>> value)
        {
            if (Value != value.DataValue)
            {
                Value = value.DataValue;
                Raise();
            }
        }

        public override void Raise()
        {
            for (int i = Listeners.Count - 1; i >= 0; i--)
            {
                if (Listeners[i] != null)
                    Listeners[i].OnEventRaised(Value);
            }
        }

        public override void AddListener(ChangeEventListener<List<int>> listener)
        {
            Listeners.Add(listener);
        }

        public override void RemoveListener(ChangeEventListener<List<int>> listener)
        {
            Listeners.Remove(listener);
        }
    }
}
