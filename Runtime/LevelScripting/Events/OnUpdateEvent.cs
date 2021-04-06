using GameplayIngredients.Managers;
using NaughtyAttributes;

namespace GameplayIngredients.Events
{
    public class OnUpdateEvent : EventBase
    {
        [EnableIf("AllowUpdateCalls")]
        public Callable[] OnUpdate;

        private void OnEnable()
        {
            if(AllowUpdateCalls())
                Manager.Get<SingleUpdateManager>().Register(SingleUpdate);
        }

        private void OnDisable()
        {
            if (AllowUpdateCalls())
                Manager.Get<SingleUpdateManager>().Remove(SingleUpdate);
        }

        private void SingleUpdate()
        {
            Callable.Call(OnUpdate, gameObject); 
        }
    }
}


