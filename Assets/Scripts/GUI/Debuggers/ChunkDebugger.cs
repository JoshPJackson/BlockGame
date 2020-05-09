using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GUI.Debuggers
{
    public class ChunkDebugger : AbstractDebugger
    {
        public override void Draw(Vector2 position)
        {
            string label = "Chunks: \n";

            WorldBuilder world = Game.currentGame.world;

            label += "Loaded: " + world.loaded;
            content.text = label;

            style.CalcSize(content);
            UnityEngine.GUI.Label(new Rect(position.x, position.y + 20, 20, 20), label, style);
        }
    }
}
