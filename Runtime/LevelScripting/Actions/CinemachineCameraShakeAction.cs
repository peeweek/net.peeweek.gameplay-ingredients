using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

namespace GameplayIngredients.Actions
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    [Callable("Cinemachine", "Misc/ic-cinemachine.png")]
    public class CinemachineCameraShakeAction : ActionBase
    {
        public enum ImpulseLocation
        {
            SelfTransform,
            RemoteTransform,
            InstigatorPosition
        }

        [InfoBox("Remember to add Cinemachine Impulse listeners extensions on Virtual Cameras you want to recieve these camera shakes.")]
        [Tooltip("Which transform to use for the source of the camera shake")]
        public ImpulseLocation impulseLocation = ImpulseLocation.SelfTransform;
        [ShowIf("useRemoteTransform")]
        [Tooltip("The Remote transform to use to generate impulses")]
        public Transform remoteImpulseLocation;
        bool useRemoteTransform => impulseLocation == ImpulseLocation.RemoteTransform;

        [Header("Impulse")]
        [Tooltip("Whether the Impulse Vector will be computed in local space (or world space)")]
        public bool LocalSpace = true;
        [Tooltip("The Impulse Vector to Use")]
        public Vector3 baseImpulse = Vector3.up;
        [Tooltip("Whether to apply randomness as well")]
        public bool randomImpulse = false;
        [Tooltip("The Random Variation scale of the Impulse")]
        [ShowIf("randomImpulse")]
        public Vector3 variation = Vector3.one;
        [Tooltip("Whether to normalize the Base+Random Impulse")]
        [ShowIf("randomImpulse")]
        public bool normalize = false;
        [Tooltip("A random rescale of the impulse vector, after normalization")]
        [ShowIf(EConditionOperator.And, "randomImpulse", "normalize")]
        public Vector2 postNormalizeRemap = Vector2.one;


        CinemachineImpulseSource m_ImpulseSource;


        public override void Execute(GameObject instigator = null)
        {
            if(m_ImpulseSource == null)
            {
                m_ImpulseSource = GetComponent<CinemachineImpulseSource>();
                if (m_ImpulseSource == null)
                    m_ImpulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
            }

            Vector3 impulse = baseImpulse;
            if(randomImpulse)
            {
                impulse += new Vector3(Random.Range(-variation.x / 2, variation.x / 2), Random.Range(-variation.y / 2, variation.y / 2), Random.Range(-variation.z / 2, variation.z / 2));
                if(normalize)
                {
                    impulse.Normalize();
                    impulse *= Random.Range(postNormalizeRemap.x, postNormalizeRemap.y);
                }
            }

            switch (impulseLocation)
            {
                default:
                case ImpulseLocation.SelfTransform:
                    if (LocalSpace)
                        impulse = transform.localToWorldMatrix.MultiplyVector(impulse);

                    m_ImpulseSource.GenerateImpulse(impulse);
                    break;
                case ImpulseLocation.RemoteTransform:
                    if(remoteImpulseLocation != null)
                    {
                        if (LocalSpace)
                            impulse = remoteImpulseLocation.localToWorldMatrix.MultiplyVector(impulse);

                        m_ImpulseSource.GenerateImpulseAt(remoteImpulseLocation.position, impulse);
                    }
                    else
                    {
                        Debug.LogWarning("CinemachineCameraShakeAction : No RemoteTransform found for setting position, using self transform instead");
                        if (LocalSpace)
                            impulse = transform.localToWorldMatrix.MultiplyVector(impulse);
                        m_ImpulseSource.GenerateImpulse(impulse);
                    }
                    break;
                case ImpulseLocation.InstigatorPosition:
                    if(instigator != null)
                    {
                        if (LocalSpace)
                            impulse = instigator.transform.localToWorldMatrix.MultiplyVector(impulse);

                        m_ImpulseSource.GenerateImpulseAt(instigator.transform.position, impulse);
                    }
                    else
                    {
                        Debug.LogWarning("CinemachineCameraShakeAction : No Instigator found for setting position, using self transform instead");
                        if (LocalSpace)
                            impulse = transform.localToWorldMatrix.MultiplyVector(impulse);
                        m_ImpulseSource.GenerateImpulse(impulse);
                    }
                    break;
            }
                
        }
    }

}

