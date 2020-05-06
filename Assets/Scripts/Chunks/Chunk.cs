using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Blocks;
using Game.Textures;

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
    }
}
