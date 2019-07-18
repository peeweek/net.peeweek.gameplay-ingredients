using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace GameplayIngredients.Editor
{
    public class NewSceneWindow : EditorWindow
    {
        const int WindowWidth = 640;
        const int WindowHeight = 380;

        [MenuItem("File/New Scene From Template... &%N", priority = 1)]
        static void ShowNewSceneWindow()
        {
            GetWindow<NewSceneWindow>(true);
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("New Scene From Template");
            this.position = new Rect((Screen.width / 2.0f) - WindowWidth / 2, (Screen.height / 2.0f) - WindowHeight / 2, WindowWidth, WindowHeight);
            this.minSize = new Vector2(WindowWidth, WindowHeight);
            this.maxSize = new Vector2(WindowWidth, WindowHeight);
            ReloadList();
        }

        private void OnGUI()
        {
            GUILayout.Label("Available Templates");
            using (new GUILayout.HorizontalScope(GUILayout.ExpandHeight(true)))
            {
                using (new GUILayout.VerticalScope(Styles.helpBox, GUILayout.Width(180)))
                {
                    DrawTemplateList();
                }
                GUILayout.Space(8);
                using (new GUILayout.VerticalScope(Styles.helpBox))
                {
                    DrawTemplateDetails();
                }
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Create"))
                {
                    // Do Something
                    this.Close();
                }
            }
        }

        public static void ReloadList()
        {
            if (templateLists == null)
                templateLists = new List<SceneTemplateList>();
            else
                templateLists.Clear();

            string[] all = AssetDatabase.FindAssets("t:" + typeof(SceneTemplateList).Name);
            foreach(var guid in all)
            {
                templateLists.Add(AssetDatabase.LoadAssetAtPath<SceneTemplateList>(AssetDatabase.GUIDToAssetPath(guid)));
            }
        }

        static List<SceneTemplateList> templateLists;

        Vector2 scrollList = Vector2.zero;

        void DrawTemplateList()
        {
            GUILayout.BeginScrollView(scrollList);

            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
        }

        void DrawTemplateDetails()
        {
            GUILayout.FlexibleSpace();
        }

        static class Styles
        {
            public static GUIStyle buttonLeft;
            public static GUIStyle buttonMid;
            public static GUIStyle buttonRight;

            public static GUIStyle helpBox;

            static Styles()
            {
                buttonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
                buttonMid = new GUIStyle(EditorStyles.miniButtonMid);
                buttonRight = new GUIStyle(EditorStyles.miniButtonRight);
                buttonLeft.fontSize = 12;
                buttonMid.fontSize = 12;
                buttonRight.fontSize = 12;

                helpBox = new GUIStyle(EditorStyles.helpBox);
                helpBox.padding = new RectOffset(12, 12, 12, 12);
            }
        }
    }
}
