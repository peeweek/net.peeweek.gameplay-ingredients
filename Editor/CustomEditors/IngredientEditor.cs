using GameplayIngredients.Editor;
using NaughtyAttributes.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class IngredientEditor : PingableEditor
{

    protected List<SerializedProperty> baseProperties;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (baseProperties == null)
            baseProperties = new List<SerializedProperty>();
        else
            baseProperties.Clear();

        Type inspectedType = this.serializedObject.targetObject.GetType();
        foreach (FieldInfo info in inspectedType.FindMembers(MemberTypes.Field,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, null))
        {
            if (info.IsNotSerialized)
                continue;

            var property = serializedObject.FindProperty(info.Name);

            if (property != null)
                baseProperties.Add(property);
        }
    }

    protected void DrawBaseProperties()
    {
        EditorGUILayout.Space();

        GUILayout.Label("Properties", EditorStyles.boldLabel);
        using (new EditorGUI.IndentLevelScope(1))
        {
            foreach (var prop in baseProperties)
                NaughtyEditorGUI.PropertyField_Layout(prop, true);
        }
    }

    protected void DrawDebugButton(MonoBehaviour target, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(Styles.callableIcon, options))
        {
            IngredientsExplorerWindow.OpenWindow(target);
        }
    }

    protected static class Styles
    {
        public static GUIContent callableIcon;

        static Styles()
        {
            callableIcon = new GUIContent(AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/net.peeweek.gameplay-ingredients/Icons/Misc/ic-callable.png"));
        }
    }
}