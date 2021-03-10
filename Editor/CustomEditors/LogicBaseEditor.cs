using UnityEngine;
using UnityEditor;
using NaughtyAttributes.Editor;
using GameplayIngredients.Logic;

namespace GameplayIngredients.Editor
{
    [CustomEditor(typeof(LogicBase), true)]
    public class LogicBaseEditor : IngredientEditor
    {
        SerializedProperty m_Name;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Name = serializedObject.FindProperty("Name");
        }

        public override void OnInspectorGUI_PingArea()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Logic Properties", EditorStyles.boldLabel);

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
                {
                    using (new EditorGUI.IndentLevelScope(1))
                    {
                        NaughtyEditorGUI.PropertyField_Layout(m_Name, true);
                    }
                }

                GUILayout.Space(8);

                OpenIngredientsExplorerButton(serializedObject.targetObject as LogicBase, GUILayout.Width(48), GUILayout.ExpandHeight(true));

            }

            DrawBaseProperties();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
