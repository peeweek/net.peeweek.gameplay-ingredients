using GameplayIngredients.Managers;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Editor
{
    [CustomPropertyDrawer(typeof(RegisteredInputActionsAttribute))]
    public class RegisteredInputActionsPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (GameplayIngredientsSettings.currentSettings.excludedeManagers.Contains(nameof(InputSystemManager)))
                return EditorGUIUtility.singleLineHeight * 2.5f;
            else
            {
                return (property.objectReferenceValue == null ? 2:1) * base.GetPropertyHeight(property, label);
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
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
            if(ism.inputActions.Any(o => o.inputActionAsset == property.objectReferenceValue))
            {
                for(int i = 0; i < ism.inputActions.Length; i++)
                {
                    if (ism.inputActions[i].inputActionAsset != null && ism.inputActions[i].inputActionAsset == property.objectReferenceValue)
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
                    property.objectReferenceValue = null;
                else
                    property.objectReferenceValue = ism.inputActions[value].inputActionAsset;
            }

            if (property.objectReferenceValue == null)
            {
                position.yMin += EditorGUIUtility.singleLineHeight;
                position.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(position, property);
            }
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
            public static GUIContent noPreset = new GUIContent("(No Preset : Specify Action)");
            public static GUIContent nullEntry = new GUIContent("Null Entry defined in InputSystemManager");
        }
    }
}




