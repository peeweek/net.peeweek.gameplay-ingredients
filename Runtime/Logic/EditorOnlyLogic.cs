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

        [Tooltip("Executes in editor whe using Play From Here or not")]
        public bool EditorOnPlayHere = false;

        public Mode ExecutionPath;

        [ReorderableList]
        public Callable[] OnExecute;

        public override void Execute()
        {
            switch(ExecutionPath)
            {

                case Mode.EditorOnly:
                    if (Application.isEditor && EditorOnPlayHere == (PlayerPrefs.GetInt("PlayFromHere") == 1))
                        Callable.Call(OnExecute);
                    break;

                case Mode.PlayerOnly:
                    if (!Application.isEditor) Callable.Call(OnExecute);
                    break;
            }
        }
    }
}
