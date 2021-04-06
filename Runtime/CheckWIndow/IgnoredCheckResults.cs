using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients
{
    public class IgnoredCheckResults : MonoBehaviour
    {
        [AddComponentMenu(ComponentMenu.basePath + "Ignored Check Results")]
        [System.Serializable]
        public struct IgnoredCheckResult
        {
            public string check;
            public GameObject gameObject;
        }

        public List<IgnoredCheckResult> ignoredCheckResults;

    }
}


