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

        Color GetTypeColor(CommentType type)
        {
            float sat = 0.5f;
            float v = 1f;
            switch (type)
            {

                default:
                case CommentType.Info:
                    return Color.HSVToRGB(0.1f, sat, v);
                case CommentType.Bug:
                    return Color.HSVToRGB(0.15f, sat, v);
                case CommentType.Request:
                    return Color.HSVToRGB(0.2f, sat, v);
                case CommentType.ToDo:
                    return Color.HSVToRGB(0.25f, sat, v);
            }
        }

        Color GetStateColor(CommentState state)
        {
            float sat = 0.2f;
            float v = 1f;
            switch (state)
            {
                default:
                case CommentState.Open:
                    return Color.HSVToRGB(0.9f, sat, v);
                case CommentState.Blocked:
                    return Color.HSVToRGB(0.85f, sat, v);
                case CommentState.Resolved:
                    return Color.HSVToRGB(0.8f, sat, v);
                case CommentState.WontFix:
                    return Color.HSVToRGB(0.75f, sat, v);
                case CommentState.Closed:
                    return Color.HSVToRGB(0.7f, sat, v);
            }
        }

        Color GetPriorityColor(CommentPriority priority)
        {
            float sat = 1f;
            float v = 1f;
            switch (priority)
            {
                default:
                case CommentPriority.High:
                    return Color.HSVToRGB(0.02f, sat, v);
                case CommentPriority.Medium:
                    return Color.HSVToRGB(0.15f, sat, v);
                case CommentPriority.Low:
                    return Color.HSVToRGB(0.3f, sat, v);
            }
        }

        void TypeLabel(CommentType value)
        {
            GUIUtils.ColoredLabel(value.ToString(), GetTypeColor(value));
        }

        void StateLabel(CommentState value)
        {
            GUIUtils.ColoredLabel(value.ToString(), GetStateColor(value));
        }
        void PriorityLabel(CommentPriority value)
        {
            GUIUtils.ColoredLabel(value.ToString(), GetPriorityColor(value));
        }

        public override void OnInspectorGUI()
        {
            using(new GUILayout.HorizontalScope())
            {
                TypeLabel((CommentType)m_Type.intValue);
                StateLabel((CommentState)m_State.intValue);
                PriorityLabel((CommentPriority)m_Priority.intValue);
                GUILayout.FlexibleSpace();
                edit = CommentEditor.DrawEditButton(edit);
            }

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
                    GUILayout.Label(m_Title.stringValue, Styles.title);
                    GUILayout.FlexibleSpace();
               }

                GUILayout.Space(8);
                GUILayout.Label(m_Body.stringValue, Styles.multiline);

            }

            EditorGUILayout.Space();
            if(GUILayout.Button("Open Comments", GUILayout.Height(24)))
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
