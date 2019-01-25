using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEditor;
using GameplayIngredients;

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
            GameObject o = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (o == null) return;

            var c = GUI.color;
            if (o.isStatic) selectionRect = DrawLabel(selectionRect, "#", Color.gray);
            if (o.GetComponents<MonoBehaviour>().Length > 0) selectionRect = DrawLabel(selectionRect, "*", Colors.green);
            if (o.GetComponents<Camera>().Length > 0) selectionRect = DrawLabel(selectionRect, "C", Colors.blue);
            if (o.GetComponents<Light>().Length > 0) selectionRect = DrawLabel(selectionRect, "L", Colors.yellow);
            if (o.GetComponents<MeshRenderer>().Length > 0) selectionRect = DrawLabel(selectionRect, "M", Colors.purple);
            if (o.GetComponents<AudioSource>().Length > 0) selectionRect = DrawLabel(selectionRect, "S", Colors.orange);
            if (o.GetComponents<VisualEffect>().Length > 0) selectionRect = DrawLabel(selectionRect, "fx", Colors.red, 16);
            GUI.color = c;
        }

        static Rect DrawLabel(Rect rect, string label, Color color, int size = 12)
        {
            GUI.color = color;
            GUI.Label(rect, label, Styles.rightLabel);
            rect.width = rect.width - size;
            return rect;
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
        }

        static class Styles
        {
            public static GUIStyle rightLabel;
            static Styles()
            {
                rightLabel = new GUIStyle(EditorStyles.label);
                rightLabel.alignment = TextAnchor.MiddleRight;
                rightLabel.normal.textColor = Color.white;
                rightLabel.onNormal.textColor = Color.white;

                rightLabel.active.textColor = Color.white;
                rightLabel.onActive.textColor = Color.white;

            }
        }

    }
}
