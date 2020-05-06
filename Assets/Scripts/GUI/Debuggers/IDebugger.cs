using UnityEngine;

namespace Game.GUI.Debuggers
{
    public interface IDebugger
    {
        void Draw(Vector2 postion);

        Vector2 GetRenderedSize();
    }
}
