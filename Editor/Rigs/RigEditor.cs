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
    public class RigEditor : NaughtyInspector
    {

        SerializedProperty m_UpdateMode;
        SerializedProperty m_RigPriority;

        List<SerializedProperty> rigProperties;
        static Dictionary<Rig, RigEditor> trackedEditors;

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

            if (trackedEditors == null)
                trackedEditors = new Dictionary<Rig, RigEditor>();

            if (!trackedEditors.ContainsKey(serializedObject.targetObject as Rig))
                trackedEditors.Add(serializedObject.targetObject as Rig, this);
        }

        protected override void OnDisable()
        {
            if (trackedEditors.ContainsKey(serializedObject.targetObject as Rig))
                trackedEditors.Remove(serializedObject.targetObject as Rig);

            base.OnDisable();
        }

        float m_pingValue;
        static Rig m_NextToPing;
        public static void PingObject(Rig r)
        {
            m_NextToPing = r;

            // Trigger a repaint if the editor is currently visible
            if (trackedEditors.ContainsKey(r))
                trackedEditors[r].Repaint();
        }

        bool UpdatePing(Rect r)
        {
            if (m_NextToPing == serializedObject.targetObject as Rig)
            {
                m_pingValue = 1;
                m_NextToPing = null;
            }

            if (m_pingValue <= 0)
                return false;

            r.yMin -= 2;
            r.xMin -= 14;
            r.width += 14;
            r.height += 6;
            EditorGUI.DrawRect(r, new Color(15f / 256, 128f / 256, 190f / 256, m_pingValue));

            m_pingValue -= 0.05f;

            return true;

        }


        public override void OnInspectorGUI()
        {

            Rect r = EditorGUILayout.BeginVertical();
            bool needRepaint = UpdatePing(r);

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Update Properties", EditorStyles.boldLabel);

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

                if (GUILayout.Button(EditorGUIUtility.IconContent("UnityEditor.ProfilerWindow"), GUILayout.Width(48), GUILayout.ExpandHeight(true)))
                {
                    // Open Debug Window
                    RigDebugWindow.OpenWindow(this.serializedObject.targetObject as Rig);
                }
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

            EditorGUILayout.EndVertical();

            if (needRepaint)
                Repaint();
        }
    }
}
