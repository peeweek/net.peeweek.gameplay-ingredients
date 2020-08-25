using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{
    [CustomEditor(typeof(SceneComment))]
    public class SceneCommentEditor : UnityEditor.Editor
    {
        [SerializeField]
        SceneComment sceneComment;
        [SerializeField]
        SerializedProperty m_Comment;

        [SerializeField]
        CommentEditor m_CommentEditor;

        private void OnEnable()
        {
            UpdateComment();

            if (sceneComment.UsePOV)
                SceneView.lastActiveSceneView.AlignViewToObject(sceneComment.transform);
        }

        void UpdateComment()
        {
            if (m_Comment == null)
                m_Comment = serializedObject.FindProperty("m_Comment");

            if (m_CommentEditor == null)
                m_CommentEditor = new CommentEditor(serializedObject, m_Comment);

            sceneComment = (serializedObject.targetObject as SceneComment);
        }

        public override void OnInspectorGUI()
        {
            UpdateComment();

            if (GUILayout.Button("Open Comments", GUILayout.Height(24)))
                CommentsWindow.Open();

            GUILayout.Space(4);
            m_CommentEditor.DrawComment();
            GUILayout.Space(16);
        }
    }
}
