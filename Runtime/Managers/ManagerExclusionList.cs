using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients
{
    public class ManagerExclusionList : ScriptableObject
    {
        [ReorderableList, TypeDropDown(typeof(Manager))]
        public string[] ExcludedManagers;
    }
}
