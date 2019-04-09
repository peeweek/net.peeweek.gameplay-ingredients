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

        public abstract void Execute(GameObject instigator = null);
        public abstract new string ToString();

        public static void Call(Callable[] calls, GameObject instigator = null)
        {
            foreach (var call in calls)
            {
                if (Debug.isDebugBuild || Application.isEditor)
                    Debug.Log($"[CALL] : {call.gameObject.scene.name} : {call.gameObject.name} :> {call.GetType().Name} ({call.Name})");

                if(call != null)
                    call.Execute(instigator);
                else
                    Debug.LogError("Cannot execute call: Null or Missing");
            }
        }

        public static void Call(Callable call, GameObject instigator = null)
        {
            if (call != null)
                call.Execute(instigator);
            else
                Debug.LogError("Cannot execute call: Null or Missing");
        }
    }
}

