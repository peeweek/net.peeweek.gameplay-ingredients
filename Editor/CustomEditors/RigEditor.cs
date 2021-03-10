using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes.Editor;
using GameplayIngredients.Rigs;

namespace GameplayIngredients.Editor
{
    [CustomEditor(typeof(Rig), true)]
    public class RigEditor : IngredientEditor
    {
        SerializedProperty m_UpdateMode;
        SerializedProperty m_RigPriority;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_UpdateMode = serializedObject.FindProperty("m_UpdateMode");
            m_RigPriority = serializedObject.FindProperty("m_RigPriority");

        }

        public override void OnInspectorGUI_PingArea()
        {
            serializedObject.Update();

            bool excludedRigManager = GameplayIngredientsSettings.currentSettings.excludedeManagers.Any(s => s == "RigManager");

            if (excludedRigManager)
            {
                EditorGUILayout.HelpBox("This Rig depends on the Rig Manager which is excluded in your Gameplay Ingredients Settings. This rig component will be inactive unless the manager is not excluded.", MessageType.Error, true);
                if (GUILayout.Button("Open Settings"))
                    Selection.activeObject = GameplayIngredientsSettings.currentSettings;
            }

            EditorGUI.BeginDisabledGroup(excludedRigManager);

            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Rig : Update Properties", EditorStyles.boldLabel);

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
                {
                    using (new EditorGUI.IndentLevelScope(1))
                    {
                        NaughtyEditorGUI.PropertyField_Layout(m_UpdateMode, true);
                        NaughtyEditorGUI.PropertyField_Layout(m_RigPriority, true);
                    }
                }

                GUILayout.Space(8);

                DrawDebugButton(this.serializedObject.targetObject as Rig, GUILayout.Width(48), GUILayout.ExpandHeight(true));

            }

            DrawBaseProperties();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndDisabledGroup();

        }
    }
}
