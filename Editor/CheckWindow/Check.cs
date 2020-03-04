using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Editor
{

    public abstract class Check
    {
        public abstract IEnumerable<CheckResult<Check>> GetResults();
        public abstract void Resolve();
    }

}
