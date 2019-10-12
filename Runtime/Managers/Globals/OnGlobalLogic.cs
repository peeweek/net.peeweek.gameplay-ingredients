using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Logic
{
    public class OnGlobalLogic : LogicBase
    {
        public Globals.Scope scope = Globals.Scope.Global;
        public Globals.Type type = Globals.Type.Boolean;
        public string Variable = "SomeVariable";
        public Evaluation evaluation = Evaluation.Equal;


        [ShowIf("isBool")]
        public bool boolValue = true;
        [ShowIf("isInt")]
        public int intValue = 1;
        [ShowIf("isString")]
        public string stringValue = "Value";
        [ShowIf("isFloat")]
        public float floatValue = 1.0f;
        [ShowIf("isGameObject")]
        public GameObject gameObjectValue;

        public enum Evaluation
        {
            Equal,
            NotEqual,
            Greater,
            GreaterOrEqual,
            Less,
            LessOrEqual,
            Exists
        }

        bool isBool() { return type == Globals.Type.Boolean; }
        bool isInt() { return type == Globals.Type.Integer; }
        bool isFloat() { return type == Globals.Type.Float; }
        bool isString() { return type == Globals.Type.String; }
        bool isGameObject() { return type == Globals.Type.GameObject; }

        [ReorderableList]
        public Callable[] OnTestSuccess;
        [ReorderableList]
        public Callable[] OnTestFail;

        public override void Execute(GameObject instigator = null)
        {
            bool result = false;

            if (evaluation == Evaluation.Exists)
            {
                switch (type)
                {
                    case Globals.Type.Boolean: result = Globals.HasBool(Variable, scope); break;
                    case Globals.Type.Float: result = Globals.HasFloat(Variable, scope); break;
                    case Globals.Type.Integer: result = Globals.HasInt(Variable, scope); break;
                    case Globals.Type.String: result = Globals.HasString(Variable, scope); break;
                }
            }
            else
            {
                switch (type)
                {
                    case Globals.Type.Boolean:
                        if (!Globals.HasBool(Variable, scope))
                        {
                            WarnNotExist(Variable, type, scope);
                        }
                        else
                        {
                            result = TestValue(Globals.GetBool(Variable, scope), boolValue);
                        }
                        break;
                    case Globals.Type.Integer:
                        if (!Globals.HasInt(Variable, scope))
                        {
                            WarnNotExist(Variable, type, scope);
                        }
                        else
                        {
                            result = TestValue(Globals.GetInt(Variable, scope), intValue);
                        }
                        break;
                    case Globals.Type.Float:
                        if (!Globals.HasFloat(Variable, scope))
                        {
                            WarnNotExist(Variable, type, scope);
                        }
                        else
                        {
                            result = TestValue(Globals.GetFloat(Variable, scope), floatValue);
                        }
                        break;
                    case Globals.Type.String:
                        if (!Globals.HasString(Variable, scope))
                        {
                            WarnNotExist(Variable, type, scope);
                        }
                        else
                        {
                            result = TestValue(Globals.GetString(Variable, scope), stringValue);
                        }
                        break;
                    case Globals.Type.GameObject:
                        if (!Globals.HasObject(Variable, scope))
                        {
                            WarnNotExist(Variable, type, scope);
                        }
                        else
                        {
                            result = TestValue(Globals.GetObject(Variable, scope), gameObjectValue);
                        }
                        break;
                }
            }

            if (result)
                Callable.Call(OnTestSuccess, instigator);
            else
                Callable.Call(OnTestFail, instigator);

        }

        bool TestValue<T>(T value, T other) where T : System.IComparable<T>
        {
            switch (evaluation)
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



        void WarnNotExist(string name,Globals.Type type, Globals.Scope location)
        {
            Debug.LogWarning(string.Format("Save Data Logic: Trying to get {0} value to non existent {1} data in {2} save.", type, name, location));
        }
    }
}
