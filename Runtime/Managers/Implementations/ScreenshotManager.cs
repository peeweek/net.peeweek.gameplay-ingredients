using UnityEngine;

namespace GameplayIngredients
{
#if !MODULE_SCREENCAPTURE
    [WarnDisabledModule("Screen Capture")]
#endif
    [AddComponentMenu(ComponentMenu.managersPath + "Screenshot Manager")]
    [ManagerDefaultPrefab("ScreenshotManager")]
    public class ScreenshotManager : Manager
    {
        [Header("Capture")]
        public KeyCode ScreenshotKeyCode = KeyCode.F11;
        [Range(1, 5)]
        public int SuperSize = 1;

        [Header("File name")]
        public string Prefix = "Screenshot";

        [Header("Actions")]
        public Callable[] OnBeforeScreenshot;
        public Callable[] OnAfterScreenshot;

        public void Update()
        {
            if (Input.GetKeyDown(ScreenshotKeyCode))
            {
#if MODULE_SCREENCAPTURE
                var now = System.DateTime.Now;
                Callable.Call(OnBeforeScreenshot);
                string path = $"{Application.dataPath}/../{Prefix}-{now.Year}{now.Month}{now.Day}-{now.Hour}{now.Minute}{now.Second}{now.Millisecond}.png";
                Debug.Log($"Capturing Screenshot (Supersampled to {SuperSize}x) to the file : {path}");
                ScreenCapture.CaptureScreenshot(path, SuperSize);
                Callable.Call(OnAfterScreenshot);
#else
                Debug.Log("Screenshot Manager Cannot Take Screenshot : Unity Module Screen Capture is Disabled.");
#endif
            }
        }
    }
}
