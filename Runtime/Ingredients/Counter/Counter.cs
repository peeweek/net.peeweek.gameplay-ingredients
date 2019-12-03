using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace GameplayIngredients
{
    public class Counter : MonoBehaviour
    {
        public int InitialValue = 0;

        public int CurrentValue { get; private set; }

        [ReorderableList, NonNullCheck]
        public Callable[] OnValueChanged;

        void Reset()
        {
            CurrentValue = InitialValue;
        }

        public void SetValue(int newValue)
        {
            if(newValue != CurrentValue)
            {
                CurrentValue = newValue;
                Callable.Call(OnValueChanged);
            }
        }

    }
}

