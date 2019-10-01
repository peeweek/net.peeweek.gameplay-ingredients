using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using GameplayIngredients.Events;
using GameplayIngredients.Logic;
using GameplayIngredients.Actions;
using GameplayIngredients.StateMachines;
using UnityEngine.SceneManagement;

namespace GameplayIngredients.Editor
{
    public class CallTreeWindow : EditorWindow
    {
        CallTreeView m_TreeView;
        [MenuItem("Window/Gameplay Ingredients/Callable Tree Explorer", priority = MenuItems.kWindowMenuPriority)]
        static void OpenWindow()
        {
            GetWindow<CallTreeWindow>();
        }

        private void OnEnable()
        {
            nodeRoots = new Dictionary<string, List<CallTreeNode>>();
            m_TreeView = new CallTreeView(nodeRoots);            
            titleContent = new GUIContent("Callable Tree Explorer");
            ReloadCallHierarchy();
            EditorSceneManager.sceneOpened += Reload;
        }

        void Reload(Scene scene, OpenSceneMode mode)
        {
            ReloadCallHierarchy();
        }

        private void OnGUI()
        {
            int tbHeight = 24;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Height(tbHeight)))
            {
                if (GUILayout.Button("Reload", EditorStyles.toolbarButton))
                {
                    ReloadCallHierarchy();
                }
                GUILayout.FlexibleSpace();
                Rect buttonRect = GUILayoutUtility.GetRect(64, 16);
                if (GUI.Button(buttonRect, "Filter", EditorStyles.toolbarDropDown))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Filter Selected"), false, () => {
                        m_TreeView.SetAutoFilter(false);
                        m_TreeView.SetFilter(Selection.activeGameObject);
                    });
                    menu.AddItem(new GUIContent("Clear Filter"), false, () => {
                        m_TreeView.SetAutoFilter(false);
                        m_TreeView.SetFilter(null);
                    });
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Automatic Filter"), m_TreeView.AutoFilter, () => {
                        m_TreeView.ToggleAutoFilter();
                    });
                    menu.DropDown(buttonRect);
                }

            }
            Rect r = GUILayoutUtility.GetRect(position.width, position.height - tbHeight);
            m_TreeView.OnGUI(r);
        }

        Dictionary<string, List<CallTreeNode>> nodeRoots;

        void ReloadCallHierarchy()
        {
            if (nodeRoots == null)
                nodeRoots = new Dictionary<string, List<CallTreeNode>>();
            else
                nodeRoots.Clear();

            AddToCategory<EventBase>("Events");
            AddToCategory<StateMachine>("State Machines");
            AddToCategory<Factory>("Factories");
            AddToCategory<SendMessageAction>("Messages");

            m_TreeView.Reload();
        }

        void AddToCategory<T>(string name) where T:MonoBehaviour
        {
            var list = Resources.FindObjectsOfTypeAll<T>().ToList();

            if (list.Count > 0)
                nodeRoots.Add(name, new List<CallTreeNode>());

            var listRoot = nodeRoots[name];
            foreach (var item in list)
            {
                if(typeof(T) == typeof(StateMachine))
                {
                    listRoot.Add(GetStateMachineNode(item as StateMachine));
                }
                else if(typeof(T) == typeof(SendMessageAction))
                {
                    listRoot.Add(GetMessageNode(item as SendMessageAction));
                }
                else
                {
                    listRoot.Add(GetNode(item));
                }
            }

        }

        CallTreeNode GetNode(MonoBehaviour bhv)
        {
            var rootNode = new CallTreeNode(bhv, GetType(bhv), $"{bhv.gameObject.name} ({bhv.GetType().Name})");
            var type = bhv.GetType();
            foreach (var field in type.GetFields())
            {
                // Find Fields that are Callable[]
                if (field.FieldType.IsAssignableFrom(typeof(Callable[])))
                {
                    var node = new CallTreeNode(bhv, CallTreeNodeType.Callable, field.Name);
                    var value = (Callable[])field.GetValue(bhv);

                    if (value != null && value.Length > 0)
                    {
                        rootNode.Children.Add(node);
                        // Add Callables from this Callable[] array
                        foreach (var call in value)
                        {
                            node.Children.Add(GetNode(call));
                        }
                    }
                }
            }
            return rootNode;
        }

        CallTreeNode GetMessageNode(SendMessageAction sm)
        {
            var rootNode = new CallTreeNode(sm, CallTreeNodeType.Message, $"{sm.MessageToSend} : ({sm.gameObject.name}.{sm.Name})");
            var all = Resources.FindObjectsOfTypeAll<OnMessageEvent>().Where(o=> o.MessageName == sm.MessageToSend).ToList();

            foreach(var evt in all)
            {
                rootNode.Children.Add(GetNode(evt));
            }
            return rootNode;
        }


        CallTreeNode GetStateMachineNode(StateMachine sm)
        {
            var rootNode = new CallTreeNode(sm, CallTreeNodeType.StateMachine, sm.gameObject.name);
            var type = sm.GetType();
            foreach (var field in type.GetFields())
            {
                // Find Fields that are State[]
                if (field.FieldType.IsAssignableFrom(typeof(State[])))
                {
                    // Add Callables from this Callable[] array
                    var value = (State[])field.GetValue(sm);
                    foreach (var state in value)
                    {
                        rootNode.Children.Add(GetStateNode(state));
                    }
                }
            }
            return rootNode;
        }

        CallTreeNode GetStateNode(State st)
        {
            var rootNode = new CallTreeNode(st, CallTreeNodeType.State, st.gameObject.name);
            var type = st.GetType();
            foreach (var field in type.GetFields())
            {
                // Find Fields that are Callable[]
                if (field.FieldType.IsAssignableFrom(typeof(Callable[])))
                {
                    var node = new CallTreeNode(st, CallTreeNodeType.Callable, field.Name);
                    rootNode.Children.Add(node);
                    // Add Callables from this Callable[] array
                    var value = (Callable[])field.GetValue(st);
                    foreach (var call in value)
                    {
                        node.Children.Add(GetNode(call));
                    }
                }
            }
            return rootNode;
        }

        CallTreeNodeType GetType(MonoBehaviour bhv)
        {
            if (bhv == null)
                return CallTreeNodeType.Callable;
            else if (bhv is EventBase)
                return CallTreeNodeType.Event;
            else if (bhv is LogicBase)
                return CallTreeNodeType.Logic;
            else if (bhv is ActionBase)
                return CallTreeNodeType.Action;
            else if (bhv is StateMachine)
                return CallTreeNodeType.StateMachine;
            else if (bhv is State)
                return CallTreeNodeType.State;
            else if (bhv is OnMessageEvent || bhv is SendMessageAction)
                return CallTreeNodeType.Message;
            else
                return CallTreeNodeType.Callable;
        }

        class CallTreeNode
        {
            public string Name;
            public MonoBehaviour Target;
            public List<CallTreeNode> Children;
            public CallTreeNodeType Type;
            public CallTreeNode(MonoBehaviour target, CallTreeNodeType type, string name = "")
            {
                Name = string.IsNullOrEmpty(name) ? target.GetType().Name : name;
                Target = target;
                Type = type;
                Children = new List<CallTreeNode>();
            }

            public bool ContainsReference(GameObject go)
            {
                if (go == null)
                    return true;
                else
                {
                    if (this.Target.gameObject == go)
                        return true;

                    bool value = false;
                    foreach (var node in Children)
                        value = value || node.ContainsReference(go);
                    return value;
                }
            }
        }

        public enum CallTreeNodeType
        {
            Callable,
            Event,
            Logic,
            Action,
            Message,
            StateMachine,
            State
        }

        class CallTreeView : TreeView
        {
            Dictionary<string, List<CallTreeNode>> m_Roots;
            Dictionary<int, CallTreeNode> m_Bindings;

            public CallTreeView(Dictionary<string, List<CallTreeNode>> roots) : base(new TreeViewState())
            {
                m_Roots = roots;
                m_Bindings = new Dictionary<int, CallTreeNode>();
            }

            GameObject m_filter = null;
            public bool AutoFilter { get; private set; }
            public void ToggleAutoFilter()
            {
                SetAutoFilter(!AutoFilter);
            }

            public void SetAutoFilter(bool value)
            {
                AutoFilter = value;
                if (AutoFilter)
                {
                    Selection.selectionChanged += UpdateAutoFilter;
                    if(this.HasSelection())
                    {
                        SetFilter(m_Bindings[this.GetSelection()[0]].Target.gameObject);
                    }
                }
                else
                    Selection.selectionChanged -= UpdateAutoFilter;
            }

            void UpdateAutoFilter()
            {
                if (Selection.activeGameObject != null)
                    SetFilter(Selection.activeGameObject);
            }

            public void SetFilter(GameObject filter = null)
            {
                m_filter = filter;
                this.Reload();
            }

            protected override TreeViewItem BuildRoot()
            {
                int id = -1;
                m_Bindings.Clear();
                var treeRoot = new TreeViewItem(++id, -1, "~Root");

                foreach(var kvp in m_Roots)
                {
                    if (kvp.Value == null || kvp.Value.Count == 0)
                        continue;

                    var currentRoot = new TreeViewItem(++id, 0, kvp.Key);
                    treeRoot.AddChild(currentRoot);
                    foreach (var node in kvp.Value)
                    {
                        if (node.ContainsReference(m_filter))
                        {
                            currentRoot.AddChild(GetNode(node, ref id, 1));
                        }
                    }
                }
                if (treeRoot.children == null)
                {
                    treeRoot.AddChild(new TreeViewItem(1, 0, "(No Results)"));
                }

                return treeRoot;
            }

            TreeViewItem GetNode(CallTreeNode node, ref int id, int depth)
            {
                id++;
                var item = new TreeViewItem(id, depth, $"{node.Name}");
                item.icon = GetIcon(node.Target, node.Type);
                m_Bindings.Add(id, node);

                foreach(var child in node.Children)
                {
                    item.AddChild(GetNode(child, ref id, depth + 1));
                }
                return item;
            }

            Texture2D GetIcon(MonoBehaviour bhv, CallTreeNodeType type)
            {
                if(bhv != null && type != CallTreeNodeType.Callable)
                {
                    var texture = EditorGUIUtility.ObjectContent(bhv, bhv.GetType()).image;
                    if (texture != null)
                        return texture as Texture2D;
                }

                switch(type)
                {
                    default:
                    case CallTreeNodeType.Callable:
                        return Styles.Callable;
                    case CallTreeNodeType.Action:
                        return Styles.Action;
                    case CallTreeNodeType.Logic:
                        return Styles.Logic;
                    case CallTreeNodeType.Event:
                        return Styles.Event;
                    case CallTreeNodeType.Message:
                        return Styles.Message;
                    case CallTreeNodeType.State:
                        return Styles.State;
                    case CallTreeNodeType.StateMachine:
                        return Styles.StateMachine;
                }
            }

            protected override void SelectionChanged(IList<int> selectedIds)
            {
                if (AutoFilter)
                    return;

                base.SelectionChanged(selectedIds);
                if (selectedIds.Count > 0 && m_Bindings.ContainsKey(selectedIds[0]))
                    Selection.activeObject = m_Bindings[selectedIds[0]].Target;
            }

            static class Styles
            {
                public static Texture2D Callable = Icon("Misc/ic-callable.png");
                public static Texture2D Action = Icon("Actions/ic-action-generic.png");
                public static Texture2D Logic = Icon("Logic/ic-generic-logic.png");
                public static Texture2D Event = Icon("Events/ic-event-generic.png");
                public static Texture2D Message = Icon("Events/ic-event-message .png");
                public static Texture2D StateMachine = Icon("Misc/ic-StateMachine.png");
                public static Texture2D State = Icon("Misc/ic-State.png");

                static Texture2D Icon(string path)
                {
                    return AssetDatabase.LoadAssetAtPath<Texture2D>($"Packages/net.peeweek.gameplay-ingredients/Icons/{path}");
                }
            }
        }
    }
}

