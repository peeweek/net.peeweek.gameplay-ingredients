using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Rigs
{
    public class ReachPositionRig : MonoBehaviour
    {
        public Transform target => m_Target;

        [Header("Target")]
        [SerializeField]
        [InfoBox("Target needs to have the same parent as the current game object", EInfoBoxType.Warning)]
        protected Transform m_Target;
        [Header("Motion")]
        public float Dampen = 1.0f;
        public float MaximumVelocity = 1.0f;
        public bool inLocalSpace = false; 

        [Header("On Reach Position")]
        [ReorderableList]
        public Callable[] OnReachPosition;
        public float ReachSnapDistance = 0.01f;

        bool m_PositionReached = false;

        bool warnLocalParent()
        {
            return m_Target != null && m_Target.transform.parent != transform.parent;
        }

        void LateUpdate()
        {
            if(m_Target != null)
            {
                var transform = gameObject.transform;

                Vector3 position = inLocalSpace ? transform.localPosition : transform.position;
                Vector3 targetPosition = inLocalSpace ? m_Target.localPosition : m_Target.position;

                if (Vector3.Distance(position, targetPosition) < ReachSnapDistance)
                {
                    if(inLocalSpace)
                        transform.localPosition = targetPosition;
                    else
                        transform.position = targetPosition;

                    if(!m_PositionReached)
                    {
                        Callable.Call(OnReachPosition, this.gameObject);
                        m_PositionReached = true;
                    }
                }
                else
                {
                    var delta = m_Target.position - transform.position;
                    var speed = Time.deltaTime * Mathf.Min((Dampen * delta.magnitude), MaximumVelocity);
                    gameObject.transform.position += delta.normalized * speed;
                    m_PositionReached = false;
                }
            }
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

    }
}
