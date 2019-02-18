using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace GameplayIngredients.Editor
{
    public class FindAndReplaceWindow : EditorWindow
    {
        [MenuItem("Edit/Find And Replace... %&#F", priority = 144)]
        static void OpenWindow()
        {
            GetWindow<FindAndReplaceWindow>();
        }

        public enum SearchBy
        {
            Name,
            ComponentType,
            Tag,
            Layer,
            Mesh,
            Material,
        }

        private void OnEnable()
        {
            titleContent = Contents.title;
        }

        private void OnGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope(GUILayout.Width(320)))
                {
                    SearchControlsGUI();
                }

                using (new GUILayout.VerticalScope(GUILayout.Width(4)))
                {
                    GUILayout.FlexibleSpace();
                    Rect r = GUILayoutUtility.GetLastRect();
                    EditorGUI.DrawRect(r, Color.black);
                }

                using (new GUILayout.VerticalScope())
                {
                    SearchResultsGUI();
                }

            }
        }

        [SerializeField]
        GameObject prefabReplacement;
        [SerializeField]
        SearchBy searchBy;

        [SerializeField]
        string nameSearch = "GameObject";
        [SerializeField]
        string tagSearch = "Player";
        [SerializeField]
        string layerSearch = "PostProcessing";
        [SerializeField]
        string componentSearch = "Light";
        [SerializeField]
        Mesh meshSearch;
        [SerializeField]
        Material materialSearch;


        void SearchControlsGUI()
        {
            EditorGUIUtility.labelWidth = 120;
            GUILayout.Space(8);
            GUILayout.Label("Search and Filter", EditorStyles.boldLabel);
            searchBy = (SearchBy)EditorGUILayout.EnumPopup(Contents.searchBy, searchBy);
            switch(searchBy)
            {
                case SearchBy.Name:
                    nameSearch = EditorGUILayout.TextField(Contents.nameSearch, nameSearch);
                    break;
                case SearchBy.Tag:
                    tagSearch = EditorGUILayout.TextField(Contents.tagSearch, tagSearch);
                    break;
                case SearchBy.Layer:
                    layerSearch = EditorGUILayout.TextField(Contents.layerSearch, layerSearch);
                    break;
                case SearchBy.ComponentType:
                    componentSearch = EditorGUILayout.TextField(Contents.componentSearch, componentSearch);
                    break;
                case SearchBy.Mesh:
                    meshSearch = (Mesh)EditorGUILayout.ObjectField(Contents.meshSearch, meshSearch, typeof(Mesh), true);
                    break;
                case SearchBy.Material:
                    materialSearch = (Material)EditorGUILayout.ObjectField(Contents.materialSearch, materialSearch, typeof(Material), true);

                    break;
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Button("Find",GUILayout.Height(32));
                GUILayout.Button("Refine",GUILayout.Height(32));
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Replace By", EditorStyles.boldLabel);
            prefabReplacement = (GameObject)EditorGUILayout.ObjectField(Contents.prefabReplacement, prefabReplacement, typeof(GameObject), true);
            GUILayout.Button("Replace All", GUILayout.Height(32));
            GUILayout.Space(8);
        }

        [SerializeField]
        List<GameObject> searchResults= new List<GameObject>();
        Vector2 scroll;

        void SearchResultsGUI()
        {
            GUILayout.Space(8);
            GUILayout.Label("Search Results", EditorStyles.boldLabel);

            using (new GUILayout.ScrollViewScope(scroll, EditorStyles.helpBox))
            {
                foreach(var obj in searchResults)
                {
                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label(obj.name);
                        GUILayout.Button("X", GUILayout.Width(32));
                    }
                }
            }
        }

        static class Contents
        {
            public static GUIContent title = new GUIContent("Find and Replace", (Texture)EditorGUIUtility.LoadRequired("ViewToolZoom On"));
            public static GUIContent searchBy = new GUIContent("Search by");
            public static GUIContent nameSearch = new GUIContent("GameObject Name");
            public static GUIContent tagSearch = new GUIContent("Tag");
            public static GUIContent layerSearch = new GUIContent("Layer");
            public static GUIContent componentSearch = new GUIContent("Component Type");
            public static GUIContent meshSearch = new GUIContent("Mesh");
            public static GUIContent materialSearch = new GUIContent("Material");

            public static GUIContent prefabReplacement = new GUIContent("Prefab Replacement");
        }
    }
}
