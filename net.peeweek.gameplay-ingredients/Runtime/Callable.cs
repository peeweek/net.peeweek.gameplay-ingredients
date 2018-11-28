using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients
{
    public abstract class Callable : MonoBehaviour, ICallable
    {
        public string Name;

        public Callable()
        {
            Name = GetType().Name;
        }

        public abstract void Execute();
        public abstract new string ToString();

        public static void Call(Callable[] calls)
        {
            foreach (var call in calls)
                call.Execute();
        }

        public static void Call(Callable call)
        {
            call.Execute();
        }
    }
}

