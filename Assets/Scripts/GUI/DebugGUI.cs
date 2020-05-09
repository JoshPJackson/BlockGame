using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.GUI.Debuggers;

namespace Game.GUI
{
    public class DebugGUI : MonoBehaviour
    {
        /*
         * Min allowed positionX
         */
        private float minPositionX = 0;

        /*
         * Max allowed positionX
         */
        private float maxPositionX = 500;

        /*
         * Min allowed positionY
         */
        private float minPositionY = 0;

        /**
         * Max allowed positionY
         */
        private float maxPositionY = 0;

        /*
         * Horizontal position
         */
        public float positionX = 0;

        /*
         * Vertical position
         */
        public float positionY = 0;

        /*
         * Width
         */
        public float width = 100;

        /*
         * Height 
         */
        public float height = 50;

        /*
         * List of debuggers to render
         */
        protected List<AbstractDebugger> debuggers;

        void Start()
        {
            debuggers = new List<AbstractDebugger>();
            debuggers.Add(new CameraDebugger());
            debuggers.Add(new MouseDebugger());
            debuggers.Add(new ScreenDebugger());
            debuggers.Add(new VelocityDebugger());
            //debuggers.Add(new BlockCountDebugger());
            debuggers.Add(new ChunkDebugger());
        }

        void OnGUI()
        {
            Vector2 currentPosition = new Vector2(minPositionX, minPositionY);

            debuggers.ForEach(delegate (AbstractDebugger debugger) {
                debugger.Draw(currentPosition);
                currentPosition.y += debugger.GetRenderedSize().y + 15;
            });
        }

        void OnValidate()
        {
            // make sure positionX does not go above max
            if (positionX > maxPositionX)
                positionX = maxPositionX;
            
            // make sure positionX does not go below min
            if (positionX < minPositionX)
                positionX = minPositionX;

            // make sure positionY does not go above max
            if (positionY > maxPositionY)
                positionY = maxPositionY;

            // make sure positionY does not go below min
            if (positionY < minPositionY)
                positionY = minPositionY;
        }
    }
}