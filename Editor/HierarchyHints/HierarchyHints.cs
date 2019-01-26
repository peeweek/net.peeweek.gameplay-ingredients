using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEditor;
using GameplayIngredients;
using UnityEngine.Playables;

namespace GameplayIngredients.Editor
{

    [InitializeOnLoad]
    public static class HierarchyHints
    {
        static HierarchyHints()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyOnGUI;
        }

        static void HierarchyOnGUI(int instanceID, Rect selectionRect)
        {
            var fullRect = selectionRect;
            fullRect.xMin = 0;
            fullRect.xMax = EditorGUIUtility.currentViewWidth;
            GameObject o = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (o == null) return;
            
            var c = GUI.color;
            if (o.isStatic)
            {
                GUI.Label(fullRect, "(s)");
                EditorGUI.DrawRect(fullRect, Colors.dimGray);
            }

            if (o.GetComponents<MonoBehaviour>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.monoBehaviourIcon, Color.white);
            if (o.GetComponents<MeshRenderer>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.meshIcon, Color.white);
            if (o.GetComponents<Collider>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.colliderIcon, Color.white);
            if (o.GetComponents<Camera>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.cameraIcon, Color.white);
            if (o.GetComponents<Light>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.lightIcon, Color.white);
            if (o.GetComponents<Animation>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.animationIcon, Color.white);
            if (o.GetComponents<Animator>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.animatorIcon, Color.white);
            if (o.GetComponents<PlayableDirector>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.directorIcon, Color.white);
            if (o.GetComponents<AudioSource>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.audioIcon, Color.white);
            if (o.GetComponents<VisualEffect>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.vfxIcon, Color.white);
            if (o.GetComponents<ParticleSystem>().Length > 0) selectionRect = DrawconContent(selectionRect, Contents.shurikenIcon, Color.white);

            GUI.color = c;
        }

        static Rect DrawLabel(Rect rect, string label, Color color, int size = 16)
        {
            GUI.color = color;
            GUI.Label(rect, label, Styles.rightLabel);
            rect.width = rect.width - size;
            return rect;
        }

        static Rect DrawconContent(Rect rect, GUIContent content, Color color, int size = 16)
        {
            GUI.color = color;
            GUI.Label(rect, content, Styles.icon);
            rect.width = rect.width - size;
            return rect;
        }

        static class Contents
        {
            public static GUIContent cameraIcon = EditorGUIUtility.IconContent("Camera Icon");
            public static GUIContent meshIcon = EditorGUIUtility.IconContent("MeshRenderer Icon");
            public static GUIContent colliderIcon = EditorGUIUtility.IconContent("BoxCollider Icon");
            public static GUIContent audioIcon = EditorGUIUtility.IconContent("AudioSource Icon");
            public static GUIContent animationIcon = EditorGUIUtility.IconContent("Animation Icon");
            public static GUIContent animatorIcon = EditorGUIUtility.IconContent("Animator Icon");
            public static GUIContent directorIcon = EditorGUIUtility.IconContent("PlayableDirector Icon");
            public static GUIContent monoBehaviourIcon = EditorGUIUtility.IconContent("cs Script Icon");
            public static GUIContent shurikenIcon = EditorGUIUtility.IconContent("ParticleSystem Icon");
            public static GUIContent vfxIcon = EditorGUIUtility.IconContent("VisualEffect Icon");
            public static GUIContent lightIcon = EditorGUIUtility.IconContent("Light Icon");
        }

        static class Colors
        {
            public static Color orange = new Color(1.0f, 0.7f, 0.1f);
            public static Color red = new Color(1.0f, 0.4f, 0.3f);
            public static Color yellow = new Color(0.8f, 1.0f, 0.1f);
            public static Color green = new Color(0.2f, 1.0f, 0.1f);
            public static Color blue = new Color(0.5f, 0.8f, 1.0f);
            public static Color violet = new Color(0.8f, 0.5f, 1.0f);
            public static Color purple = new Color(1.0f, 0.5f, 0.8f);
            public static Color dimGray = new Color(0.4f, 0.4f, 0.4f, 0.2f);
        }

        static class Styles
        {
            public static GUIStyle rightLabel;
            public static GUIStyle icon;

            static Styles()
            {
                rightLabel = new GUIStyle(EditorStyles.label);
                rightLabel.alignment = TextAnchor.MiddleRight;
                rightLabel.normal.textColor = Color.white;
                rightLabel.onNormal.textColor = Color.white;

                rightLabel.active.textColor = Color.white;
                rightLabel.onActive.textColor = Color.white;

                icon = new GUIStyle(rightLabel);
                icon.padding = new RectOffset();
                icon.margin = new RectOffset();


            }
        }

    }
}
