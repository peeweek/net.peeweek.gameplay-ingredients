using GameplayIngredients.Actions;
using GameplayIngredients.Logic;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Editor
{
    public class CallableProvider : IBrowsePopupProvider
    {
        public string targetPropertyName;

        static Dictionary<string, Type> s_CallableTypes;

        static AddComponentInfo addNextComponentInfo;
        struct AddComponentInfo
        {
            public GameObject gameObject;
            public Component component;
            public string propertyName;
            public Type newComponentType;
        }

        public CallableProvider(GameObject gameObject, Component component, string propertyName)
        {
            addNextComponentInfo.gameObject = gameObject;
            addNextComponentInfo.component = component;
            addNextComponentInfo.propertyName = propertyName;
        }

        [InitializeOnLoadMethod]
        static void BuildMenu()
        {
            var types = TypeUtility.GetConcreteTypes<Callable>().OrderBy(o => o.Name);
            s_CallableTypes = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                string path = string.Empty;
                if (type.IsSubclassOf(typeof(LogicBase)))
                {
                    path += "Logic/";
                }
                else if (type.IsSubclassOf(typeof(ActionBase)))
                {
                    path += "Action/";
                }

                path += ObjectNames.NicifyVariableName(type.Name);
                s_CallableTypes[path] = type;
            }
        }

        public void CreateComponentTree(List<BrowsePopup.Element> tree)
        {
            tree.Add(new BrowsePopup.Group(0, "Add New Callable"));
            HashSet<string> categories = new HashSet<string>();

            foreach (var kvp in s_CallableTypes.OrderBy(o => o.Key))
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

                tree.Add(new CallableElement(i, kvp.Value, () => 
                {
                    addNextComponentInfo.newComponentType = kvp.Value;
                    EditorApplication.delayCall += AddCallable;
                } ));
            }
        }

        static void AddCallable()
        {
            addNextComponentInfo.gameObject.AddCallable(
                addNextComponentInfo.component,
                addNextComponentInfo.propertyName, 
                addNextComponentInfo.newComponentType
                );

            EditorApplication.delayCall -= AddCallable;
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

