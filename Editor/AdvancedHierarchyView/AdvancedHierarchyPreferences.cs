using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameplayIngredients.Editor
{
    static class AdvancedHierarchyPreferences
    {
        [SettingsProvider]
        public static SettingsProvider GetAdvancedHierarchyPreferences()
        {
            var provider = new SettingsProvider("Preferences/Gameplay Ingredients/Advanced Hierarchy View", SettingsScope.User)
            {
                label = "Advanced Hierarchy Options",
                guiHandler = OnGUI
            };
            return provider;
        }

        static Dictionary<Type, bool> s_CachedVisibility;
        static readonly string componentPrefix = "GameplayIngredients.HierarchyHints.";
        static readonly string staticPref = "GameplayIngredients.HierarchyHints.Static";

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            if (s_CachedVisibility == null)
                s_CachedVisibility = new Dictionary<Type, bool>();

            foreach (var type in AdvancedHierarchyView.allTypes)
            {
                if (!s_CachedVisibility.ContainsKey(type))
                    s_CachedVisibility.Add(type, EditorPrefs.GetBool(componentPrefix + type.Name, true));
                else
                    s_CachedVisibility[type] = EditorPrefs.GetBool(componentPrefix + type.Name, true);
            }
        }

        public static bool showStatic { get { return EditorPrefs.GetBool(staticPref, true); } }

        public static bool IsVisible(Type t)
        {
            if (s_CachedVisibility.ContainsKey(t))
                return s_CachedVisibility[t];
            else
                return false;
        }

        static void OnGUI(string search)
        {
            EditorGUIUtility.labelWidth = 260;
            EditorGUI.indentLevel ++;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Preferences", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            var s = EditorGUILayout.Toggle("Show Static", showStatic);
            if(EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(staticPref, s);
                EditorApplication.RepaintHierarchyWindow();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Visible Components", EditorStyles.boldLabel);

            EditorGUI.indentLevel ++;
            foreach (var type in AdvancedHierarchyView.allTypes)
            {
                EditorGUI.BeginChangeCheck();
                var value = EditorGUILayout.Toggle(type.Name, s_CachedVisibility[type]);
                if(EditorGUI.EndChangeCheck())
                {
                    s_CachedVisibility[type] = value;
                    EditorPrefs.SetBool(componentPrefix + type.Name, value);
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
            EditorGUI.indentLevel -= 2;
        }
    }

}