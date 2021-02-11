using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using NaughtyAttributes.Editor;
using GameplayIngredients.Rigs;

namespace GameplayIngredients.Editor
{
    [CustomEditor(typeof(Rig), true)]
    public class RigEditor : PingableEditor
    {
        SerializedProperty m_UpdateMode;
        SerializedProperty m_RigPriority;

        List<SerializedProperty> rigProperties;

        static GUIContent callableIcon;

        protected override void OnEnable()
        {
            base.OnEnable();

            callableIcon = new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/net.peeweek.gameplay-ingredients/Icons/Misc/ic-callable.png"));

            m_UpdateMode = serializedObject.FindProperty("m_UpdateMode");
            m_RigPriority = serializedObject.FindProperty("m_RigPriority");

            if (rigProperties == null)
                rigProperties = new List<SerializedProperty>();
            else
                rigProperties.Clear();

            Type inspectedType = this.serializedObject.targetObject.GetType();
            foreach(FieldInfo info in inspectedType.FindMembers(MemberTypes.Field,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, null))
            {
                if (info.IsNotSerialized)
                    continue;

                var property = serializedObject.FindProperty(info.Name);

                if (property != null)
                    rigProperties.Add(property);
            }
        }

        public override void OnInspectorGUI_PingArea()
        {

            serializedObject.Update();
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

                if (GUILayout.Button(callableIcon, GUILayout.Width(48), GUILayout.ExpandHeight(true)))
                {
                    // Open Debug Window
                    IngredientsExplorerWindow.OpenWindow(this.serializedObject.targetObject as Rig);
                }
            }


            EditorGUILayout.Space();
            GUILayout.Label("Rig Properties", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope(1))
            {
                foreach (var prop in rigProperties)
                    NaughtyEditorGUI.PropertyField_Layout(prop, true);

            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

        }
    }
}
