using System.Security.Policy;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{
    [CustomEditor(typeof(SceneComment))]
    public class SceneCommentsEditor : UnityEditor.Editor
    {
        bool edit;
        SceneComment sceneComment;

        SerializedProperty m_Comment;
        SerializedProperty m_Message;
        SerializedProperty m_Type;
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
            m_Message = m_Comment.FindPropertyRelative("message");
            m_Type = m_Comment.FindPropertyRelative("type");
            m_State = m_Comment.FindPropertyRelative("state");
            m_Title = m_Message.FindPropertyRelative("title");
            m_Body = m_Message.FindPropertyRelative("body");
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
                serializedObject.Update();
                EditorGUILayout.PropertyField(m_Title);
                EditorGUILayout.PropertyField(m_Body);
                EditorGUILayout.PropertyField(m_Type);
                EditorGUILayout.PropertyField(m_State);

                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                using(new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(m_Title.stringValue, Styles.title);
                    GUILayout.FlexibleSpace(); 
                    GUILayout.Label(((CommentType)m_Type.intValue).ToString());
                    GUILayout.Label(((CommentState)m_State.intValue).ToString());
                }

                GUILayout.Space(8);
                GUILayout.Label(m_Body.stringValue, EditorStyles.textArea);

            }

            EditorGUILayout.Space();
            if(GUILayout.Button("Open Comments", GUILayout.Height(24)))
                CommentsWindow.Open();

        }

        static class Styles
        {
            public static GUIStyle title;

            static Styles()
            {
                title = new GUIStyle(EditorStyles.boldLabel);
                title.fontSize = 16;
            }
        }
    }
}
