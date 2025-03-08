using UnityEngine;
#if PACKAGE_CINEMACHINE_3
using Unity.Cinemachine;
#else
using Cinemachine;
#endif
using NaughtyAttributes;

namespace GameplayIngredients.Actions
{
    [Callable("Cinemachine", "Misc/ic-cinemachine.png")]
    [AddComponentMenu(ComponentMenu.cinemachinePath + "Cinemachine Set Camera Noise Action")]

    public class CinemachineSetCameraNoiseAction : ActionBase
    {
        [SerializeField]
        bool useLiveCamera;
        [SerializeField, HideIf("useLiveCamera")]
#if PACKAGE_CINEMACHINE_3
        CinemachineCamera targetCamera;
#else
        CinemachineVirtualCamera targetCamera;
#endif

        [SerializeField]
        NoiseSettings settings;

        public override void Execute(GameObject instigator = null)
        {
#if PACKAGE_CINEMACHINE_3
            CinemachineCamera cam = useLiveCamera ?
                Manager.Get<VirtualCameraManager>().GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineCamera
                : targetCamera;
#else
            CinemachineVirtualCamera cam = useLiveCamera ?
                Manager.Get<VirtualCameraManager>().GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera
                : targetCamera;
#endif
            if(cam == null)
            {
                Debug.Log("CinemachineSetCameraNoiseAction : Cannot find a suitable CinemachineCamera to set Noise to");
                return;
            }

#if PACKAGE_CINEMACHINE_3
            var noise = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();

            if (noise == null && settings != null)
                cam.gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();

            noise.NoiseProfile = settings;
#else
            var noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if(noise == null && settings != null)
                noise = cam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            noise.m_NoiseProfile = settings;
#endif

        }

        public override string GetDefaultName() => $"CM Set Noise ({settings.name}) for {(useLiveCamera? "Live Camera" : targetCamera?.gameObject.name)}";

    }

}

