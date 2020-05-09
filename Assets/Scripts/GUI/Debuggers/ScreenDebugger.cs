using System.Collections;
using UnityEngine;

namespace Game.GUI.Debuggers
{
    public class ScreenDebugger : AbstractDebugger
    {
        public override void Draw(Vector2 position)
        {
            string label = "Screen: \n";
            label += "Dimensions: " + Screen.height + "x" + Screen.width;
            content.text = label;

            style.CalcSize(content);
            UnityEngine.GUI.Label(new Rect(position.x, position.y + 20, 20, 20), label, style);
        }
    }
}