using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Editor
{
    public class EmptyGameObjectCheck : Check
    {
        public override string name => "EmptyGameObject";

        public override bool defaultEnabled => true;

        public override string[] ResolutionActions => new string[]{"Do Nothing", "Set Static"};

        public override int defaultResolutionActionIndex => 0;

        public override IEnumerable<CheckResult<Check>> GetResults()
        {
            GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
            foreach (var go in all)
            {
                var allComps = go.GetComponents<Component>();
                if (allComps.Length == 1)
                {
                    if (!go.isStatic)
                        yield return new CheckResult<Check>(this, CheckResult<Check>.Result.Failed, $"Empty Game Object {go.name} is not static", go);
                }

            }
        }

        public override void Resolve(int action)
        {
            throw new System.NotImplementedException();
        }
    }
}

