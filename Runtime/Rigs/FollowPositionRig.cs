using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositionRig : MonoBehaviour
{
    public Transform target => m_Target;

    [SerializeField]
    protected Transform m_Target;
    public float Dampen = 1.0f;
    public float MaximumVelocity = 1.0f;

    void Update()
    {
        if(m_Target != null)
        {
            var transform = gameObject.transform;
            var delta = m_Target.position - transform.position;
            var speed = Time.deltaTime * Mathf.Min((Dampen * delta.magnitude), MaximumVelocity);
            gameObject.transform.position += delta.normalized * speed;
        }
    }

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

}
