using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayIngredients.Editor
{
    static class MenuItems
    {
        [MenuItem("Edit/Select None &D", priority = 149)]
        static void UnselectAll()
        {
            Selection.activeObject = null;
        }


        static readonly string helperPreferenceName = "GameplayIngredients.toggleIngredientHelpers";
        [MenuItem("Edit/Gameplay Ingredients/Toggle Helpers", priority = 117)]
        static void ToggleIngredientHelpers()
        {
            bool value = EditorPrefs.GetBool(helperPreferenceName, false);
            value = !value;
            EditorPrefs.SetBool(helperPreferenceName, value);

        }

        [MenuItem("Edit/Gameplay Ingredients/Toggle Helpers", true, 117)]
        static bool ToggleIngredientHelpersValidation()
        {
            Menu.SetChecked("Edit/Gameplay Ingredients/Toggle Helpers", EditorPrefs.GetBool(helperPreferenceName, false));
            return true;
        }

    }
}

