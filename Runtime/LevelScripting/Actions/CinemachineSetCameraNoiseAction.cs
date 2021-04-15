using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

namespace GameplayIngredients.Actions
{
    [Callable("Cinemachine", "Misc/ic-cinemachine.png")]
    public class CinemachineSetCameraNoiseAction : ActionBase
    {
        [SerializeField]
        bool useLiveCamera;
        [SerializeField, HideIf("useLiveCamera")]
        CinemachineVirtualCameraBase targetCamera;

        public override void Execute(GameObject instigator = null)
        {
            CinemachineVirtualCameraBase cam = useLiveCamera ?
                Manager.Get<VirtualCameraManager>().GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCameraBase
                : targetCamera;

            if(cam == null)
            {
                Debug.Log("CinemachineSetCameraNoiseAction : Cannot find a suitable Camera to set noise");
                return;
            }


        }
    }

}

