using UnityEngine;

namespace GameplayIngredients
{
    public class PlayerInputActionAttribute : PropertyAttribute 
    {
        public readonly string name;

        public PlayerInputActionAttribute(string propertyName)
        {
            this.name = propertyName;
        }
    }
}
