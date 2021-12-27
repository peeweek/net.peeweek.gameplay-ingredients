using System.Collections.Generic;
using System;

namespace GameplayIngredients
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ManagerDependsOnAttribute : Attribute
    {
        public Type[] dependantTypes { get; private set; }

        public ManagerDependsOnAttribute(params Type[] dependencies)
        {
            var dt = new List<Type>();

            foreach(var type in dependencies)
            {
                // Only add if this is a manager
                if(type != null && typeof(Manager).IsAssignableFrom(type))
                {
                    dt.Add(type);
                }
            }

            dependantTypes = dt.ToArray();
        }
    }
}

