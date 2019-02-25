using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Logic
{
    public class SaveDataLogic : LogicBase
    {
        public enum Evaluation
        {
            Equal,
            NotEqual,
            Greater,
            GreaterOrEqual,
            Less,
            LessOrEqual
        }

        public Evaluation Test = Evaluation.Equal;
        public GameSaveManager.Location SaveLocation = GameSaveManager.Location.System;
        public GameSaveManager.ValueType ValueType = GameSaveManager.ValueType.String;
        public string Key = "SomeKey";

        public bool BoolTargetValue;
        public int IntTargetValue;
        public float FloatTargetValue;
        public string StringTargetValue;

        [ReorderableList]
        public Callable[] OnTestSuccess;
        [ReorderableList]
        public Callable[] OnTesFail;

        public override void Execute(GameObject instigator = null)
        {
            var gsm = Manager.Get<GameSaveManager>();
            bool result = false;

            switch(ValueType)
            {
                case GameSaveManager.ValueType.Bool:
                    if (!gsm.HasBool(Key, SaveLocation))
                    {
                        WarnNotExist(Key, ValueType, SaveLocation); 
                    }
                    else
                    {
                        result = TestValue(gsm.GetBool(Key, SaveLocation), BoolTargetValue);
                    }
                    break;
                case GameSaveManager.ValueType.Int:
                    if (!gsm.HasInt(Key, SaveLocation))
                    {
                        WarnNotExist(Key, ValueType, SaveLocation); 
                    }
                    else
                    {
                        result = TestValue(gsm.GetInt(Key, SaveLocation), IntTargetValue);
                    }
                    break;
                case GameSaveManager.ValueType.Float:
                    if (!gsm.HasFloat(Key, SaveLocation))
                    {
                        WarnNotExist(Key, ValueType, SaveLocation); 
                    }
                    else
                    {
                        result = TestValue(gsm.GetFloat(Key, SaveLocation), FloatTargetValue);
                    }
                    break;
                case GameSaveManager.ValueType.String:
                    if (!gsm.HasSting(Key, SaveLocation))
                    {
                        WarnNotExist(Key, ValueType, SaveLocation); 
                    }
                    else
                    {
                        result = TestValue(gsm.GetString(Key, SaveLocation), StringTargetValue);
                    }
                    break;
            }

        }

        bool TestValue<T>(T value, T other) where T : System.IComparable<T>
        {
            switch(Test)
            {
                case Evaluation.Equal: return value.CompareTo(other) == 0;
                case Evaluation.NotEqual: return value.CompareTo(other) != 0;
                case Evaluation.Greater: return value.CompareTo(other) > 0;
                case Evaluation.GreaterOrEqual: return value.CompareTo(other) >= 0;
                case Evaluation.Less: return value.CompareTo(other) < 0;
                case Evaluation.LessOrEqual: return value.CompareTo(other) <= 0;
            }
            return false;
        }



        void WarnNotExist(string name, GameSaveManager.ValueType type, GameSaveManager.Location location)
        {
            Debug.LogWarning(string.Format("Save Data Logic: Trying to get {0} value to non existent {1} data in {2} save.", type, name, location));
        }
    }
}
