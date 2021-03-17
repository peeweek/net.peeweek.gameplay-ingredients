using NaughtyAttributes;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameplayIngredients.Logic
{
    public class PlatformLogic : LogicBase
    {
        public enum InclusionMode
        {
            IsTarget,
            IsNotTarget
        }

        [ReorderableList]
        public RuntimePlatform[] platforms;

        public InclusionMode inclusionMode = InclusionMode.IsTarget;

        [ReorderableList, FormerlySerializedAs("Calls")]
        public Callable[] OnTestValid;

        [ReorderableList]
        public Callable[] OnTestInvalid;


        public override void Execute(GameObject instigator = null)
        {

            if(platforms.Contains(Application.platform) == (inclusionMode == InclusionMode.IsTarget))
                Call(OnTestValid, instigator);
            else
                Call(OnTestInvalid, instigator);
        }

        public override string GetDefaultName()
        {
            return $"If Platform {inclusionMode} : {string.Join(", ",platforms)}";
        }
    }
}

