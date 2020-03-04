using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Editor
{
    public class CheckResult<T> where T:Check
    {
        public enum Result
        {
            Pass,
            Warning,
            Failed
        }

        T check;
        Result result;
        Object[] objects;
        

        public CheckResult(T check, Result result, Object[] objects)
        {
            this.check = check;
            this.result = result;
            this.objects = objects;
        }
    }
}
