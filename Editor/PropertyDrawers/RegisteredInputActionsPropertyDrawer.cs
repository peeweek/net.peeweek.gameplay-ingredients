using GameplayIngredients.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameplayIngredients.Editor
{
    [CustomPropertyDrawer(typeof(InputSystemManager.RegisteredInputAction))]
    public class RegisteredInputActionsPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (GameplayIngredientsSettings.currentSettings.excludedeManagers.Contains(nameof(InputSystemManager)))
                return EditorGUIUtility.singleLineHeight * 2.5f;
            else
            {
                return 2 * base.GetPropertyHeight(property, label);
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
#if ENABLE_INPUT_SYSTEM
            if (GameplayIngredientsSettings.currentSettings.excludedeManagers.Contains(typeof(InputSystemManager).Name))
            {
                EditorGUI.HelpBox(position, "Cannot check for Input Actions : \nInputSystemManager is excluded in your Assets/Resources/GameplayIngredientsSettings.asset", MessageType.Error);
                return;
            }
            InputSystemManager ism = GetDefaultInputSystemManager();
            if(ism == null)
            {
                GUI.color = Color.red;
                EditorGUI.LabelField(position,"Error while getting the InputSystemManager prefab", EditorStyles.boldLabel);
                GUI.color = Color.white;
                return;
            }

            position.height = EditorGUIUtility.singleLineHeight;

            int length = 1 + ism.inputActions.Length;
            GUIContent[] displayedOptions = new GUIContent[length];
            int[] optionValues = new int[length];
            for(int i = 0; i<length; i++)
            {
                if (i == 0)
                    displayedOptions[i] = Contents.noPreset;
                else
                {
                    var action = ism.inputActions[i - 1].inputActionAsset;
 
                    displayedOptions[i] = action? new GUIContent(action.name) : Contents.nullEntry ;
                }
                optionValues[i] = i - 1;
            }

            int value = -1;

            if(ism.inputActions.Any(o => o.inputActionAsset == GetInputAsset(property) ))
            {
                for(int i = 0; i < ism.inputActions.Length; i++)
                {
                    if (ism.inputActions[i].inputActionAsset != null && ism.inputActions[i].inputActionAsset == property.FindPropertyRelative("asset").objectReferenceValue)
                    {
                        value = i;
                        break;
                    }
                }
            }

            value = EditorGUI.IntPopup(position, Contents.preset, value, displayedOptions, optionValues);

            if(GUI.changed)
            {
                if (value == -1)
                    property.FindPropertyRelative("asset").objectReferenceValue = null;
                else
                    property.FindPropertyRelative("asset").objectReferenceValue = ism.inputActions[value].inputActionAsset;
            }

            position.yMin += EditorGUIUtility.singleLineHeight;
            position.height = EditorGUIUtility.singleLineHeight;

            if (GetInputAsset(property) != null)
            {
                InputActionAsset ia = property.FindPropertyRelative("asset").objectReferenceValue as InputActionAsset;
                List<string> items = new List<string>();

                foreach(var actionMap in ia.actionMaps)
                {
                    foreach(var action in actionMap.actions)
                    {
                        items.Add($"{actionMap.name}/{action.name}");
                    }
                }

                string val = property.FindPropertyRelative("actionPath").stringValue;
                int selected = -1;
                GUIContent[] names = new GUIContent[items.Count];
                int[] indices = new int[items.Count];
                for(int i = 0; i < items.Count; i++)
                {
                    names[i] = new GUIContent(items[i]);
                    indices[i] = i;

                    if (val == items[i])
                        selected = i;
                }

                selected = EditorGUI.IntPopup(position, new GUIContent("  - Action"), selected, names, indices);

                if(GUI.changed)
                {
                    property.FindPropertyRelative("actionPath").stringValue = items[selected];
                }

            }
#else
            EditorGUI.HelpBox(position, "New Input System package is not installed", MessageType.Warning);
#endif
        }

        static InputAction GetAction(SerializedProperty property)
        {
            var inputAsset = GetInputAsset(property);
            string path = property.FindPropertyRelative("actionPath").stringValue;
            return InputSystemManager.RegisteredInputAction.GetAtPath(inputAsset, path);
        }

        static InputActionAsset GetInputAsset(SerializedProperty property)
        {
            return property.FindPropertyRelative("asset").objectReferenceValue as InputActionAsset;
        }

        InputSystemManager GetDefaultInputSystemManager()
        {
            string prefabName = typeof(InputSystemManager).GetCustomAttribute<ManagerDefaultPrefabAttribute>().prefab;

            GameObject prefab = Resources.Load<GameObject>(prefabName);

            if(prefab != null && prefab.TryGetComponent(out InputSystemManager ism))
            {
                return ism;
            }

            prefab = Resources.Load<GameObject>("Default_" + prefabName);
            if (prefab != null && prefab.TryGetComponent(out InputSystemManager ism2))
            {
                return ism2;
            }

            return null;
        }

        static class Contents
        {
            public static GUIContent preset = new GUIContent("Preset", "Preset as defined in your InputSystemManager");
            public static GUIContent noPreset = new GUIContent("(No Preset)");
            public static GUIContent nullEntry = new GUIContent("Null Entry defined in InputSystemManager");
        }
    }
}




