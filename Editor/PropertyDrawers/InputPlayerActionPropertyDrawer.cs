#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;
using GameplayIngredients.Events;

namespace GameplayIngredients.Editor
{
    [CustomPropertyDrawer(typeof(PlayerInputAction))]
    public class InputPlayerActionPropertyDrawer: PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 8;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var asset = property.FindPropertyRelative("playerInput");
            var path = property.FindPropertyRelative("path");

            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, asset, new GUIContent("Player Input"));
            EditorGUI.indentLevel++;
            position.y += EditorGUIUtility.singleLineHeight + 2;
            
            var paths = GetPaths(asset.objectReferenceValue as InputActionAsset);
            int selected = paths.IndexOf(path.stringValue);
            selected = EditorGUI.Popup(position, "Action", selected, paths);
            if(GUI.changed)
            {
                if (selected >= 0)
                    path.stringValue = paths[selected];
                else
                    path.stringValue = string.Empty;
            }
            EditorGUI.indentLevel--;
        }

        string[] GetPaths(InputActionAsset asset)
        {
            if (asset == null)
                return new string[0];

            List<string> paths = new List<string>();

            foreach(var map in asset.actionMaps)
            {
                if (map == null) continue;
                foreach(var action in map.actions)
                {
                    if (action == null) continue;

                    paths.Add($"{map.name}{InputAssetAction.pathSeparator}{action.name}");
                }
            }

            return paths.ToArray();
        }
    }
}
#endif
