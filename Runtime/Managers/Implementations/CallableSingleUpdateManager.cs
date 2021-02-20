using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameplayIngredients.Managers
{
    [NonExcludeableManager]
    public class CallableSingleUpdateManager : Manager
    {
        class Call
        {
            public Callable[] calls;
            public GameObject instigator;
            public Call(Callable[] calls, GameObject instigator = null)
            {
                this.calls = calls;
                this.instigator = instigator;
            }
        }

        List<Call> updateCalls;

        List<Call> pendingAdd;
        List<Call> pendingRemove;

        private void OnEnable()
        {
            updateCalls = new List<Call>();
            pendingAdd = new List<Call>();
            pendingRemove = new List<Call>();
        }

        public void RegisterCalls(Callable[] calls, GameObject instigator = null)
        {
            if (!updateCalls.Any(o => o.calls == calls && o.instigator == instigator) && !pendingAdd.Any(o => o.calls == calls && o.instigator == instigator))
            {
                Call newCall = new Call(calls, instigator);
                pendingAdd.Add(newCall);
            }
            else
                Debug.LogWarning("CallableSingleUpdateManager: Already found an entry for Callable List with same Instigator, ignoring."); 
        }
        public void RemoveCalls(Callable[] calls, GameObject instigator = null)
        {
            if(updateCalls.Any(o => o.calls == calls && o.instigator == instigator))
            {
                Call found = updateCalls.Where(o => o.calls == calls && o.instigator == instigator).First();
                // Check if not already pending remove
                if(!pendingRemove.Any(o => o.calls == calls && o.instigator == instigator))
                    pendingRemove.Add(found);
            }
            else
                Debug.LogWarning("CallableSingleUpdateManager: Did not found a matching entry for Callable List with same Instigator, cannot remove.");
        }

        public void Update()
        {
            if (!GameplayIngredientsSettings.currentSettings.allowUpdateCalls)
                return;

            foreach(var call in updateCalls)
            {
                if(call.calls == null)
                {
                    Debug.LogError("Null Calls, cannot execute");
                    continue;
                }

                if(!pendingRemove.Contains(call))
                    Callable.Call(call.calls, call.instigator);
            }
        }

        public void LateUpdate()
        {
            foreach(var call in pendingAdd)
            {
                updateCalls.Add(call);
            }
            pendingAdd.Clear();

            foreach(var call in pendingRemove)
            {
                updateCalls.RemoveAll(o => o.calls == call.calls && o.instigator == call.instigator);
            }
            pendingRemove.Clear();    
        }
    }
}


