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

            GUILayout.Label("Event Properties", EditorStyles.boldLabel);

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
                {
                    using (new EditorGUI.IndentLevelScope(1))
                    {
                        EditorGUILayout.LabelField(this.serializedObject.targetObject.GetType().Name);
                    }
                }

                GUILayout.Space(8);

                DrawDebugButton(this.serializedObject.targetObject as EventBase, GUILayout.Width(48), GUILayout.ExpandHeight(true));
            }

            DrawBaseProperties();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
