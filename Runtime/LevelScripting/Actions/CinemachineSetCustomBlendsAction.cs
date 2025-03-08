using UnityEngine;
#if PACKAGE_CINEMACHINE_3
using Unity.Cinemachine;
#else
using Cinemachine;
#endif

namespace GameplayIngredients.Actions
{
    [Callable("Cinemachine", "Misc/ic-cinemachine.png")]
    [AddComponentMenu(ComponentMenu.cinemachinePath + "Cinemachine Set Custom Blends Action")]
    public class CinemachineSetCustomBlendsAction : ActionBase
    {
        public enum Action
        {
            Enable,
            Disable
        }

        [SerializeField]
        Action action;
        
        [SerializeField]
        CinemachineBlenderSettings settings;

        public override void Execute(GameObject instigator = null)
        {
            if(Manager.TryGet(out VirtualCameraManager vcm))
            {
                if (action == Action.Disable || settings == null)
                {
#if PACKAGE_CINEMACHINE_3
                    vcm.Brain.CustomBlends = null;
#else
                    vcm.Brain.m_CustomBlends = null;
#endif
                }
                else
                {
#if PACKAGE_CINEMACHINE_3
                    vcm.Brain.CustomBlends = settings;
#else
                    vcm.Brain.m_CustomBlends = settings;
#endif
                }
            }
        }

        public override string GetDefaultName() => $"{action} CM Custom Blends : {settings}";

    }
}

