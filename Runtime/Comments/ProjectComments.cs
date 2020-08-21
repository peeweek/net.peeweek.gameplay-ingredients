using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Comments
{
    [CreateAssetMenu(fileName ="New Project Comments", menuName ="Project Comments")]
    public class ProjectComments : ScriptableObject
    {
        public List<Comment> comments;
    }
}
