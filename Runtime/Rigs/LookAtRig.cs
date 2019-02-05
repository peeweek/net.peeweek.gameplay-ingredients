using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Rigs
{
    public class LookAtRig : MonoBehaviour
    {
        public Transform LookAtTarget;
        public Vector3 UpVector = Vector3.up;

        void Update()
        {
            if (LookAtTarget != null)
                transform.LookAt(LookAtTarget, UpVector);
        }
    }
}
