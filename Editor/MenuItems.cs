using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace GameplayIngredients.Editor
{
    public static class MenuItems
    {
        const int kSelectMenuPriority = 149;
        const int kPlayMenuPriority = 160;
        const int kMenuPriority = 330;

        [MenuItem("Edit/Unelect All #D", priority = kSelectMenuPriority)]
        static void UnselectAll()
        {
            Selection.activeObject = null;
        }

        [MenuItem("Edit/Play from SceneView Position #%&P", priority = kPlayMenuPriority)]
        static void PlayHere()
        {
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Edit/Play from SceneView Position #%&P", priority = kPlayMenuPriority, validate = true)]
        static bool PlayHereValidate()
        {
            return PlayFromHere.IsReady;
        }

        #region EXCLUSION LIST
        [MenuItem("Assets/Create/Gameplay Ingredients/Manager Exclusion List")]
        static void CreateExclusionList()
        {
            AssetFactory.CreateAssetInProjectWindow<ManagerExclusionList>(string.Empty, "ManagerExclusionList.asset");
        }
        #endregion

        #region GROUP_UNGROUP

        const int kGroupMenuIndex = 500;
        const string kGroupMenuString = "Edit/Group Selected %G";
        const string kUnGroupMenuString = "Edit/Un-Group Selected %#G";

        [MenuItem(kGroupMenuString, priority = kGroupMenuIndex, validate = false)]
        static void Group()
        {
            if (Selection.gameObjects.Length <= 1)
                return;

            var selected = Selection.gameObjects;
            Transform parent = selected[0].transform.parent;
            Scene scene = selected[0].scene;

            bool sparseParents = false;

            foreach (var obj in selected)
            {
                if (obj.transform.parent != parent || obj.scene != scene)
                {
                    sparseParents = true;
                    break;
                }
            }

            if (sparseParents)
            {
                parent = null;
                scene = SceneManager.GetActiveScene();
            }

            Vector3 posSum = Vector3.zero;

            foreach (var go in selected)
            {
                posSum += go.transform.position;
            }

            GameObject groupObj = new GameObject("Group");
            groupObj.transform.position = posSum / selected.Length;
            groupObj.transform.parent = parent;
            groupObj.isStatic = true;

            foreach (var go in selected)
                go.transform.parent = groupObj.transform;

            // Expand by pinging the first object
            EditorGUIUtility.PingObject(selected[0]);
            
        }

        [MenuItem(kGroupMenuString, priority = kGroupMenuIndex, validate = true)]
        static bool GroupCheck()
        {
            return (Selection.gameObjects.Length > 1);
        }


        [MenuItem(kUnGroupMenuString, priority = kGroupMenuIndex+1, validate = false)]
        static void UnGroup()
        {
            if (Selection.gameObjects.Length == 0)
                return;

            var selected = Selection.gameObjects;
            List<Transform> oldParents = new List<Transform>();
            foreach(var go in selected)
            {
                if(go.transform.parent != null)
                {
                    if(!oldParents.Contains(go.transform.parent))
                        oldParents.Add(go.transform.parent);

                    go.transform.parent = go.transform.parent.parent;
                }
            }

            List<GameObject> toDelete = new List<GameObject>();

            // Cleanup old parents
            foreach(var parent in oldParents)
            {
                var go = parent.gameObject;
                if(parent.childCount == 0 && parent.GetComponents<Component>().Length == 1) // if no more children and only transform/rectTransform
                {
                    toDelete.Add(go);
                }
            }

            foreach (var trash in toDelete)
                GameObject.DestroyImmediate(trash);
            
        }

        [MenuItem(kUnGroupMenuString, priority = kGroupMenuIndex+1, validate = true)]
        static bool UnGroupCheck()
        {
            return (Selection.gameObjects.Length > 0);
        }

        #endregion

        #region TOGGLE GIZMOS

        static bool s_ShowAllGizmos = true;
        const string kToggleGizmosMenu = "Edit/Show Gizmos #G";
        [MenuItem(kToggleGizmosMenu, priority = kMenuPriority)]
        static void ToggleGizmos()
        {
            s_ShowAllGizmos = !s_ShowAllGizmos;
            SetAllAnnotations(AnnotationType.Gizmo, s_ShowAllGizmos);
            SetAllAnnotations(AnnotationType.Icon, s_ShowAllGizmos);
            Menu.SetChecked(kToggleGizmosMenu, s_ShowAllGizmos);
        }

        [MenuItem(kToggleGizmosMenu, priority = 600, validate = true)]
        static bool CheckToggleGizmos()
        {
            Menu.SetChecked(kToggleGizmosMenu, s_ShowAllGizmos);
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            return true;
        }

        enum AnnotationType
        {
            Gizmo,
            Icon
        }

        static void SetAllAnnotations(AnnotationType type, bool value)
        {
            var Annotation = Type.GetType("UnityEditor.Annotation, UnityEditor");
            var ClassId = Annotation.GetField("classID");
            var ScriptClass = Annotation.GetField("scriptClass");

            Type AnnotationUtility = Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
            var GetAnnotations = AnnotationUtility.GetMethod("GetAnnotations", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var SetGizmoEnabled = AnnotationUtility.GetMethod("SetGizmoEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var SetIconEnabled = AnnotationUtility.GetMethod("SetIconEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            Array annotations = (Array)GetAnnotations.Invoke(null, null);
            foreach (var a in annotations)
            {
                int classId = (int)ClassId.GetValue(a);
                string scriptClass = (string)ScriptClass.GetValue(a);

                switch (type)
                {
                    case AnnotationType.Gizmo:
                        SetGizmoEnabled.Invoke(null, new object[] { classId, scriptClass, value ? 1 : 0 });
                        break;
                    case AnnotationType.Icon:
                        SetIconEnabled.Invoke(null, new object[] { classId, scriptClass, value ? 1 : 0 });
                        break;
                }
            }
        }
        #endregion
    }
}
