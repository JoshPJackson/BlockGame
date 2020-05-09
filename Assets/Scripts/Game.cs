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

        public Vector3 radius = new Vector3(3, 2, 3);

        public Vector3 chunkPosition = new Vector3();

        public Vector3 currentPosition;

        public bool useGravity = false;

        // Start is called before the first frame update
        void Start()
        {
            world = new WorldBuilder(new Vector2(5, 5));

            mapper = new TextureAtlasMapper();
            currentGame = this;
            setUpDebug();
            setUpCamera();
            world.startPosition = new Vector3(10, 0, 10);
            world.Generate();
            world.startPosition.y += 20;
            Camera.main.transform.position = world.startPosition;
            Camera.main.gameObject.AddComponent<FirstPersonCamera>();
        }

        // Update is called once per frame
        void Update()
        {
            Render();
        }

        void Render()
        {
            currentPosition = Camera.main.transform.position;

            for (int x = 0; x < world.Size.x; x++)
            {
                chunkPosition.x = x;

                for (int y = 0; y < world.Size.y; y++)
                {
                    chunkPosition.z = y;

                    for (int i = 0; i < ChunkColumn.chunksPerColumn; i++)
                    {
                        chunkPosition.y = i;

                        if (
                            Math.Abs(currentPosition.x / Chunk.size - chunkPosition.x) < radius.x &&
                            Math.Abs(currentPosition.z / Chunk.size - chunkPosition.z) < radius.z &&
                            Math.Abs(currentPosition.y / Chunk.size - i) < radius.y
                        )
                        {
                            if (world.chunkColumns[x, y].chunks[i] == null)
                            {
                                Chunk chunk = Chunk.Load(new Vector2(x, y), i);

                                world.loaded++;

                                world.chunkColumns[x, y].chunks[i] = chunk;
                                chunk.Render();
                                chunk.Optimize();
                                return;
                            }
                        }
                        else
                        {
                            if (world.chunkColumns[x, y].chunks[i] != null)
                            {
                                world.chunkColumns[x, y].chunks[i].Save();
                                world.chunkColumns[x, y].chunks[i].Destroy();
                                world.chunkColumns[x, y].chunks[i] = null;
                                world.loaded--;
                                return;
                            }
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
            camera.AddComponent<CharacterController>();

        }
    }
}