using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients.Interactive
{
    public class BasicInteractiveUser : InteractiveUser
    {
        public Camera Camera;
        public float InteractionAngle = 60.0f;

        public override bool CanInteract(Interactive interactive)
        {
            Vector3 toInteractive = (interactive.transform.position - Camera.transform.position).normalized;
            return Mathf.Acos(Vector3.Dot(toInteractive, Camera.transform.forward)) < InteractionAngle;
        }
    }
}

