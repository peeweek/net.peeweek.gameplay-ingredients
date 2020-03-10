using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    public class CheckResult
    {
        public enum Result
        {
            Notice,
            Warning,
            Failed
        }

        public Check check { get; private set; }
        public Result result { get; private set; }
        public Object mainObject { get { if (objects != null && objects.Length > 0) return objects[0]; else return null; } }
        Object[] objects;
        public GUIContent message;


        public int resolutionActionIndex;
        public string action { get { return check.ResolutionActions[resolutionActionIndex]; } }
        public CheckResult(Check check, Result result, string message, params Object[] objects)
        {
            this.check = check;
            this.result = result;
            this.message = new GUIContent(message, GetIcon(result));
            this.objects = objects;
        }

        static Texture GetIcon(Result r)
        {
            switch (r)
            {
                default:
                case Result.Notice:
                    return EditorGUIUtility.IconContent("console.infoicon.sml").image;
                case Result.Warning:
                    return EditorGUIUtility.IconContent("console.warnicon.sml").image;
                case Result.Failed:
                    return EditorGUIUtility.IconContent("console.erroricon.sml").image;

            }
        }
    }
}
