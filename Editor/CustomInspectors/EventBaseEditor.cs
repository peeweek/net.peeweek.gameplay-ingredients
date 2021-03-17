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

            DrawBreadCrumb("Event", color, () =>
            {
                GUILayout.Label(ObjectNames.NicifyVariableName(name));
                GUILayout.FlexibleSpace();
                OpenIngredientsExplorerButton(serializedObject.targetObject as EventBase);
            });

            DrawBaseProperties();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        static readonly Color color = new Color(.1f, .5f, 1f, 1f);
    }
}
