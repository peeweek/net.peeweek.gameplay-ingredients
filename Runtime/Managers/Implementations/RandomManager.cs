using UnityEngine;

namespace GameplayIngredients
{
    [ManagerDefaultPrefab("RandomManager")]
    public class RandomManager : Manager
    {
        public bool setInitialSeed = false;
        public int initialSeed = 0;

        public int currentSeed { get; private set; }

        void Awake()
        {
            if (setInitialSeed)
                SetRandomSeed(initialSeed);
        }

        public void SetRandomSeed(int seed)
        {
            Random.InitState(seed);
            currentSeed = seed;
        }
    }
}


