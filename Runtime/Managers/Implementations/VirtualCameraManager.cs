using UnityEngine;
#if PACKAGE_CINEMACHINE_3
using Unity.Cinemachine;
#else
using Cinemachine;
#endif

namespace GameplayIngredients
{
    [AddComponentMenu(ComponentMenu.managersPath + "Virtual Camera Manager")]
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineBrain))]
    [ManagerDefaultPrefab("VirtualCameraManager")]
    public class VirtualCameraManager : Manager
    {
        public Camera Camera { get; private set; }
        public CinemachineBrain Brain { get; private set; }

        private void Awake()
        {
            Camera = GetComponent<Camera>();
            Brain = GetComponent<CinemachineBrain>();
        }

    }
}
