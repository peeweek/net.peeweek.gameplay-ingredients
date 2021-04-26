#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System;
using System.Reflection;

namespace GameplayIngredients.Editor
{
    [CustomPropertyDrawer(typeof(PlayerInputActionAttribute))]
    public class PlayerInputActionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PlayerInputActionAttribute pia = attribute as PlayerInputActionAttribute;

            var obj = property.serializedObject.targetObject;

            var fi = obj.GetType().GetField(pia.name, BindingFlags.NonPublic
            | BindingFlags.Public
            | BindingFlags.Instance);

            if(fi != null)
            {
                var playerInput = fi.GetValue(obj) as PlayerInput;

                if(playerInput != null)
                {
                    if (playerInput.actions != null)
                    {
                        var field = obj.GetType().GetField(property.name, BindingFlags.NonPublic
        | BindingFlags.Public
        | BindingFlags.Instance);
                        var fieldValue = field.GetValue(property.serializedObject.targetObject);

                        var actionMap = playerInput.actions.FindActionMap(playerInput.defaultActionMap);
                        int selected = -1;
                        string[] names = new string[actionMap.actions.Count];
                        for(int i = 0; i < actionMap.actions.Count; i ++)
                        {
                            if (fieldValue == actionMap.actions[i])
                                selected = i;

                            names[i] = actionMap.actions[i].name;
                        }
                        selected = EditorGUI.Popup(position, ObjectNames.NicifyVariableName(property.name), selected, names);
                        if(GUI.changed)
                        {
                            Undo.RecordObject(property.serializedObject.targetObject, "Set Action Map");
                            property.serializedObject.Update();
                            field.SetValue(property.serializedObject.targetObject, actionMap.actions[selected]);
                            EditorUtility.SetDirty(property.serializedObject.targetObject);
                            property.serializedObject.ApplyModifiedProperties();


                        }
                    }
                    else
                        EditorGUI.HelpBox(position, $"Target Player Input had no actions configured", MessageType.Warning);



                }
                else
                    EditorGUI.HelpBox(position, $"Property {pia.name} is null or not of PlayerInput type", MessageType.Error);

            }
            else
                EditorGUI.HelpBox(position, $"Could not find field {pia.name}", MessageType.Error);
        }
    }
}
#endif
