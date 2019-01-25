using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
public class SelectionHistoryWindow : EditorWindow
{
    [MenuItem("Window/Selection History")]
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

    List<GameObject> selectionHistory;
    List<GameObject> lockedObjects;

    int maxHistoryCount = 32;
    bool ignoreNextSelection = false;

    void OnSelectionChange()
    {
        if (ignoreNextSelection)
        {
            ignoreNextSelection = false;
            return;
        }

        if (selectionHistory == null) selectionHistory = new List<GameObject>();
        if (lockedObjects == null) lockedObjects = new List<GameObject>();

        if (Selection.activeGameObject != null || Selection.gameObjects.Length > 0)
        {

            foreach(var go in Selection.gameObjects)
            {
                if (!selectionHistory.Contains(go))
                    selectionHistory.Add(go);
            }

            if (selectionHistory.Count > maxHistoryCount)
                selectionHistory.Take(maxHistoryCount);

            Repaint();
        }

    }

    public bool CompareArray(GameObject[] a, GameObject[] b)
    {
        return a.SequenceEqual(b);
    }

    void Selection_OnGUI()
    {
        if (selectionHistory == null) selectionHistory = new List<GameObject>();
        if (lockedObjects == null) lockedObjects = new List<GameObject>();
        int i = 0;
        int toRemove = -1;

        if (lockedObjects.Count > 0)
        {
            GUILayout.Label("Favorites", EditorStyles.boldLabel);
            i = 0;
            toRemove = -1;
            foreach (var obj in lockedObjects)
            {
                if (obj == null)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("(object is either null or has been deleted)");
                        if (GUILayout.Button("X", GUILayout.Width(24)))
                        {
                            toRemove = i;
                        }
                    }
                }
                else
                {
                    bool highlight = Selection.gameObjects.Contains(obj);
                    Color backup = GUI.color;

                    if (highlight)
                        GUI.color = Styles.highlightColor;

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label(Contents.star, EditorStyles.miniLabel, GUILayout.Width(24));
                        string label = obj.name;

                        if (GUILayout.Button(label, EditorStyles.foldout))
                        {
                            ignoreNextSelection = true;
                            Selection.activeObject = obj;
                        }
                        if (GUILayout.Button("Focus", EditorStyles.miniButton, GUILayout.Width(40)))
                        {
                            ignoreNextSelection = true;
                            Selection.activeObject = obj;
                            SceneView.lastActiveSceneView.FrameSelected();
                        }
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(24)))
                        {
                            toRemove = i;
                        }
                    }
                    var rect = GUILayoutUtility.GetLastRect();
                    EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 0.5f));

                    GUI.color = backup;
                }
                i++;
            }
            if (toRemove != -1) lockedObjects.RemoveAt(toRemove);
        }

        int toAdd = -1;
        toRemove = -1;
        i = 0;
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("History", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear"))
            {
                selectionHistory.Clear();
                Repaint();
            }
        }

        GUILayout.Space(8);

        var reversedHistory = selectionHistory.Reverse<GameObject>().ToArray();
        foreach (var obj in reversedHistory)
        {
            if (obj == null)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("(object is either null or has been deleted)");
                    if (GUILayout.Button("X", GUILayout.Width(24)))
                    {
                        toRemove = i;
                    }
                }

            }
            else
            {
                bool highlight = Selection.gameObjects.Contains(obj);
                Color backup = GUI.color;

                if (highlight)
                    GUI.color = Styles.highlightColor;

                using (new EditorGUILayout.HorizontalScope())
                {
                    
                    GUILayout.Space(16);
                    string label = obj.name;
                    if (GUILayout.Button(label, EditorStyles.foldout))
                    {
                        ignoreNextSelection = true;
                        Selection.activeObject = obj;
                    }
                    if (GUILayout.Button("Focus", Styles.historyButton, GUILayout.Width(40)))
                    {
                        ignoreNextSelection = true;
                        Selection.activeObject = obj;
                        SceneView.lastActiveSceneView.FrameSelected();
                    }
                    if (GUILayout.Button(Contents.star, Styles.historyButton, GUILayout.Width(24)))
                    {
                        toAdd = i;
                    }
                }
                var rect = GUILayoutUtility.GetLastRect();
                EditorGUI.DrawRect(rect, new Color(0.2f,0.2f,0.2f,0.5f));

                GUI.color = backup;
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
            selectionHistory.RemoveAt(toRemove);
            Repaint();
        }

    }

    static class Styles
    {
        public static GUIStyle historyButton;
        public static GUIStyle highlight;
        public static Color highlightColor = new Color(2.0f, 2.0f, 2.0f);

        static Styles()
        {
            historyButton = new GUIStyle(EditorStyles.miniButton);
            historyButton.alignment = TextAnchor.MiddleLeft;
            highlight = new GUIStyle(EditorStyles.miniLabel);
            highlight.onNormal.background = Texture2D.whiteTexture;
            highlight.onHover.background = Texture2D.whiteTexture;
            highlight.onActive.background = Texture2D.whiteTexture;
            highlight.onFocused.background = Texture2D.whiteTexture;

        }
    }

    static class Contents
    {
        public static GUIContent title = new GUIContent("Selection History");
        public static GUIContent star = EditorGUIUtility.IconContent("CustomSorting");
    }
}
