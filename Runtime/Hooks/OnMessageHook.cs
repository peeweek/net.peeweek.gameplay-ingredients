using NaughtyAttributes;

namespace GameplayIngredients.Hooks
{
    public class OnMessageHook : HookBase
    {
        public string MessageName = "Message";

        [ReorderableList]
        public Callable[] OnMessageRecieved;

        void OnEnable()
        {
            Messager.RegisterEvent(MessageName, Execute);
        }

        void OnDisable()
        {
            Messager.UnregisterEvent(MessageName, Execute);
        }

        void Execute()
        {
            Callable.Call(OnMessageRecieved, gameObject);
        }


    }
}
