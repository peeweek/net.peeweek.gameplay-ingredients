using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GameplayIngredients
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineBrain))]
    [ManagerDefaultPrefab("VirtualCameraManager")]
    public class VirtualCameraManager : Manager
    {
        Camera m_Camera;
        CinemachineBrain m_Brain;

        private void Awake()
        {
            m_Camera = GetComponent<Camera>();
            m_Brain = GetComponent<CinemachineBrain>();
        }

        public void BlendToCamera(CinemachineVirtualCamera camera, float delay)
        {
            
        }

    }
}
