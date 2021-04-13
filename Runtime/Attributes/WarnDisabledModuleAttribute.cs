using System;

namespace GameplayIngredients
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WarnDisabledModuleAttribute : Attribute
    {
        public string module;

        public WarnDisabledModuleAttribute(string module)
        {
            this.module = module;
        }
    }
}


