using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using NaughtyAttributes.Editor;

namespace GameplayIngredients.Rigs
{
    [CustomEditor(typeof(Rig), true)]
    public class RigEditor : NaughtyInspector
    {

        SerializedProperty m_UpdateMode;
        SerializedProperty m_RigPriority;

        List<SerializedProperty> rigProperties;

        protected override void OnEnable()
        {
            base.OnEnable();

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

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            GUILayout.Label("Update Properties", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope(1))
            {
                NaughtyEditorGUI.PropertyField_Layout(m_UpdateMode, true);
                NaughtyEditorGUI.PropertyField_Layout(m_RigPriority, true);

            }
            EditorGUILayout.Space();
            GUILayout.Label("Rig Properties", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope(1))
            {
                foreach(var prop in rigProperties)
                    NaughtyEditorGUI.PropertyField_Layout(prop, true);

            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

        }
    }
}
