using UnityEngine;
using UnityEditor;
using GameplayIngredients.Events;

namespace GameplayIngredients.Editor
{
    [CustomEditor(typeof(EventBase), true)]
    public class EventBaseEditor : IngredientEditor
    {
        public override void OnInspectorGUI_PingArea()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            string name = this.serializedObject.targetObject.GetType().Name;

            DrawBreadCrumb("Event", Color.yellow, () =>
            {
                GUILayout.Label(ObjectNames.NicifyVariableName(name));
                GUILayout.FlexibleSpace();
                OpenIngredientsExplorerButton(serializedObject.targetObject as EventBase, GUILayout.Width(48), GUILayout.Height(24));
            });

            DrawBaseProperties();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
