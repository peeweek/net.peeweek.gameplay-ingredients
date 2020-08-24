using System.Security.Policy;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments.Editor
{
    public class CommentEditor
    {
        SerializedObject serializedObject;
        SerializedProperty comment;

        public CommentEditor(SerializedObject serializedObject, SerializedProperty comment, bool editRoot)
        {
            if(editRoot)
            {

            }

        }

        public static bool DrawEditButton(bool edit)
        {
            return GUILayout.Toggle(edit, edit ? "Close" : "Edit", EditorStyles.miniButton, GUILayout.Width(64));
        }

        public static void DrawComment(SerializedProperty comment)
        {

        }

        public static void DrawEditComment(SerializedProperty comment)
        {

        }
    }
}

