using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameplayIngredients.Editor
{
    static class SceneViewToolbar
    {
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
                    bool link = LinkGameView.Active;
                    link = GUILayout.Toggle(link, "Link Game View", EditorStyles.toolbarButton);

                    if (GUI.changed)
                        LinkGameView.Active = link;

                    GUILayout.FlexibleSpace();
                }
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

