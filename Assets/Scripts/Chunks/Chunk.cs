using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Blocks;
using Game.Textures;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace Game.Chunks
{
    public class Chunk
    {
        public Vector3 globalPosition;

        public int localPosition;

        public ChunkColumn parent;

        public Block[,,] blocks;

        public GameObject GameObject;

        public static int size = 16;

        public bool rendered = false;

        public Vector2 chunkColumnPosition;

        public Chunk()
        {
            GameObject = new GameObject("chunk");
            blocks = new Block[size, size, size];
        }

        public Block GetBlockInChunk(Vector3 position)
        {
            return blocks[(int) position.x, (int) position.y, (int) position.z];
        }

        public void SetBlock(Block block, Vector3 position)
        {
            blocks[(int) position.x, (int) position.y, (int) position.z] = block;
        }

        public void Render()
        {
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    for (int z = 0; z < size; z++) {
                        blocks[x, y, z].Render();
                    }
                }
            }

            Optimize();
        }

        public void Optimize()
        {
            if (!rendered)
            {
                //1. Combine all children meshes
                MeshFilter[] meshFilters = GameObject.GetComponentsInChildren<MeshFilter>();
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];
                int i = 0;
                while (i < meshFilters.Length)
                {
                    combine[i].mesh = meshFilters[i].sharedMesh;
                    combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                    i++;
                }

                //2. Create a new mesh on the parent object
                MeshFilter mf = (MeshFilter)GameObject.AddComponent(typeof(MeshFilter));
                mf.mesh = new Mesh();

                //3. Add combined meshes on children as the parent's mesh
                mf.mesh.CombineMeshes(combine);
                TextureAtlasMapper mapper = Game.currentGame.mapper;

                //4. Create a renderer for the parent
                MeshRenderer renderer = GameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                renderer.material = (Material)Resources.Load("mymaterial", typeof(Material));
                renderer.material.mainTexture = mapper.atlas;


                //5. Delete all uncombined children
                foreach (Transform quad in GameObject.transform)
                {
                    GameObject.Destroy(quad.gameObject);
                }

                // 6. Add collision
                MeshCollider collider = GameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                collider.sharedMesh = GameObject.transform.GetComponent<MeshFilter>().mesh;
                rendered = true;
            }
        }

        public void Save()
        {
            string fileName = GetFileName(chunkColumnPosition, localPosition);

                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                fileStream.Close();

                StreamWriter streamWriter = new StreamWriter(fileName, true, Encoding.ASCII);

                for (int i = 0; i < Chunk.size; i++)
                {
                    for (int j = 0; j < Chunk.size; j++)
                    {
                        for (int k = 0; k < Chunk.size; k++)
                        {
                            streamWriter.WriteLine(blocks[i, j, k].ToString());
                        }
                    }
                }

                streamWriter.Close();
            
        }

        public static void StaticSave(Vector2 chunkColumnPosition, int localPosition, Block[,,] blocks)
        {
            string fileName = GetFileName(chunkColumnPosition, localPosition);
            FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            fileStream.Close();

            StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.ASCII);

            for (int i = 0; i < Chunk.size; i++)
            {
                for (int j = 0; j < Chunk.size; j++)
                {
                    for (int k = 0; k < Chunk.size; k++)
                    {
                        streamWriter.WriteLine(blocks[i, j, k].ToString());
                    }
                }
            }

            streamWriter.Close();
        }

        public static Chunk Load(Vector2 chunkColumnLocalPosition, int localChunkPosition)
        {
            Chunk chunk = new Chunk();
            chunk.chunkColumnPosition = chunkColumnLocalPosition;
            chunk.localPosition = localChunkPosition;
            chunk.parent = Game.currentGame.world.chunkColumns[(int) chunkColumnLocalPosition.x, (int) chunkColumnLocalPosition.y];
            
            chunk.globalPosition = Chunk.size * new Vector3(
                chunkColumnLocalPosition.x,
                localChunkPosition,
                chunkColumnLocalPosition.y
            );

            string fileName = GetFileName(chunkColumnLocalPosition, localChunkPosition);

            StreamReader streamReader = new StreamReader(fileName);

            for (int x = 0; x < Chunk.size; x++)
            {
                for (int y = 0; y < Chunk.size; y++)
                {
                    for (int z = 0; z < Chunk.size; z++)
                    {
                        string className = streamReader.ReadLine();

                        Block newBlock = (Block) Activator.CreateInstance(Type.GetType(className));

                        newBlock.chunk = chunk;
                        newBlock.positionInChunk = new Vector3(x, y, z);
                        newBlock.globalPosition = new Vector3(
                            chunk.globalPosition.x + x,
                            chunk.globalPosition.y + y,
                            chunk.globalPosition.z + z
                        );

                        chunk.blocks[x, y, z] = newBlock;
                    }
                }
            }

            streamReader.Close();

            return chunk;
        }

        public static string GetFileName(Vector2 chunkColumnPosition, int localPosition)
        {
            return "C:/Users/joshp/Documents/chunks/chunk_column_" + 
                chunkColumnPosition.x + "_" + 
                chunkColumnPosition.y +  "_chunk_" + 
                localPosition + ".dat";
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}
