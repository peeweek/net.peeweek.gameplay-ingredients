using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Comments
{
    public class SceneComment : MonoBehaviour
    {

#if UNITY_EDITOR
        public bool UsePOV;

        public Comment comment => m_Comment;
        [SerializeField]
        Comment m_Comment;
#endif
    }
}
