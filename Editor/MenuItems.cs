using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace GameplayIngredients.Editor
{
    public static class MenuItems
    {
        const int kSelectMenuPriority = 149;
        const int kPlayMenuPriority = 160;
        const int kMenuPriority = 330;

        [MenuItem("Edit/Select None &D", priority = kSelectMenuPriority)]
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
