using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.U2D;
using Game.Meshes;
using Game.Textures;
using Game.Chunks;

namespace Game.Blocks
{
    abstract public class Block
    {
        // cube sides
        public enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

        // block types
        public enum BlockType { AIR, DIRT, GRASS }

        // uvs
        public virtual string[] textureNames { get; }

        public bool isSolid = true;

        // global position
        public Vector3 globalPosition;
        public Vector3 localPosition;

        // parent chunk
        public Chunk chunk;

        // position inside chunk
        public Vector3 positionInChunk;

        public Cubeside[] renderedSides;

        public Mesh mesh;

        public GameObject parent;

        public bool hasMesh = false;

        public Dictionary<Cubeside, bool> hasCubeSideMesh;

        public Block()
        {
            hasCubeSideMesh = new Dictionary<Cubeside, bool>(6);
            hasCubeSideMesh[Cubeside.TOP] = false;
            hasCubeSideMesh[Cubeside.BOTTOM] = false;
            hasCubeSideMesh[Cubeside.LEFT] = false;
            hasCubeSideMesh[Cubeside.RIGHT] = false;
            hasCubeSideMesh[Cubeside.FRONT] = false;
            hasCubeSideMesh[Cubeside.BACK] = false;
        }
        
        public WorldBuilder worldBuilder()
        {
            return chunk.parent.parent;
        }

        public void Render()
        {
            if (this.GetType() != typeof(Air)) {
                if (HasAirBack() && !hasCubeSideMesh[Cubeside.BACK])
                {
                    CreateQuad(Cubeside.BACK);
                }

                if (HasAirBelow() && !hasCubeSideMesh[Cubeside.BOTTOM])
                {
                    CreateQuad(Cubeside.BOTTOM);
                }

                if (HasAirFront() && !hasCubeSideMesh[Cubeside.FRONT])
                {
                    CreateQuad(Cubeside.FRONT);
                }

                if (HasAirAbove() && !hasCubeSideMesh[Cubeside.TOP])
                {
                    CreateQuad(Cubeside.TOP);
                }

                if (HasAirLeft() && !hasCubeSideMesh[Cubeside.LEFT])
                {
                    CreateQuad(Cubeside.LEFT);
                }

                if (HasAirRight() && !hasCubeSideMesh[Cubeside.RIGHT])
                {
                    CreateQuad(Cubeside.RIGHT);
                }
            }
        }

        public bool HasAirAbove()
        {
            int maxHeight = ChunkColumn.chunksPerColumn * Chunk.size;

            if (globalPosition.y + 1 > maxHeight - 1)
            {
                return true;
            }

            Block blockAbove = worldBuilder().GetBlockAtGlobalPosition(new Vector3(
                globalPosition.x,
                globalPosition.y + 1,
                globalPosition.z
            ));
            
            return blockAbove.GetType() == typeof(Air);
        }

        public bool HasAirBelow()
        {
            if (globalPosition.y - 1 >= 0)
            {

                Block blockAbove = worldBuilder().GetBlockAtGlobalPosition(new Vector3(
                    globalPosition.x,
                    globalPosition.y - 1,
                    globalPosition.z
                ));

                return blockAbove.GetType() == typeof(Air);
            }

            return true;
        }

        public bool HasAirLeft()
        {
            if (globalPosition.x - 1 >= 0)
            {
                Block blockAbove = worldBuilder().GetBlockAtGlobalPosition(new Vector3(
                    globalPosition.x - 1,
                    globalPosition.y,
                    globalPosition.z
                ));


                return blockAbove.GetType() == typeof(Air);
            }

            return true;
        }

        public bool HasAirRight()
        {
            if (globalPosition.x + 1 < worldBuilder().Size.x * Chunk.size)
            {
                Block blockAbove = worldBuilder().GetBlockAtGlobalPosition(new Vector3(
                    globalPosition.x + 1,
                    globalPosition.y,
                    globalPosition.z
                ));

                return blockAbove.GetType() == typeof(Air);
            }

            return true;
        }

        public bool HasAirBack()
        {
            if (globalPosition.z - 1 >= 0)
            {
                Block blockAbove = worldBuilder().GetBlockAtGlobalPosition(new Vector3(
                    globalPosition.x,
                    globalPosition.y,
                    globalPosition.z - 1
                ));

                return blockAbove.GetType() == typeof(Air);
            }

            return true;
        }

        public bool HasAirFront()
        {
            if (globalPosition.z + 1 < worldBuilder().Size.x * Chunk.size - 1)
            {
                Block blockAbove = worldBuilder().GetBlockAtGlobalPosition(new Vector3(
                    globalPosition.x,
                    globalPosition.y,
                    globalPosition.z + 1
                ));

                return blockAbove.GetType() == typeof(Air);
            }

            return true;
        }

        public virtual void CreateQuad(Cubeside side)
        {
            string[] textures = new String[6];

            if (textureNames.Length == 1)
            {
                for (int j = 0; j < 6; j++)
                {
                    textures[j] = textureNames[0];
                }
            } else
            {
                textures = textureNames;
            }

            for (int i = 0; i < textures.Length; i++)
            {
                QuadMesh quadMesh = new QuadMesh(side);

                TextureAtlasMapper mapper = Game.currentGame.mapper;
                Vector2[] uvs = mapper.GetMappedUVs(textures[i]);
                
                GameObject quad = new GameObject("Quad");

                quad.transform.position = globalPosition;
                quad.transform.parent = chunk.GameObject.transform;
                MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
                Mesh myMesh = quadMesh.mesh;
                myMesh.uv = uvs;
                myMesh.RecalculateBounds();
                myMesh.name = "my mesh";
                meshFilter.mesh = myMesh;

                
                hasCubeSideMesh[side] = true;
            }
        }

        public override string ToString()
        {
            return GetType().ToString();
        }
    }
}
