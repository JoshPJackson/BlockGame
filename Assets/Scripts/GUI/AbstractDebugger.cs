using System.Collections;
using UnityEngine;

namespace Game.GUI.Debuggers {
    public abstract class AbstractDebugger : IDebugger
    {
        abstract public void Draw(Vector2 position);

        protected GUIContent content;

        protected GUIStyle style;

        public AbstractDebugger()
        {
            content = new GUIContent();
            style = new GUIStyle();
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        public Vector2 GetRenderedSize()
        {
            return style.CalcSize(this.content);
        }
    }
}
