using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace GameplayIngredients.Logic
{
    public class EditorOnlyLogic : LogicBase
    {
        public enum Mode
        {
            EditorOnly,
            PlayerOnly
        }

        public Mode ExecutionPath;

        [ReorderableList]
        public Callable[] OnExecute;

        public override void Execute()
        {
            switch(ExecutionPath)
            {
                case Mode.EditorOnly:
                    if (Application.isEditor) Callable.Call(OnExecute);
                    break;
                case Mode.PlayerOnly:
                    if (!Application.isEditor) Callable.Call(OnExecute);
                    break;
            }
        }
    }
}
