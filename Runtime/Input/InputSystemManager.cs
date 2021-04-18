using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GameplayIngredients.Managers
{
#if !ENABLE_INPUT_SYSTEM
    [DoNotCreateManager]
    [WarnDisabledModule("New Input System","Player Settings")]
#endif
    [AddComponentMenu(ComponentMenu.managersPath + "Input System Manager (New Input System)")]
    [ManagerDefaultPrefab("InputSystemManager")]
    public class InputSystemManager : Manager
    {
#if ENABLE_INPUT_SYSTEM

        public InputActionDefinition[] inputActions { get => m_InputActions; }

        [SerializeField]
        InputActionDefinition[] m_InputActions;

        [Serializable]
        public struct InputActionDefinition
        {
            public InputActionAsset inputActionAsset;
            public bool createUIActions;

        }
#endif
    }
}


