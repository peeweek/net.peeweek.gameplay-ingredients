using UnityEngine;
using UnityEditor;
using NaughtyAttributes.Editor;
using UnityEditorInternal;
using System.Linq;

namespace GameplayIngredients.Editor
{
    public class CallableListPropertyDrawer : ReorderableListPropertyDrawer
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            SpecialCaseDrawerAttributeExtensions.Add<CallableListAttribute>(Instance);
        }

        public static new readonly CallableListPropertyDrawer Instance = new CallableListPropertyDrawer();

        protected override ReorderableList GetReorderableList(SerializedProperty property)
        {
            var list = base.GetReorderableList(property);
            if (list.onAddDropdownCallback == null)
                list.onAddDropdownCallback = AddDropdown;

            return list;
        }

        public void AddDropdown(Rect buttonRect, ReorderableList list)
        {
            CallableProvider.targetSerializedProperty = list.serializedProperty;
            CallableProvider.targetGameObject = (list.serializedProperty.serializedObject.targetObject as Component).gameObject;
            BrowsePopup.Show(buttonRect.position, CallableProvider.instance);
        }

    }
}


