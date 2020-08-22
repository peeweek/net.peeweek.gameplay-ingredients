using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{
    [CustomEditor(typeof(SceneComment))]
    public class SceneCommentsEditor : UnityEditor.Editor
    {
        bool edit;

        private void OnEnable()
        {
            SceneView.lastActiveSceneView.AlignViewToObject((serializedObject.targetObject as SceneComment).transform);
            edit = false;
        }

        public override void OnInspectorGUI()
        {
            using(new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                edit = GUILayout.Toggle(edit, edit ? "Close" : "Edit", EditorStyles.miniButton, GUILayout.Width(64) );
            }
            if(edit)
            {
                base.OnInspectorGUI();
            }
            EditorGUILayout.Space();
            if(GUILayout.Button("Open Comments", GUILayout.Height(24)))
                CommentsWindow.Open();

        }
    }
}
