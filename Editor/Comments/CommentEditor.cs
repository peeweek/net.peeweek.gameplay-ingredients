using GameplayIngredients.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace GameplayIngredients.Comments.Editor
{
    public class CommentEditor
    {
        SerializedObject serializedObject;
        SerializedProperty rootMessage;
        SerializedProperty title;
        SerializedProperty type;
        SerializedProperty state;
        SerializedProperty priority;
        SerializedProperty editMessage;

        bool edit => editMessage == rootMessage;

        public CommentEditor(SerializedObject serializedObject, SerializedProperty comment)
        {
            this.serializedObject = serializedObject;
            title = comment.FindPropertyRelative("title");
            type = comment.FindPropertyRelative("type");
            state = comment.FindPropertyRelative("state");
            priority = comment.FindPropertyRelative("priority");
            rootMessage = comment.FindPropertyRelative("message");
        }

        public bool DrawEditButton(bool edit)
        {
            return GUILayout.Toggle(edit, edit ? "Close" : "Edit", EditorStyles.miniButton, GUILayout.Width(64));
        }

        public void DrawComment()
        {
            using (new GUILayout.HorizontalScope())
            {
                TypeLabel((CommentType)type.intValue);
                StateLabel((CommentState)state.intValue);
                PriorityLabel((CommentPriority)priority.intValue);

                GUILayout.FlexibleSpace();
                EditorGUI.BeginChangeCheck();
                bool editRoot = DrawEditButton(edit);
                if(EditorGUI.EndChangeCheck())
                {
                    if (editRoot)
                        editMessage = rootMessage;
                    else
                        editMessage = null;
                }
            }

            GUILayout.Space(6);

            if(edit)
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(title);
                EditorGUILayout.PropertyField(type);
                EditorGUILayout.PropertyField(state);
                EditorGUILayout.PropertyField(priority);
                serializedObject.ApplyModifiedProperties();
            }

            GUILayout.Space(6);

            DrawMessage(rootMessage);
        }

        void DrawMessage(SerializedProperty message)
        {
            SerializedProperty body = message.FindPropertyRelative("body");
            SerializedProperty URL = message.FindPropertyRelative("URL");
            SerializedProperty from = message.FindPropertyRelative("from");
            SerializedProperty attn = message.FindPropertyRelative("attn");
            SerializedProperty objects = message.FindPropertyRelative("targets");
            SerializedProperty replies = message.FindPropertyRelative("replies");

            if (editMessage == message)
            {
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();


                EditorGUILayout.PropertyField(body);
                EditorGUILayout.PropertyField(URL);
                EditorGUILayout.PropertyField(from);
                EditorGUILayout.PropertyField(attn);
                EditorGUILayout.PropertyField(objects);


                if(EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
            }
            else
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(CommentEditor.GetPriorityContent(" " + title.stringValue, (CommentPriority)(priority.intValue)), Styles.title);
                    GUILayout.FlexibleSpace();
                }

                GUILayout.Space(8);

                GUILayout.Label(body.stringValue, Styles.multiline);

                GUILayout.Space(8);
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Reply", GUILayout.Width(64)))
                    {
                        replies.InsertArrayElementAtIndex(0);
                        editMessage = replies.GetArrayElementAtIndex(0);
                    }
                }


            }

            int replyCount = replies.arraySize; 

            if(replyCount > 0)
            {
                EditorGUI.indentLevel++;
                GUILayout.Label("Replies", EditorStyles.foldoutHeader);
                for (int i = 0; i < replyCount; i++)
                {
                    DrawMessage(replies.GetArrayElementAtIndex(0));
                }
                EditorGUI.indentLevel--;
            }
        }

        public static Color GetTypeColor(CommentType type)
        {
            float sat = 0.3f;
            float v = 1f;
            switch (type)
            {

                default:
                case CommentType.Info:
                    return Color.HSVToRGB(0.4f, 0, v);
                case CommentType.Bug:
                    return Color.HSVToRGB(0.05f, sat, v);
                case CommentType.Request:
                    return Color.HSVToRGB(0.15f, sat, v);
                case CommentType.ToDo:
                    return Color.HSVToRGB(0.25f, sat, v);
            }
        }

        public static Color GetStateColor(CommentState state)
        {
            float sat = 0.8f;
            float v = 1f;
            switch (state)
            {
                default:
                case CommentState.Open:
                    return Color.HSVToRGB(0.3f, sat, v);
                case CommentState.Blocked:
                    return Color.HSVToRGB(0.05f, sat, v);
                case CommentState.Resolved:
                    return Color.HSVToRGB(0.5f, sat, v);
                case CommentState.WontFix:
                    return Color.HSVToRGB(0.05f, sat, v);
                case CommentState.Closed:
                    return Color.HSVToRGB(0.7f, 0f, v);
            }
        }

        public static Color GetPriorityColor(CommentPriority priority)
        {
            float sat = 0.9f;
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

        public static void TypeLabel(CommentType value)
        {
            GUIUtils.ColoredLabel(value.ToString(), GetTypeColor(value));
        }

        public static void StateLabel(CommentState value)
        {
            GUIUtils.ColoredLabel(value.ToString(), GetStateColor(value));
        }

        public static void PriorityLabel(CommentPriority value)
        {
            GUIUtils.ColoredLabel(value.ToString(), GetPriorityColor(value));
        }

        public static GUIContent GetPriorityContent(string text, CommentPriority priority)
        {
            return new GUIContent(text, GetPriorityTexture(priority));
        }

        public static Texture GetPriorityTexture(CommentPriority priority)
        {
            switch (priority)
            {
                case CommentPriority.High:
                    return CheckResult.GetIcon(CheckResult.Result.Failed);
                case CommentPriority.Medium:
                    return CheckResult.GetIcon(CheckResult.Result.Warning);
                default:
                case CommentPriority.Low:
                    return CheckResult.GetIcon(CheckResult.Result.Notice);
            }
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

