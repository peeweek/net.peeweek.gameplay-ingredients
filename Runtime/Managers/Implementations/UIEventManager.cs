using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
#endif

namespace GameplayIngredients
{
    [AddComponentMenu(ComponentMenu.managersPath + "UI Event Manager")]
    [RequireComponent(typeof(EventSystem))]
#if ENABLE_LEGACY_INPUT_MANAGER
    [RequireComponent(typeof(StandaloneInputModule))]
#endif
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(InputSystemUIInputModule))]
#endif

    [ManagerDefaultPrefab("UIEventManager")]
    public class UIEventManager : Manager
    {
        [SerializeField]
        private EventSystem m_EventSystem;

        private void OnEnable()
        {
            m_EventSystem = GetComponent<EventSystem>();

            if (TryGetComponent(out StandaloneInputModule im))
#if !ENABLE_LEGACY_INPUT_MANAGER
                im.enabled = false;
#else
                im.enabled = true;
#endif

#if ENABLE_INPUT_SYSTEM
            if (!TryGetComponent(out InputSystemUIInputModule ism))
                Debug.LogWarning("You are using the new Input System but the UI Event Manager prefab is not configured to use input from this package. In order to fix the issue, please add and configure a InputSystemUIInputModule component to your Assets/Resources/UIEventManagerPrefab. If the prefab is not present, use the Gameplay Ingredients Wizard located at Window/Gameplay Ingredients/Setup Wizard");
#endif

        }

        public void FocusUI(Selectable selectable)
        {
            // Before selecting, we ensure that there's no selection in the EventSystem
            m_EventSystem.SetSelectedGameObject(null);

            if(selectable != null)
                selectable.Select();
        }
    }
}