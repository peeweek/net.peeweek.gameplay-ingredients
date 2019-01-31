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

        [Tooltip("Disables when using Play From Here")]
        public bool DisableOnPlayFromHere = false;

        public Mode ExecutionPath;

        [ReorderableList]
        public Callable[] OnExecute;

        public override void Execute(GameObject instigator = null)
        {
            switch(ExecutionPath)
            {

                case Mode.EditorOnly:
                    if (Application.isEditor && !(DisableOnPlayFromHere && (PlayerPrefs.GetInt("PlayFromHere") == 1)))
                        Callable.Call(OnExecute);
                    break;

                case Mode.PlayerOnly:
                    if (!Application.isEditor) Callable.Call(OnExecute);
                    break;
            }
        }
    }
}
