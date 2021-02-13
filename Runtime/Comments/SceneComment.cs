using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameplayIngredients.Comments
{
    public class SceneComment : MonoBehaviour
    {
#if UNITY_EDITOR
        const string kUserPreference = "GameplayIngredients.Comments.User";
        public Comment comment => m_Comment;
        [SerializeField]
        Comment m_Comment;
        private void Reset()
        {
            m_Comment.message.from = EditorPrefs.GetString(kUserPreference, "user");
            transform.hideFlags = HideFlags.HideInInspector;
        }

        public void SetDefault()
        {
            m_Comment.title = "New Comment";
            m_Comment.message.body = "This is a new Comment, it can describe a problem in the scene, a note to the attention of other user, or a bug encountered.";
            m_Comment.message.from = EditorPrefs.GetString(kUserPreference, "user");
        }

#endif
    }
}
