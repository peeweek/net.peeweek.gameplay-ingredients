using System.Reflection;
using System;
using System.Linq;
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

        static readonly Dictionary<string, Type> s_assemblyTypes = GetTypes();

        private static Dictionary<string,Type> GetTypes()
        {
            Dictionary<string, Type> all = new Dictionary<string, Type>();

            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type t in assembly.GetTypes())
                {
                    if(typeof(Component).IsAssignableFrom(t) && !all.ContainsKey(t.Name))
                        all.Add(t.Name, t);
                }
            }

            return all;
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

        enum SearchOp
        {
            Find,
            Add,
            Refine
        }

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
                    SearchButtonsGUI(searchBy, nameSearch);
                    break;
                case SearchBy.Tag:
                    tagSearch = EditorGUILayout.TextField(Contents.tagSearch, tagSearch);
                    SearchButtonsGUI(searchBy, tagSearch);
                    break;
                case SearchBy.Layer:
                    layerSearch = EditorGUILayout.TextField(Contents.layerSearch, layerSearch);
                    SearchButtonsGUI(searchBy, layerSearch);
                    break;
                case SearchBy.ComponentType:
                    componentSearch = EditorGUILayout.TextField(Contents.componentSearch, componentSearch);
                    SearchButtonsGUI(searchBy, componentSearch);
                    break;
                case SearchBy.Mesh:
                    meshSearch = (Mesh)EditorGUILayout.ObjectField(Contents.meshSearch, meshSearch, typeof(Mesh), true);
                    SearchButtonsGUI(searchBy, meshSearch);
                    break;
                case SearchBy.Material:
                    materialSearch = (Material)EditorGUILayout.ObjectField(Contents.materialSearch, materialSearch, typeof(Material), true);
                    SearchButtonsGUI(searchBy, materialSearch);
                    break;
            }


            GUILayout.FlexibleSpace();
            GUILayout.Label("Replace By", EditorStyles.boldLabel);
            prefabReplacement = (GameObject)EditorGUILayout.ObjectField(Contents.prefabReplacement, prefabReplacement, typeof(GameObject), true);
            GUILayout.Button("Replace All", GUILayout.Height(32));
            GUILayout.Space(8);
        }

        void SearchButtonsGUI(SearchBy by, object criteria)
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Find", GUILayout.Height(32)))
                    Search(SearchOp.Find, by, criteria);

                if (GUILayout.Button("Add", GUILayout.Height(32)))
                    Search(SearchOp.Add, by, criteria);

                if (GUILayout.Button("Refine", GUILayout.Height(32)))
                    Search(SearchOp.Refine, by, criteria);

            }
        }

        void Search(SearchOp op, SearchBy by, object criteria)
        {
            List<GameObject> query = new List<GameObject>();
            
            switch(by)
            {
                case SearchBy.Name:
                    foreach(var go in FindObjectsOfType<GameObject>())
                    {
                        if (go.name.Contains((string)criteria))
                            query.Add(go);
                    }
                    break;
                case SearchBy.Tag:
                    query.AddRange(GameObject.FindGameObjectsWithTag((string)criteria));
                    break;
                case SearchBy.Layer:
                    foreach (var go in FindObjectsOfType<GameObject>())
                    {
                        if (go.layer == LayerMask.NameToLayer((string)criteria))
                            query.Add(go);
                    }
                    break;
                case SearchBy.ComponentType:
                    if(s_assemblyTypes.ContainsKey((string)criteria))
                    {
                        Type t = s_assemblyTypes[(string)criteria];
                        if( typeof(Component).IsAssignableFrom(t))
                        {
                            Component[] components = (Component[])FindObjectsOfType(t);
                            if(components != null)
                            {
                                foreach(var c in components)
                                {
                                    if (!query.Contains(c.gameObject))
                                        query.Add(c.gameObject);
                                }
                            }
                        }
                    }
                    break;
                case SearchBy.Mesh:
                    Mesh mesh = (Mesh)criteria;
                    foreach (var go in FindObjectsOfType<GameObject>())
                    {
                        MeshFilter filter = go.GetComponent<MeshFilter>();
                        if (filter != null && filter.sharedMesh == mesh)
                        {
                            query.Add(go);
                        }
                    }
                    break;
                case SearchBy.Material:
                    Material mat = (Material)criteria;
                    foreach (var go in FindObjectsOfType<GameObject>())
                    {
                        Renderer renderer = go.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            if(renderer.sharedMaterials.Contains(mat))
                            {
                                query.Add(go);
                            }
                        }
                    }
                    break;
            }

            switch (op)
            {
                case SearchOp.Find:
                    searchResults = query;
                    break;
                case SearchOp.Add:
                    foreach(var item in query)
                    {
                        if (!searchResults.Contains(item))
                            searchResults.Add(item);
                    }
                    break;
                case SearchOp.Refine:
                    List<GameObject> refined = new List<GameObject>();
                    foreach (var item in searchResults)
                    {
                        if (query.Contains(item))
                            refined.Add(item);
                    }
                    searchResults = refined;
                    break;
            }
        }

        [SerializeField]
        List<GameObject> searchResults= new List<GameObject>();
        Vector2 scroll;

        void SearchResultsGUI()
        {
            GUILayout.Space(8);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Search Results", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("From Selection"))
                {
                    searchResults = Selection.gameObjects.ToList();
                }
                if(GUILayout.Button("Select"))
                {
                    Selection.objects = searchResults.ToArray();
                }
            }

            scroll = GUILayout.BeginScrollView(scroll, EditorStyles.helpBox);
            {
                GameObject toRemove = null;

                foreach(var obj in searchResults)
                {
                    using (new GUILayout.HorizontalScope(EditorStyles.textField))
                    {
                        GUILayout.Label(obj.name, EditorStyles.label);
                        if(GUILayout.Button("X", GUILayout.Width(32)))
                        {
                            toRemove = obj;
                        }
                    }
                }

                if (toRemove != null)
                    searchResults.Remove(toRemove);
            }
            GUILayout.EndScrollView();

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
