using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

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

        protected List<MonoBehaviour> m_Watches;

        private void OnEnable()
        {
            m_Watches = new List<MonoBehaviour>();
            m_TreeView = new CallTreeView(m_Watches);
            m_TreeView.Reload();
            titleContent = new GUIContent("Callable Tree Explorer");
        }

        private void OnGUI()
        {
            int tbHeight = 24;
            using(new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Height(tbHeight)))
            {
                if(GUILayout.Button("Get",EditorStyles.toolbarButton))
                {
                    Debug.Log("Get");
                }
                GUILayout.FlexibleSpace();
            }
            Rect r = GUILayoutUtility.GetRect(position.width, position.height - tbHeight);
            m_TreeView.OnGUI(r);
        }

        class CallTreeNode
        {
            public string Name;
            public Texture Icon;
            public GameObject Target;
            public List<CallTreeNode> Children;

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
        }

        class CallTreeView : TreeView
        {
            List<MonoBehaviour> m_Roots;
            public CallTreeView(List<MonoBehaviour> roots) : base(new TreeViewState())
            {
                m_Roots = roots;
            }
            
            protected override TreeViewItem BuildRoot()
            {
                int id = 0;
                var treeRoot = new TreeViewItem(id++, -1, "~Root");
                foreach(var bh in m_Roots)
                {
                    var root_item = new TreeViewItem(id++, 0, bh.name);
                    treeRoot.AddChild(root_item);
                }
                return treeRoot;
            }
        }
    }
}

