using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GUI.Debuggers
{
    public class VelocityDebugger : AbstractDebugger
    {
        public override void Draw(Vector2 position)
        {
            string label = "Velocity: \n";
            label += GameObject.Find("Main Camera").GetComponent<CharacterController>().velocity.ToString();
            content.text = label;

            style.CalcSize(content);
            UnityEngine.GUI.Label(new Rect(position.x, position.y + 20, 20, 20), label, style);
        }
    }
}
