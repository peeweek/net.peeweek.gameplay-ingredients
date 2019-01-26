using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameplayIngredients.Editor
{
    static class SceneViewToolbar
    {
        public delegate void SceneViewToolbarDelegate(SceneView sceneView);

        public static event SceneViewToolbarDelegate OnSceneViewToolbarGUI;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {

            var r = new Rect(Vector2.zero, new Vector2(sceneView.position.width,24));

            using (new GUILayout.AreaScope(r))
            {
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    bool play = GUILayout.Toggle(EditorApplication.isPlaying, Contents.playFromHere, EditorStyles.toolbarButton);

                    if(GUI.changed)
                    {
                        if (play)
                            PlayFromHere.Play();
                        else
                            EditorApplication.isPlaying = false;
                    }

                    GUILayout.Space(24);

                    bool isLinked = LinkGameView.Active;
                    isLinked = GUILayout.Toggle(isLinked, Contents.linkGameView, EditorStyles.toolbarButton, GUILayout.Width(64));

                    if (GUI.changed)
                    {
                        LinkGameView.Active = isLinked;
                    }

                    bool isLocked = LinkGameView.LockedSceneView == sceneView;
                    isLocked = GUILayout.Toggle(isLocked, Contents.lockLinkGameView, EditorStyles.toolbarButton);

                    if (GUI.changed)
                    {
                        if (isLocked)
                        {
                            LinkGameView.LockedSceneView = sceneView;
                        }
                        else
                        {
                            LinkGameView.LockedSceneView = null;
                        }
                    }

                    GUILayout.FlexibleSpace();

                    // Custom Code here
                    if (OnSceneViewToolbarGUI != null)
                        OnSceneViewToolbarGUI.Invoke(sceneView);

                    // Saving Space not to overlap view controls
                    GUILayout.Space(96);

                }
            }
        }

        static class Contents
        {
            public static GUIContent playFromHere;
            public static GUIContent lockLinkGameView;
            public static GUIContent linkGameView;

            static Contents()
            {
                lockLinkGameView = new GUIContent(EditorGUIUtility.IconContent("IN LockButton"));
                linkGameView = new GUIContent(EditorGUIUtility.Load("Packages/net.peeweek.gameplay-ingredients/Icons/GUI/Camera16x16.png") as Texture);
                linkGameView.text = " Game";

                playFromHere = new GUIContent(EditorGUIUtility.IconContent("Animation.Play"));
                playFromHere.text = "Here";
            }
        }

        static class Styles
        {
            public static GUIStyle toolbar;

            static Styles()
            {
                toolbar = new GUIStyle(EditorStyles.inspectorFullWidthMargins);                
            }
        }
    }
}

