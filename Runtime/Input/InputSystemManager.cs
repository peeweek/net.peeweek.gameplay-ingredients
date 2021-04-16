using UnityEngine;

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
        //public InputActionAsset
    }

}


