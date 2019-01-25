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

    List<GameObject[]> selectionHistory;
    List<GameObject[]> lockedObjects;

    int maxHistoryCount = 32;
    bool ignoreNextSelection = false;



    void OnSelectionChange()
    {
        if (ignoreNextSelection)
        {
            ignoreNextSelection = false;
            return;
        }

        if (selectionHistory == null) selectionHistory = new List<GameObject[]>();
        if (lockedObjects == null) lockedObjects = new List<GameObject[]>();

        if (Selection.activeGameObject != null || Selection.gameObjects.Length > 0)
        {
            if (selectionHistory.Count > maxHistoryCount)
                selectionHistory.RemoveAt(0);

            if (selectionHistory.Count == 0 || CompareArray(selectionHistory[selectionHistory.Count - 1], Selection.gameObjects)) ;
            selectionHistory.Add(Selection.gameObjects);
            Repaint();
        }

    }

    public bool CompareArray(GameObject[] a, GameObject[] b)
    {
        return a.SequenceEqual(b);
    }

    private static bool CheckObjects(GameObject[] objs)
    {
        if (objs == null) return false;

        foreach (var obj in objs)
        {
            if (obj == null) return false;
        }
        return true;
    }

    void Selection_OnGUI()
    {
        if (selectionHistory == null) selectionHistory = new List<GameObject[]>();
        if (lockedObjects == null) lockedObjects = new List<GameObject[]>();
        int i = 0;

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Unselect All"))
            {
                Selection.activeObject = null;
                ignoreNextSelection = true;
                Repaint();
            }
        }
        int toRemove = -1;

        if (lockedObjects.Count > 0)
        {
            GUILayout.Label("Retained", EditorStyles.boldLabel);
            i = 0;
            toRemove = -1;
            foreach (var obj in lockedObjects)
            {
                if (obj == null || obj.Length == 0 || !CheckObjects(obj))
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
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label(Contents.star, GUILayout.Width(24));
                        string label = obj.Length > 1 ? "Multiple (" + obj.Length + ") : " + obj[0].name + " ..." : obj[0].name;
                        if (GUILayout.Button(label))
                        {
                            ignoreNextSelection = true;
                            Selection.objects = obj;
                        }
                        if (GUILayout.Button("F", GUILayout.Width(24)))
                        {
                            ignoreNextSelection = true;
                            Selection.objects = obj;
                            SceneView.lastActiveSceneView.FrameSelected();
                        }
                        if (GUILayout.Button("X", GUILayout.Width(24)))
                        {
                            toRemove = i;
                        }
                    }
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

        var reversedHistory = selectionHistory.Reverse<GameObject[]>().ToArray();
        foreach (var obj in reversedHistory)
        {
            if (obj == null || obj.Length == 0 || !CheckObjects(obj))
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
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Space(24);
                    string label = obj.Length > 1 ? "Multiple (" + obj.Length + ") : " + obj[0].name + " ..." : obj[0].name;
                    if (GUILayout.Button(label))
                    {
                        ignoreNextSelection = true;
                        Selection.objects = obj;
                    }
                    if (GUILayout.Button("F", GUILayout.Width(24)))
                    {
                        ignoreNextSelection = true;
                        Selection.objects = obj;
                        SceneView.lastActiveSceneView.FrameSelected();
                    }
                    if (GUILayout.Button("+", GUILayout.Width(24)))
                    {
                        toAdd = i;
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
            selectionHistory.RemoveAt(toRemove);
            Repaint();
        }

    }

    static class Contents
    {
        public static GUIContent title = new GUIContent("Select History");
        public static GUIContent star = EditorGUIUtility.IconContent("CustomSorting");
    }
}
