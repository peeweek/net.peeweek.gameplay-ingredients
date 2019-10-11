using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace GameplayIngredients
{
    public class TimerDisplayRig : MonoBehaviour
    {
        [NonNullCheck]
        public Text text;
        [NonNullCheck]
        public TextMesh textMesh;


        [NonNullCheck]
        public Timer timer;

        [InfoBox("Use the following wildcards:\n - %h : hours\n - %m : minutes\n - %s : seconds\n - %x : milliseconds", InfoBoxType.Normal)]
        public string format = "%h:%m:%s:%x";

        private void Update()
        {
            if (timer == null || (text == null && textMesh == null))
                return;

            var value = format;
            value = value.Replace("%h", timer.CurrentHours.ToString("D2"));
            value = value.Replace("%m", timer.CurrentMinutes.ToString("D2"));
            value = value.Replace("%s", timer.CurrentSeconds.ToString("D2"));
            value = value.Replace("%x", timer.CurrentMilliseconds.ToString("D3"));

            if (text != null)
                text.text = value;

            if (textMesh != null)
                textMesh.text = value;
        }
    }
}

