using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace GameplayIngredients
{
    public abstract class Manager : MonoBehaviour
    {
        private static Dictionary<Type, Manager> s_Managers = new Dictionary<Type, Manager>();

        public static T Get<T>() where T: Manager
        {
            if(s_Managers.ContainsKey(typeof(T)))
                return (T)s_Managers[typeof(T)];
            else
                return null;
        }

        static readonly Type[] kAllManagerTypes = GetAllManagerTypes();

        [RuntimeInitializeOnLoadMethod]
        static void AutoCreateAll()
        {
            var exclusionList = Resources.Load<ManagerExclusionList>("ManagerExclusionList");

            Debug.Log("Initializing all Managers...");
            foreach(var type in kAllManagerTypes)
            {
                if(exclusionList != null && exclusionList.ExcludedManagers.ToList().Contains(type.Name))
                {
                    Debug.LogWarning($"Found Manager : {type.Name} in Exclusion List, ignoring Creation");
                    continue;
                }
                var attrib =type.GetCustomAttribute<ManagerDefaultPrefabAttribute>(); 
                GameObject gameObject;

                if(attrib != null)
                {
                    var prefab = Resources.Load<GameObject>(attrib.prefab);

                    if(prefab == null) // Try loading the "Default_" prefixed version of the prefab
                    {
                        prefab = Resources.Load<GameObject>("Default_"+attrib.prefab);
                    }

                    if(prefab != null)
                    {
                        gameObject = GameObject.Instantiate(prefab);
                    }
                    else
                    {
                        Debug.LogError($"Could not instantiate default prefab for {type.ToString()} : No prefab '{attrib.prefab}' found in resources folders. Ignoring...");
                        continue;
                    }
                }
                else
                {
                    gameObject = new GameObject();
                    gameObject.AddComponent(type);
                }
                gameObject.name = type.Name;
                GameObject.DontDestroyOnLoad(gameObject);
                var comp = (Manager)gameObject.GetComponent(type);
                s_Managers.Add(type,comp);

                Debug.Log(string.Format(" -> <{0}> OK", type.Name));
            }
        }

        static Type[] GetAllManagerTypes()
        {
            List<Type> types = new List<Type>();
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type t in assembly.GetTypes())
                {
                    if(typeof(Manager).IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        types.Add(t);
                    }                
                }
            }
            return types.ToArray();
        }
    }
}
