using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    public class CheckWindow : EditorWindow
    {
        [MenuItem("Window/Gameplay Ingredients/Check and Resolve")]
        static void OpenWindow()
        {
            GetWindow<CheckWindow>(true);
        }

        private void OnEnable()
        {
            this.titleContent = new GUIContent("Check/Resolve");
            this.minSize = new Vector2(640, 180);
        }

        Vector2 Scroll = new Vector2();

        private void OnGUI()
        {
            using(new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Height(22)))
            {
                if (GUILayout.Button("Check", EditorStyles.toolbarButton))
                    PerformChecks();
                if(GUILayout.Button("", EditorStyles.toolbarPopup))
                {
                    GenericMenu menu = new GenericMenu();
                    
                }

                if (GUILayout.Button("Resolve", EditorStyles.toolbarButton))
                    PerformChecks();


                GUILayout.FlexibleSpace();
            }
            using(new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                Scroll = GUILayout.BeginScrollView(Scroll);
                GUILayout.Label("No Results");
                GUILayout.FlexibleSpace();
                GUILayout.EndScrollView();
            }
        }

        void PerformChecks()
        {

        }

    }

}
