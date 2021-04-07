using UnityEngine;

namespace GameplayIngredients.Events
{
    [AddComponentMenu(ComponentMenu.eventsPath + "On Key Down Event")]
    public class OnKeyDownEvent : EventBase
    {
        public KeyCode Key = KeyCode.F5;

        public Callable[] OnKeyDown;
        public Callable[] OnKeyUp;

        void Update()
        {
            if (Input.GetKeyDown(Key))
                Callable.Call(OnKeyDown, gameObject);

            if (Input.GetKeyUp(Key))
                Callable.Call(OnKeyUp, gameObject);
        }
    }
}


