using UnityEngine;

namespace GameplayIngredients.Events
{
    [AddComponentMenu(ComponentMenu.eventsPath + "On Button Down Event")]
    public class OnButtonDownEvent : EventBase
    {
        public string Button = "Fire1";

        public Callable[] OnButtonDown;
        public Callable[] OnButtonUp;

        void Update()
        {
            if (Input.GetButtonDown(Button))
                Callable.Call(OnButtonDown, gameObject);

            if (Input.GetButtonUp(Button))
                Callable.Call(OnButtonUp, gameObject);
        }
    }
}


