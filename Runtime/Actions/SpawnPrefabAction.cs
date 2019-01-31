using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Actions
{
    public class SpawnPrefabAction : ActionBase
    {
        [ReorderableList]
        public GameObject[] Prefabs;

        public Transform TargetTransform;

        public bool AttachToTarget = false;
        public bool DontDestroyPrefabsOnLoad = false;

        public override void Execute(GameObject instigator = null)
        {
            foreach (var prefab in Prefabs)
            {
                string name = prefab.name;
                var obj = Instantiate<GameObject>(prefab);
                obj.name = name;

                obj.transform.position = TargetTransform.position;
                obj.transform.rotation = TargetTransform.rotation;
                if (AttachToTarget)
                    obj.transform.parent = TargetTransform;

                if (DontDestroyPrefabsOnLoad)
                    DontDestroyOnLoad(obj);
            }
        }
    }
}
