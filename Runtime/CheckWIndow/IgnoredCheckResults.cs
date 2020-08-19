using System.Collections.Generic;
using UnityEngine;

public class IgnoredCheckResults : MonoBehaviour
{
    [System.Serializable]
    public struct IgnoredCheckResult
    {
        public string check;
        public GameObject gameObject;
    }

    public List<IgnoredCheckResult> ignoredCheckResults;
    
}
