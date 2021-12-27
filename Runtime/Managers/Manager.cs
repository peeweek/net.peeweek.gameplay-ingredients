using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace GameplayIngredients
{
    
    [HelpURL(Help.URL + "managers")]
    public abstract class Manager : MonoBehaviour
    {
        private static Dictionary<Type, Manager> s_Managers = new Dictionary<Type, Manager>();

        public static bool TryGet<T>(out T manager) where T: Manager
        {
            manager = null;
            if(s_Managers.ContainsKey(typeof(T)))
            {
                manager = (T)s_Managers[typeof(T)];
                return true;
            }
            else
                return false;
        }

        public static T Get<T>() where T: Manager
        {
            if(s_Managers.ContainsKey(typeof(T)))
                return (T)s_Managers[typeof(T)];
            else
            {
                Debug.LogError($"Manager of type '{typeof(T)}' could not be accessed. Check the excludedManagers list in your GameplayIngredientsSettings configuration file.");
                return null;
            }
        }

        public static bool Has<T>() where T:Manager
        {
            return(s_Managers.ContainsKey(typeof(T)));
        }

        static readonly Type[] kAllManagerTypes = TypeUtility.GetConcreteTypes<Manager>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void AutoCreateAll()
        {
            s_Managers.Clear();

            var exclusionList = GameplayIngredientsSettings.currentSettings.excludedeManagers;

            if(GameplayIngredientsSettings.currentSettings.verboseCalls)
                Debug.Log("Initializing all Managers...");

            DependencyGraph dg = new DependencyGraph();

            foreach (var type in kAllManagerTypes)
            {
                // Check for any Do Not Create Attribute
                var doNotCreateAttr = type.GetCustomAttribute<DoNotCreateManagerAttribute>();
                if (doNotCreateAttr != null)
                    continue;

                dg.Add(type);

                var dependencies = type.GetCustomAttribute<ManagerDependsOnAttribute>();
                if(dependencies != null)
                {
                    foreach(var depType in dependencies.dependantTypes)
                    {
                        dg.AddDependency(type, depType);
                    }
                }
            }

            if (exclusionList != null)
            {
                foreach (var type in exclusionList)
                {
                    if(dg.TryRemove(type, exclusionList))
                    {
                        if (GameplayIngredientsSettings.currentSettings.verboseCalls)
                            Debug.LogWarning($"Manager : Excluded {type} from manager creation");
                    }
                    else
                    {
                        if (GameplayIngredientsSettings.currentSettings.verboseCalls)
                            Debug.LogWarning($"Manager : Could not exclude {type} from manager creation because it has dependencies");
                    }
                }
            }

            List<Type> toBeCreated = dg.GetOrderedList();

            // Finally, create all managers
            foreach (var type in toBeCreated)
            {
                var prefabAttr = type.GetCustomAttribute<ManagerDefaultPrefabAttribute>();
                GameObject gameObject;

                if (prefabAttr != null)
                {
                    var prefab = Resources.Load<GameObject>(prefabAttr.prefab);

                    if (prefab == null) // Try loading the "Default_" prefixed version of the prefab
                    {
                        prefab = Resources.Load<GameObject>("Default_" + prefabAttr.prefab);
                    }

                    if (prefab != null)
                    {
                        gameObject = GameObject.Instantiate(prefab);
                    }
                    else
                    {
                        Debug.LogError($"Could not instantiate default prefab for {type.ToString()} : No prefab '{prefabAttr.prefab}' found in resources folders. Ignoring...");
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
                s_Managers.Add(type, comp);

                if (GameplayIngredientsSettings.currentSettings.verboseCalls)
                    Debug.Log(string.Format(" -> <{0}> OK", type.Name));
            }
        }

        class DependencyGraph
        {
            List<DependencyNode> nodes;

            public DependencyGraph()
            {
                nodes = new List<DependencyNode>();
            }

            public void Add(Type o)
            {
                if (!nodes.Any(n => n.target == o))
                {
                    var node = new DependencyNode(o);
                    nodes.Add(node);
                }
            }

            DependencyNode Get(Type o)
            {
                return nodes.Where(n => n.target == o).FirstOrDefault();
            }

            public void AddDependency(Type o, Type dependency)
            {
                Add(o);
                Add(dependency);
                var node = Get(o);
                if(!IsDependentOn(dependency, o)) // Prevent Circular Dependency
                {
                    node.dependencies.Add(Get(dependency));
                }
                else
                {
                    if (GameplayIngredientsSettings.currentSettings.verboseCalls)
                        Debug.LogWarning($"Managers : Found circular dependency {o} -> {dependency}, ignoring");
                }
            }

            // Try remove a type by its name, if it's not a dependency of another node
            public bool TryRemove(string type, string[] exclusionList)
            {
                var n = nodes.Where(n => n.target.Name == type).FirstOrDefault();

                foreach(var node in nodes)
                {
                    // If type is not excluded and our type is a dependency, then we can't remove the node
                    if (!exclusionList.Contains(node.target.Name) && IsDependentOn(node.target, n.target))
                        return false;
                }    

                nodes.Remove(n);
                return true;
            }

            // Walk the tree to find dependencies
            public bool IsDependentOn(Type type, Type dependency)
            {
                foreach(var dep in Get(type).dependencies)
                {
                    if (dep.target == type)
                        return true;
                    else
                        return (IsDependentOn(dep.target, dependency));
                }
                return false;
            }

            // Return all dependencies of a given type
            public List<Type> GetDependencies(Type type)
            {
                List<Type> t = new List<Type>();
                t.Add(type);

                foreach(var dep in Get(type).dependencies)
                {
                    if (!t.Contains(dep.target))
                        t.Concat(GetDependencies(dep.target));
                }
                return t;
            }


            // Builds a list of loading order
            public List<Type> GetOrderedList()
            {
                Dictionary<Type, int> d = new Dictionary<Type, int>();

                foreach(var node in nodes)
                {
                    if (!d.ContainsKey(node.target))
                        d.Add(node.target, 0);
                    else
                        d[node.target] += 1;

                    foreach(var dep in node.dependencies)
                    {
                        if (!d.ContainsKey(dep.target))
                            d.Add(dep.target, 0);
                        else
                            d[dep.target] += 1;
                    }
                }

                return d.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value).Keys.ToList();

            }

            class DependencyNode
            {
                public List<DependencyNode> dependencies;
                public readonly Type target;

                public DependencyNode(Type target)
                {
                    dependencies = new List<DependencyNode>();
                    this.target = target;
                }

                public override string ToString()
                {
                    return target.Name;
                }
            }
        }
    }
}
