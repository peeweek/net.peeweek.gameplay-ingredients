using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{
    [CustomEditor(typeof(ProjectComments))]
    public class ProjectCommentsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(@"In order to edit Project Comments, use the Comments window.", MessageType.None);
            if(GUILayout.Button("Open Comments", GUILayout.Height(24)))
            {
                // Open Window
            }
        }
    }
}
