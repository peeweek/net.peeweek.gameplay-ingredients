using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Actions
{
    public enum RigidbodyActionType { Force, Torque, ExplosionForce, Sleep };
    public enum ActionSpace { Local, World };

    [ExecuteAlways]
    public class RigidbodyAction : ActionBase
    {
        public Rigidbody m_Rigidbody;
        [OnValueChanged("OnParameterTypeChanged")]
        public RigidbodyActionType actionType;

        bool force = true;
        bool explosion;

        [ShowIf("force")]
        public ActionSpace actionSpace;
        [ShowIf("force")]
        public ForceMode forceMode;
        [ShowIf("force")]
        public Vector3 direction;
        [ShowIf("explosion")]
        public float explosionForce;
        [ShowIf("explosion")]
        public Vector3 explositonPosition;
        [ShowIf("explosion")]
        public float explosionRadius;

        public override void Execute(GameObject instigator = null)
        {
            if (m_Rigidbody == null)
                return;

            switch (actionType)
            {
                case RigidbodyActionType.Force:
                    if(actionSpace == ActionSpace.World)
                    {
                        m_Rigidbody.AddForce(direction, forceMode);
                    }
                    if(actionSpace == ActionSpace.Local)
                    {
                        m_Rigidbody.AddRelativeForce(direction, forceMode);
                    }
                    break;
                case RigidbodyActionType.Torque:
                    if (actionSpace == ActionSpace.World)
                    {
                        m_Rigidbody.AddTorque(direction, forceMode);
                    }
                    if (actionSpace == ActionSpace.Local)
                    {
                        m_Rigidbody.AddRelativeTorque(direction, forceMode);
                    }
                    break;
                case RigidbodyActionType.ExplosionForce:
                    m_Rigidbody.AddExplosionForce(explosionForce, explositonPosition, explosionRadius, 0, forceMode);
                    break;
                case RigidbodyActionType.Sleep:
                    m_Rigidbody.Sleep();
                    break;
            }
        }

        private void OnParameterTypeChanged()
        {
            force = false;
            explosion = (actionType == RigidbodyActionType.ExplosionForce);

            switch (actionType)
            {
                case RigidbodyActionType.Force:
                    force = true;
                    break;
                case RigidbodyActionType.Torque:
                    force = true;
                    break;
            }
        }
    }
}
