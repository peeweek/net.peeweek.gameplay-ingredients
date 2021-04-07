using UnityEngine;

namespace GameplayIngredients.Actions
{
    [AddComponentMenu(ComponentMenu.actionsPath + "Take Screenshot Action")]
    [Callable("Screen", "Actions/ic-action-screen.png")]
    public class TakeScreenshotAction : ActionBase
    {
        [Tooltip("Super Sampling multiplier")]
        public int supersampleRate = 1;
        [Tooltip("Base Filename (will be numbered) ")]
        public string fileName = "screenshot";
        [Tooltip("How many digits in the sequence numbers.")]
        public int figureCount = 2;
        private int screenshotNumber = 0;

        public override void Execute(GameObject instigator = null)
        {
            ScreenCapture.CaptureScreenshot(fileName + screenshotNumber.ToString().PadLeft(figureCount, '0') + ".png", supersampleRate);
            screenshotNumber += 1;
        }

        public override string GetDefaultName()
        {
            return $"Take Screenshot : {screenshotNumber.ToString().PadLeft(figureCount, '#')}.png";
        }
    }
}
