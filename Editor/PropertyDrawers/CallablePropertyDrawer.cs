using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
  
    [CustomPropertyDrawer(typeof(Callable))]
    public class CallablePropertyDrawer : PropertyDrawer
    {
        private Callable setNextObjectValue = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(setNextObjectValue != null)
            {
                property.objectReferenceValue = setNextObjectValue;
                setNextObjectValue = null;
            }

            var pickRect = new Rect(position);
            pickRect.xMin = pickRect.xMax - 184;
            pickRect.xMax -= 30;

            var gotoRect = new Rect(position);
            gotoRect.xMin = gotoRect.xMax - 24;

            var objRect = new Rect(position);
            objRect.xMax -= 188;

            var obj = EditorGUI.ObjectField(objRect, property.objectReferenceValue, typeof(Callable), true);

            if (GUI.changed)
                property.objectReferenceValue = obj;

            if (GUI.Button(gotoRect, ">"))
            {
                Selection.activeObject = property.objectReferenceValue;
            }
            if (GUI.Button(pickRect, (property.objectReferenceValue as Callable).Name, EditorStyles.popup))
            {
                ShowMenu(property);
            }
        }

        void ShowMenu(SerializedProperty property)
        {
            GenericMenu menu = new GenericMenu();
            var components = (property.objectReferenceValue as Callable).gameObject.GetComponents<Callable>();
            foreach(var component in components)
            {
                menu.AddItem(new GUIContent(component.GetType().Name + " - " + component.Name), component == property.objectReferenceValue, SetMenu, component);
            }

            menu.ShowAsContext();
        }

        void SetMenu(object o)
        {
            Callable component = o as Callable;
            setNextObjectValue = component;

        }
        
    }
}


