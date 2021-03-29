using GameplayIngredients.Actions;
using GameplayIngredients.Logic;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace GameplayIngredients.Editor
{
    public class CallableProvider : IBrowsePopupProvider
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            var provider = new CallableProvider();
            instance = provider;
        }

        public static CallableProvider instance { get; private set; }

        public static GameObject targetGameObject;
        public static SerializedProperty targetSerializedProperty;

        static Dictionary<string, Type> s_CallableTypes; 

        public CallableProvider()
        {
            var types = TypeUtility.GetConcreteTypes<Callable>();
            s_CallableTypes = new Dictionary<string, Type>();
            foreach(var type in types)
            {
                string path = string.Empty;
                if(typeof(LogicBase).IsAssignableFrom(type))
                {
                    path += "Logic/";
                }
                else if(typeof(ActionBase).IsAssignableFrom(type))
                {
                    path += "Action/";
                }
                path += ObjectNames.NicifyVariableName(type.Name);
                s_CallableTypes[path] = type;
            }
        }

        public void CreateComponentTree(List<BrowsePopup.Element> tree)
        {
            tree.Add(new BrowsePopup.Group(0, "Callables"));
            HashSet<string> categories = new HashSet<string>();

            foreach (var kvp in s_CallableTypes)
            {
                int i = 0;

                string cat = string.Join("/", kvp.Key.Split('/').Reverse().Skip(1).Reverse());
                string name = kvp.Key.Split('/').Last();


                if (!categories.Contains(cat) && cat != "")
                {
                    string[] split = cat.Split('/');
                    string current = "";

                    while (i < split.Length)
                    {
                        current += split[i];
                        if (!categories.Contains(current))
                            tree.Add(new BrowsePopup.Group(i + 1, split[i]));
                        i++;
                        current += "/";
                    }
                    categories.Add(cat);
                }
                else
                {
                    i = cat.Split('/').Length;
                }

                if (cat != "")
                    i++;

                tree.Add(new CallableElement(i, kvp.Value, () => {
                    var cmp = targetGameObject?.AddComponent(kvp.Value);
                    targetSerializedProperty.serializedObject.Update();       

                    targetSerializedProperty.InsertArrayElementAtIndex(targetSerializedProperty.arraySize - 1);
                    targetSerializedProperty.GetArrayElementAtIndex(targetSerializedProperty.arraySize - 1).objectReferenceValue = cmp;

                    targetSerializedProperty.serializedObject.ApplyModifiedProperties();
                    }
                ));
            }

        }

        class CallableElement : BrowsePopup.Element
        {
            public Type type;
            public Action spawnCallback;

            public CallableElement(int level, Type type, Action spawnCallback = null)
            {
                this.level = level;
                this.content = new GUIContent(ObjectNames.NicifyVariableName(type.Name));
                this.type = type;
                this.spawnCallback = spawnCallback;
            }
        }

        public bool GoToChild(BrowsePopup.Element element, bool addIfComponent)
        {
            if (element is CallableElement)
            {
                var ce = element as CallableElement;
                ce.spawnCallback?.Invoke();

                return true;
            }
            else
                return false;
        }
    }


}

