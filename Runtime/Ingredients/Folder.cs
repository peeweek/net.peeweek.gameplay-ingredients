using UnityEngine;

namespace GameplayIngredients
{
    [ExecuteAlways]
    public class Folder : MonoBehaviour
    {
        public Color Color = Color.yellow;

#if UNITY_EDITOR
        private void Awake()
        {
            Reset();
        }
        private void Reset()
        {
            gameObject.isStatic = true;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            if(Application.isPlaying)
            {
                Destroy(this);
            }
        }
#endif
    }
}

