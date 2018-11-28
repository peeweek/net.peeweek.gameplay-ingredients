using NaughtyAttributes;

namespace GameplayIngredients.Hooks
{
    public class OnStartHook : HookBase
    {
        [ReorderableList]
        public Callable[] OnStart;

        private void Start()
        {
            Callable.Call(OnStart); 
        }
    }
}


