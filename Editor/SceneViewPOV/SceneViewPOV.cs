using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace GameplayIngredients.Editor
{
    public class SceneViewPOV : PopupWindowContent
    {
        static GameObject POVRoot;
        static GameObject[] ALlPOVRoots;

        const string kPOVObjectName = "__SceneView__POV__";
        const string kPOVRootTag = "SceneViewPOVRoot";

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            SceneManager.activeSceneChanged -= EditorSceneManager_activeSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;

            SceneManager.activeSceneChanged += EditorSceneManager_activeSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private static void SceneManager_sceneUnloaded(Scene arg0)
        {
            CheckPOVGameObjects();
        }

        private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            CheckPOVGameObjects();
        }

        private static void EditorSceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            CheckPOVGameObjects();
        }

        public static void CheckPOVGameObjects()
        {
            var activePov = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault<GameObject>(o => o.name == kPOVObjectName && o.tag == kPOVRootTag);

            if (activePov == null)
            {
                activePov = CreatePOVRootObject();
            }

            POVRoot = activePov;
            ALlPOVRoots = GameObject.FindGameObjectsWithTag(kPOVRootTag);
        }

        static GameObject CreatePOVRootObject()
        {
            var povRoot = new GameObject(kPOVObjectName);
            povRoot.isStatic = true;
            povRoot.tag = kPOVRootTag;
            povRoot.hideFlags = HideFlags.HideInHierarchy;
            return povRoot;
        }

        static GameObject CreatePOV(GameObject povRoot, string name, Transform transform)
        {
            var pov = new GameObject(name);
            pov.isStatic = true;
            pov.hideFlags = HideFlags.HideInHierarchy;
            pov.transform.position = transform.position;
            pov.transform.rotation = transform.rotation;
            pov.transform.parent = povRoot.transform;
            return pov;
        }

        public static void ShowPopup(Rect buttonRect, SceneView sceneView)
        {
            PopupWindow.Show(buttonRect, new SceneViewPOV(sceneView));
        }

        private SceneView m_SceneView;

        public SceneViewPOV(SceneView sceneView)
        {
            m_SceneView = sceneView;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(256.0f, 480.0f);
        }

        public override void OnGUI(Rect rect)
        {
            if (POVRoot == null)
                CheckPOVGameObjects();

            if (POVRoot != null && SceneView.lastActiveSceneView != null)
            {
                var povs = GameObject.FindGameObjectsWithTag("POV");

                GUILayout.Label("Go to POVs", EditorStyles.boldLabel);
                foreach (var pov in povs.OrderBy(o => o.name))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(pov.name))
                        {
                            SceneView.lastActiveSceneView.AlignViewToObject(pov.transform);
                        }
                        if (GUILayout.Button("X", GUILayout.Width(32)))
                        {
                            if (EditorUtility.DisplayDialog("Destroy POV?", "Do you want to destroy this POV: " + pov.name + " ?", "Yes", "No")) ;
                            //Destroy(pov);
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No POV Root found (Create an Empty Game Object named 'POV_ROOT') or no SceneView currently active", MessageType.Warning);
            }

        }
    }
}

