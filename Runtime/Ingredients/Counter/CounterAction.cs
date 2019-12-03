using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Actions
{
    public class CounterAction : ActionBase
    {
        public enum CounterOperation
        {
            Set,
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo
        }

        public enum ValueSourceType
        {
            Property,
            Global,
            GameSave,
        }

        [ReorderableList, NonNullCheck]
        public Counter[] Counters;

        public CounterOperation Operation = CounterOperation.Set;
        public ValueSourceType ValueSource = ValueSourceType.Property;
        [ShowIf("isValueProperty")]
        public int Value = 1;

        [ShowIf("isValueGameSave")]
        public string GameSaveVariableName = "Variable";
        [ShowIf("isValueGameSave")]
        public GameSaveManager.Location GameSaveLocation = GameSaveManager.Location.System;

        [ShowIf("isValueGlobal")]
        public string GlobalVariableName = "Variable";
        [ShowIf("isValueGlobal")]
        public Globals.Scope GlobalScope = Globals.Scope.Global;

        bool isValueProperty() { return ValueSource == ValueSourceType.Property; }
        bool isValueGameSave() { return ValueSource == ValueSourceType.GameSave; }
        bool isValueGlobal() { return ValueSource == ValueSourceType.Global; }

        public override void Execute(GameObject instigator = null)
        {
            int value;
            switch (ValueSource)
            {
                default:
                case ValueSourceType.Property:
                    value = Value;
                    break;
                case ValueSourceType.Global:
                    if (Globals.HasInt(GlobalVariableName, GlobalScope))
                        Globals.GetInt(GlobalVariableName, GlobalScope);
                    else
                    {
                        Debug.LogWarning($"CounterAction ({name}) : Could not find Global {GlobalVariableName}({GlobalScope})");
                        value = 0;
                    }
                    break;
                case ValueSourceType.GameSave:
                    var gsm = Manager.Get<GameSaveManager>();

                    if (gsm.HasInt(GameSaveVariableName, GameSaveLocation))
                        gsm.GetInt(GameSaveVariableName, GameSaveLocation);
                    else
                    {
                        Debug.LogWarning($"CounterAction ({name}) : Could not find Global {GlobalVariableName}({GlobalScope})");
                        value = 0;
                    }

                    break;
            }

            foreach(var counter in Counters)
            {
                if (counter == null)
                    continue;

                switch (Operation)
                {
                    default:
                    case CounterOperation.Set:
                        counter.SetValue(Value);
                        break;
                    case CounterOperation.Add:
                        counter.SetValue(counter.CurrentValue + Value);
                        break;
                    case CounterOperation.Subtract:
                        counter.SetValue(counter.CurrentValue - Value);
                        break;
                    case CounterOperation.Multiply:
                        counter.SetValue(counter.CurrentValue * Value);
                        break;
                    case CounterOperation.Divide:
                        if (Value != 0)
                            counter.SetValue(counter.CurrentValue / Value);
                        else
                            Debug.LogWarning($"{this.name} : Division by zero");
                        break;
                    case CounterOperation.Modulo:
                        if (Value != 0)
                            counter.SetValue(counter.CurrentValue % Value);
                        else
                            Debug.LogWarning($"{this.name} : Modulo by zero");

                        break;
                }
            }
        }
    }
}
