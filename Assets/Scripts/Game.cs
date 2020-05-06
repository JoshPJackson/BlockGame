using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.GUI;
using Game.Blocks;
using Game.Chunks;
using Game.Textures;
using System;

namespace Game {
    public class Game : MonoBehaviour
    {
        public bool debugMode = true;

        public DebugGUI debugGUI;

        public GameObject debugGUIObject;

        public WorldBuilder world;

        public TextureAtlasMapper mapper;

        public static Game currentGame;

        // Start is called before the first frame update
        void Start()
        {
            world = new WorldBuilder();
            world.Generate();

            mapper = new TextureAtlasMapper();
            currentGame = this;
            setUpDebug();
            setUpCamera();
            Camera.main.transform.position = new Vector3(10, 100, 10);
        }

        // Update is called once per frame
        void Update()
        {
            StartCoroutine("Render");
        }

        IEnumerator Render()
        {
            int radius = 2;
            Vector3 currentPosition = Camera.main.transform.position;
            Vector2 currentChunkPosition = new Vector2((float) Math.Floor(currentPosition.x / Chunk.size), (float) Math.Floor(currentPosition.z / Chunk.size));
            float chunkColumnDistance;

            for (int x = 0; x < world.Size.x; x++)
            {
                for (int y = 0; y < world.Size.y; y++)
                {
                    chunkColumnDistance = Mathf.Sqrt((float) Math.Pow(x - currentChunkPosition.x, 2) + (float)Math.Pow(y - currentChunkPosition.y, 2));

                    if (chunkColumnDistance <= radius)
                    {
                        for (int i = ChunkColumn.chunksPerColumn - 1; i > -1; --i)
                        {
                            if (world.chunkColumns[x, y].chunks[i].rendered)
                            {
                                continue;
                            }

                            world.chunkColumns[x, y].chunks[i].Render();
                            yield return null;
                        }
                    }
                }
            }
        }

        void setUpDebug()
        {
            debugGUIObject = new GameObject("Debug GUI");
            debugGUIObject.AddComponent<DebugGUI>();
            debugGUIObject.SetActive(debugMode);
        }

        void setUpCamera()
        {
            GameObject camera = GameObject.Find("Main Camera");
            camera.AddComponent<FirstPersonCamera>();
            camera.AddComponent<CharacterController>();

        }
    }
}