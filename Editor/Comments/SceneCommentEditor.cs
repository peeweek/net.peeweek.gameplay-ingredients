using System.Security.Policy;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{
    [CustomEditor(typeof(SceneComment))]
    public class SceneCommentEditor : UnityEditor.Editor
    {
        bool edit;
        SceneComment sceneComment;

        SerializedProperty m_Comment;
        SerializedProperty m_UsePOV;
        SerializedProperty m_Message;
        SerializedProperty m_Type;
        SerializedProperty m_Priority;
        SerializedProperty m_State;
        SerializedProperty m_Title;
        SerializedProperty m_Body;
        private void OnEnable()
        {
            sceneComment = (serializedObject.targetObject as SceneComment);
            if (sceneComment.UsePOV)
                SceneView.lastActiveSceneView.AlignViewToObject(sceneComment.transform);
            edit = false;

            m_Comment = serializedObject.FindProperty("m_Comment");
            m_UsePOV = serializedObject.FindProperty("UsePOV");
            m_Message = m_Comment.FindPropertyRelative("message");
            m_Type = m_Comment.FindPropertyRelative("type");
            m_Priority = m_Comment.FindPropertyRelative("priority");
            m_State = m_Comment.FindPropertyRelative("state");
            m_Title = m_Message.FindPropertyRelative("title");
            m_Body = m_Message.FindPropertyRelative("body");
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(4);

            using (new GUILayout.HorizontalScope())
            {
                CommentEditor.TypeLabel((CommentType)m_Type.intValue);
                CommentEditor.StateLabel((CommentState)m_State.intValue);
                CommentEditor.PriorityLabel((CommentPriority)m_Priority.intValue);
                GUILayout.FlexibleSpace();
                edit = CommentEditor.DrawEditButton(edit);
            }

            GUILayout.Space(6);

            if(edit)
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(m_Title);
                EditorGUILayout.PropertyField(m_Body);
                EditorGUILayout.PropertyField(m_Type);
                EditorGUILayout.PropertyField(m_State);
                EditorGUILayout.PropertyField(m_Priority);
                EditorGUILayout.PropertyField(m_UsePOV);

                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                using(new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(CommentEditor.GetPriorityContent(" "+ m_Title.stringValue, (CommentPriority)(m_Priority.intValue)), Styles.title);
                    GUILayout.FlexibleSpace();
               }

                GUILayout.Space(8);
                GUILayout.Label(m_Body.stringValue, Styles.multiline);

            }

            GUILayout.Space(16);
            if (GUILayout.Button("Open Comments", GUILayout.Height(24)))
                CommentsWindow.Open();

        }

        static class Styles
        {
            public static GUIStyle title;
            public static GUIStyle multiline;
            public static GUIStyle coloredLabel;
            static Styles()
            {
                title = new GUIStyle(EditorStyles.boldLabel);
                title.fontSize = 16;

                multiline = new GUIStyle(EditorStyles.label);
                multiline.wordWrap = true;
                multiline.fontSize = 14;

                coloredLabel = new GUIStyle(EditorStyles.label);
                coloredLabel.fontSize = 12;
                coloredLabel.padding = new RectOffset(12, 12, 2, 2);

            }
        }
    }
}
