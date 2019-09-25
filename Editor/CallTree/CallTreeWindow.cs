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

namespace GameplayIngredients
{
    public class CallTreeWindow : EditorWindow
    {
        CallTreeView m_TreeView;
        [MenuItem("Window/Callable Tree Explorer")]
        static void OpenWindow()
        {
            GetWindow<CallTreeWindow>();
        }

        private void OnEnable()
        {
            nodeRoots = new List<CallTreeNode>();
            m_TreeView = new CallTreeView(nodeRoots);            
            titleContent = new GUIContent("Callable Tree Explorer");
            BuildCallTree();
            EditorSceneManager.sceneOpened += Reload;
        }

        void Reload(Scene scene, OpenSceneMode mode)
        {
            BuildCallTree();
        }

        private void OnGUI()
        {
            int tbHeight = 24;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Height(tbHeight)))
            {
                if (GUILayout.Button("Update", EditorStyles.toolbarButton))
                {
                    BuildCallTree();
                }
                GUILayout.FlexibleSpace();
            }
            Rect r = GUILayoutUtility.GetRect(position.width, position.height - tbHeight);
            m_TreeView.OnGUI(r);
        }

        List<CallTreeNode> nodeRoots;

        void BuildCallTree()
        {
            if (nodeRoots == null)
                nodeRoots = new List<CallTreeNode>();
            else
                nodeRoots.Clear();

            var allEvents = Resources.FindObjectsOfTypeAll<EventBase>().ToList();
            var allStateMachines = Resources.FindObjectsOfTypeAll<StateMachine>().ToList();
            foreach (var evt in allEvents)
            {
                nodeRoots.Add(GetNode(evt));
            }

            foreach (var sm in allStateMachines)
            {
                nodeRoots.Add(GetStateMachineNode(sm));
            }
            m_TreeView.Reload();
        }

        CallTreeNode GetNode(MonoBehaviour bhv)
        {
            var rootNode = new CallTreeNode(bhv, GetType(bhv), $"{bhv.gameObject.name} : {bhv.GetType().Name}");
            var type = bhv.GetType();
            foreach (var field in type.GetFields())
            {
                // Find Fields that are Callable[]
                if (field.FieldType.IsAssignableFrom(typeof(Callable[])))
                {
                    var node = new CallTreeNode(bhv, CallTreeNodeType.Callable, field.Name);
                    rootNode.Children.Add(node);
                    // Add Callables from this Callable[] array
                    var value = (Callable[])field.GetValue(bhv);
                    foreach (var call in value)
                    {
                        node.Children.Add(GetNode(call));
                    }
                }
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

            if (bhv is EventBase)
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
            List<CallTreeNode> m_Roots;
            Dictionary<int, CallTreeNode> m_Bindings;

            public CallTreeView(List<CallTreeNode> roots) : base(new TreeViewState())
            {
                m_Roots = roots;
                m_Bindings = new Dictionary<int, CallTreeNode>();
            }
            
            protected override TreeViewItem BuildRoot()
            {
                int id = 0;
                m_Bindings.Clear();
                var treeRoot = new TreeViewItem(id++, -1, "~Root");
                foreach(var node in m_Roots)
                {
                    treeRoot.AddChild(GetNode(node, ref id, 0));
                }
                return treeRoot;
            }

            TreeViewItem GetNode(CallTreeNode node, ref int id, int depth)
            {
                var item = new TreeViewItem(id++, depth, $"{node.Name} ({node.Type})");
                item.icon = GetIcon(node.Type);
                m_Bindings.Add(id, node);

                foreach(var child in node.Children)
                {
                    item.AddChild(GetNode(child, ref id, depth + 1));
                }
                return item;
            }

            Texture2D GetIcon(CallTreeNodeType type)
            {
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
                base.SelectionChanged(selectedIds);
                if (selectedIds.Count > 0)
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

