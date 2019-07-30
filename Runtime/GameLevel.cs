using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayIngredients.LevelStreaming;
using NaughtyAttributes;
using GameplayIngredients;

public class GameLevel : ScriptableObject
{
    [ReorderableList, Scene]
    public string[] StartupScenes;
}
