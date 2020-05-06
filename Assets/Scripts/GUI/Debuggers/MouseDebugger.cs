using System.Collections;
using UnityEngine;

namespace Game.GUI.Debuggers
{
    public class MouseDebugger : AbstractDebugger
    {
        public override void Draw(Vector2 position)
        {
            var view = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            string mouseLabel = "Mouse: \n";
            mouseLabel += "Position: " + Input.mousePosition.ToString() + "\n";
            mouseLabel += "In Scope: " + (view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1 ? "False" : "True");

            style = new GUIStyle();
            content = new GUIContent();
            content.text = mouseLabel;

            style.CalcSize(content);
            UnityEngine.GUI.Label(new Rect(position.x, position.y + 20, 20, 20), mouseLabel, style);
        }
    }
}
