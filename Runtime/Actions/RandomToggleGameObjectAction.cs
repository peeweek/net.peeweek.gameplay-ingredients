using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Actions
{
    public class RandomToggleGameObjectAction : ActionBase
    {
        [ReorderableList]
        public GameObject[] Targets;

        public override void Execute(GameObject instigator = null)
        {
            var random = Targets[Random.Range(0,Targets.Length)];

            foreach (var target in Targets)
            {
                if (target != null)
                    target.SetActive(random == target);

            }
        }
    }
}
