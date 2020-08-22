using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Comments
{
    public class SceneComment : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        Comment comment;
#endif
    }
}
