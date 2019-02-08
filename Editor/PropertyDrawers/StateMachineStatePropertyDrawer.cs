using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using GameplayIngredients.StateMachines;

namespace GameplayIngredients.Editor
{
    [CustomPropertyDrawer(typeof(StateMachineStateAttribute))]
    public class StateMachineStatePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string propertyName = (attribute as StateMachineStateAttribute).PropertyName;
            StateMachine stateMachine;

            if (propertyName == "")
                stateMachine = (StateMachine)property.serializedObject.targetObject;
            else
                stateMachine = (StateMachine)property.serializedObject.FindProperty(propertyName).objectReferenceValue;

            if (stateMachine == null)
            {
                EditorGUI.HelpBox(position, "StateMachineStateAttribute property parameter references a null or not a GameplayIngredients.StateMachines.StateMachine type", MessageType.Error);
            }
            else
            {
                string currentValue = property.stringValue;
                int count = stateMachine.States.Length;
                int[] indices = new int[count];
                GUIContent[] labels = new GUIContent[count];
                int selected = -1;
                for(int i = 0; i< count; i++)
                {
                    string stateName = stateMachine.States[i].StateName;
                    if (stateName == currentValue)
                        selected = i;
                    indices[i] = i;
                    labels[i] = new GUIContent(stateName);

                }

                int newIdx = EditorGUI.IntPopup(position, new GUIContent(property.displayName), selected, labels, indices);
                if(GUI.changed)
                {
                    property.stringValue = stateMachine.States[newIdx].StateName;
                }
            }
        }

        static class Styles
        {
            public static GUIStyle errorfield;

            static Styles()
            {
                errorfield = new GUIStyle(EditorStyles.objectField);
            }
        }
    }
}
