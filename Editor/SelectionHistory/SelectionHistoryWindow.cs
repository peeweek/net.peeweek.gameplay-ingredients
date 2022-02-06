using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    public class SelectionHistoryWindow : EditorWindow
    {
        [MenuItem("Window/General/Selection History")]
        public static void OpenSelectionHistoryWindow()
        {
            EditorWindow.GetWindow<SelectionHistoryWindow>();
        }

        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {
            titleContent = Contents.title;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                Selection_OnGUI();
            }
            EditorGUILayout.EndScrollView();
        }

        void OnEnable()
        {
            lockedObjects = null;
            selectionHistory = null;
        }

        void OnDisable()
        {
            lockedObjects = null;
            selectionHistory = null;
        }

        List<UnityEngine.Object> selectionHistory;
        List<UnityEngine.Object> lockedObjects;

        int maxHistoryCount = 32;
        bool ignoreNextSelection = false;

        void OnSelectionChange()
        {
            if (ignoreNextSelection)
            {
                ignoreNextSelection = false;
                return;
            }

            if (selectionHistory == null) selectionHistory = new List<UnityEngine.Object>();
            if (lockedObjects == null) lockedObjects = new List<UnityEngine.Object>();

            if (Selection.activeObject != null || Selection.objects.Length > 0)
            {

                foreach(var obj in Selection.objects)
                {
                    if (!selectionHistory.Contains(obj))
                        selectionHistory.Add(obj);
                }

                if (selectionHistory.Count > maxHistoryCount)
                    selectionHistory.Take(maxHistoryCount);

                Repaint();
            }

        }

        public bool CompareArray(UnityEngine.Object[] a, UnityEngine.Object[] b)
        {
            return a.SequenceEqual(b);
        }

        void Selection_OnGUI()
        {
            if (selectionHistory == null) selectionHistory = new List<UnityEngine.Object>();
            if (lockedObjects == null) lockedObjects = new List<UnityEngine.Object>();
            int i = 0;
            int toRemove = -1;

            if (lockedObjects.Count > 0)
            {
                GUI.backgroundColor = new Color(0, 0, 0, 0.2f);
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("Favorites", EditorStyles.boldLabel);
                }

                GUI.backgroundColor = Color.white;
                i = 0;
                toRemove = -1;
                foreach (var obj in lockedObjects)
                {
                    
                    if (obj == null)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            GUILayout.Label("(object is either null or has been deleted)");
                            if (GUILayout.Button("�", GUILayout.Width(16)))
                            {
                                toRemove = i;
                            }
                        }
                    }
                    else
                    {
                        using (new EditorGUILayout.HorizontalScope(Styles.historyLine))
                        {
                            var b = GUI.color;
                            GUI.color = Color.yellow * 3;
                            if (GUILayout.Button(Contents.star, Styles.icon, GUILayout.Width(16)))
                            {
                                toRemove = i;
                            }

                            GUI.color = b;

                            GUIContent label = EditorGUIUtility.ObjectContent(obj, obj.GetType());
                            string name = label.text;
                            label.text = string.Empty;
                            GUILayout.Label(label, Styles.icon);
                            if (GUILayout.Button(name, Styles.historyItem))
                            {
                                ignoreNextSelection = true;
                                Selection.activeObject = obj;
                            }

                            if (obj is GameObject && GUILayout.Button("Focus", Styles.historyButton, GUILayout.Width(48)))
                            {
                                ignoreNextSelection = true;
                                Selection.activeObject = obj;
                                SceneView.lastActiveSceneView.FrameSelected();
                            }
                        }
                    }
                    i++;
                }
                if (toRemove != -1) lockedObjects.RemoveAt(toRemove);

                GUILayout.Space(8);
            }

            int toAdd = -1;
            toRemove = -1;
            i = 0;
            
            GUI.backgroundColor = new Color(0, 0, 0, 0.2f);
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("History", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
                {
                    selectionHistory.Clear();
                    Repaint();
                }
            }



            GUI.backgroundColor = Color.white;
            var reversedHistory = selectionHistory.Reverse<UnityEngine.Object>().ToArray();
            foreach (var obj in reversedHistory)
            {
                if (obj != null)
                {
                    bool highlight = Selection.gameObjects.Contains(obj);

                    using (new EditorGUILayout.HorizontalScope(Styles.historyLine))
                    {
                        bool favorited = lockedObjects.Contains(obj);

                        if (GUILayout.Button(favorited ? Contents.star : Contents.starDisabled, Styles.icon, GUILayout.Width(16)))
                        {
                            if (!favorited)
                                toAdd = i;
                            else
                                toRemove = i;
                        }

                        GUIContent label = EditorGUIUtility.ObjectContent(obj, obj.GetType());
                        string name = label.text;
                        label.text = string.Empty;
                        GUILayout.Label(label, Styles.icon);
                        if (GUILayout.Button(name, Styles.historyItem))
                        {
                            ignoreNextSelection = true;
                            Selection.activeObject = obj;
                        }


                        if (obj is GameObject && GUILayout.Button("Focus", Styles.historyButton, GUILayout.Width(48)))
                        {
                            ignoreNextSelection = true;
                            Selection.activeObject = obj;
                            SceneView.lastActiveSceneView.FrameSelected();
                        }
                    }

                }

                i++;
            }
            if (toAdd != -1)
            {
                lockedObjects.Add(reversedHistory[toAdd]);
                Repaint();
            }
            if (toRemove != -1)
            {
                lockedObjects.Remove(reversedHistory[toRemove]);
                Repaint();
            }

        }

        static class Styles
        {
            public static GUIStyle historyLine;
            public static GUIStyle historyItem;
            public static GUIStyle historyButton;
            public static GUIStyle highlight;
            public static Color highlightColor = new Color(2.0f, 2.0f, 2.0f);

            public static GUIStyle icon;

            static Styles()
            {
                historyLine = new GUIStyle(EditorStyles.toolbarButton);

                historyItem = new GUIStyle(EditorStyles.foldout);
                historyItem.active.background = Texture2D.blackTexture;
                historyItem.onActive.background = Texture2D.blackTexture;
                historyItem.focused.background = Texture2D.blackTexture;
                historyItem.onFocused.background = Texture2D.blackTexture;
                historyItem.hover.background = Texture2D.blackTexture;
                historyItem.onHover.background = Texture2D.blackTexture;
                historyItem.normal.background = Texture2D.blackTexture;
                historyItem.onNormal.background = Texture2D.blackTexture;
                historyItem.fixedHeight = 16;
                historyItem.padding = new RectOffset();


                historyButton = new GUIStyle(EditorStyles.miniButton);
                historyButton.alignment = TextAnchor.MiddleLeft;

                highlight = new GUIStyle(EditorStyles.miniLabel);
                highlight.onNormal.background = Texture2D.whiteTexture;
                highlight.onHover.background = Texture2D.whiteTexture;
                highlight.onActive.background = Texture2D.whiteTexture;
                highlight.onFocused.background = Texture2D.whiteTexture;

                icon = new GUIStyle(EditorStyles.label);
                icon.fixedHeight = 16;
                icon.fixedWidth = 16;
                icon.padding = new RectOffset();
                icon.margin = new RectOffset(0,8,0,0);

            }
        }

        static class Contents
        {
            public static GUIContent title = new GUIContent("Selection History");
            public static GUIContent star = new GUIContent(EditorGUIUtility.IconContent("Favorite Icon").image);
            public static GUIContent starDisabled = new GUIContent(EditorGUIUtility.IconContent("Favorite").image);
        }
    }
}
