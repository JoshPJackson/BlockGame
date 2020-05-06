using System.Collections;
using UnityEngine;

namespace Game.GUI.Debuggers
{
    public class CameraDebugger : AbstractDebugger
    {
        public override void Draw(Vector2 position)
        {
            GameObject camera = GameObject.Find("Main Camera");
            string cameraLabel = "Camera: \n";
            cameraLabel += "Position: " + camera.transform.position.ToString();
            cameraLabel += "\n" + "Orientation: " + camera.transform.eulerAngles.ToString();

            style = new GUIStyle();
            content = new GUIContent();
            content.text = cameraLabel;

            style.CalcSize(content);
            UnityEngine.GUI.Label(new Rect(position.x, position.y + 20, 20, 20), cameraLabel, style);
        }
    }
}
