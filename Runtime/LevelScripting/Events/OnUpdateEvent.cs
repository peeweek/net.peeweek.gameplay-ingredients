using NaughtyAttributes;

namespace GameplayIngredients.Events
{
    public class OnUpdateEvent : EventBase
    {
        [ReorderableList, ShowIf("AllowUpdateCalls")]
        public Callable[] OnUpdate;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Update()
        {
            Callable.Call(OnUpdate, gameObject); 
        }
    }
}


