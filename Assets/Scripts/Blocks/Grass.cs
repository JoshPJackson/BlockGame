using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Meshes;
using Game.Textures;

namespace Game.Blocks
{
    public class Grass : Block
    {
        public override string[] textureNames => new string[3] { "grass_top", "dirt", "grass_side" };

        public override void CreateQuad(Cubeside side)
        {
            string textureName;

            switch (side)
            {
                case Cubeside.TOP: {
                        textureName = textureNames[0];
                        break;
                    }

                case Cubeside.BOTTOM:
                    {
                        textureName = textureNames[1];
                        break;
                    }

                default:
                    {
                        textureName = textureNames[2];
                        break;
                    }
            }

            QuadMesh quadMesh = new QuadMesh(side);

            TextureAtlasMapper mapper = Game.currentGame.mapper;
            Vector2[] uvs = mapper.GetMappedUVs(textureName);

            GameObject quad = new GameObject("Quad");

            quad.transform.position = globalPosition;
            quad.transform.parent = chunk.GameObject.transform;
            MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
            Mesh myMesh = quadMesh.mesh;
            myMesh.uv = uvs;
            myMesh.RecalculateBounds();
            myMesh.name = "my mesh";
            meshFilter.mesh = myMesh;

            MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        }
    }
}
