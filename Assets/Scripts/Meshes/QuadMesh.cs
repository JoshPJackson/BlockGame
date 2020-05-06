using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Textures;
using Game.Blocks;

namespace Game.Meshes
{
    public class QuadMesh
    {
        Vector3[] allVertices = new Vector3[8];

        Vector3[] vertices = new Vector3[4];

        Vector3[] normals = new Vector3[4];

        Vector2[] uvs = new Vector2[4];

        public Mesh mesh; 

        int[] triangles = new int[6] { 3, 1, 0, 3, 2, 1 };

        public QuadMesh(Block.Cubeside side)
        {
            SetAllVertices();

            mesh = new Mesh();
            
            normals = GetNormalCollection(Vector3.up);

            switch (side)
            {
                case Block.Cubeside.BOTTOM:
                    vertices = GetVertexCollection(new int[] { 0, 1, 2, 3});
                    normals = GetNormalCollection(Vector3.down);
                    break;
                case Block.Cubeside.TOP:
                    vertices = GetVertexCollection(new int[] { 7, 6, 5, 4 });
                    break;
                case Block.Cubeside.LEFT:
                    vertices = GetVertexCollection(new int[] { 7, 4, 0, 3 });
                    normals = GetNormalCollection(Vector3.left);
                    break;
                case Block.Cubeside.RIGHT:
                    vertices = GetVertexCollection(new int[] { 5, 6, 2, 1 });
                    normals = GetNormalCollection(Vector3.right);
                    break;
                case Block.Cubeside.FRONT:
                    vertices = GetVertexCollection(new int[] { 4, 5, 1, 0 });
                    break;
                case Block.Cubeside.BACK:
                    vertices = GetVertexCollection(new int[] { 6, 7, 3, 2 });
                    break;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            // mesh.uv = uvs;

            Vector2 uv00 = new Vector2(0f, 0f);
            Vector2 uv10 = new Vector2(1f, 0f);
            Vector2 uv01 = new Vector2(0f, 1f);
            Vector2 uv11 = new Vector2(1f, 1f);
            mesh.uv = new Vector2[] { uv11, uv01, uv00, uv10 };

            mesh.triangles = triangles;
            mesh.RecalculateBounds();
        }

        private void SetAllVertices()
        {
            allVertices.SetValue(new Vector3(-0.5f, -0.5f, 0.5f), 0);
            allVertices.SetValue(new Vector3(0.5f, -0.5f, 0.5f), 1);
            allVertices.SetValue(new Vector3(0.5f, -0.5f, -0.5f), 2);
            allVertices.SetValue(new Vector3(-0.5f, -0.5f, -0.5f), 3);
            allVertices.SetValue(new Vector3(-0.5f, 0.5f, 0.5f), 4);
            allVertices.SetValue(new Vector3(0.5f, 0.5f, 0.5f), 5);
            allVertices.SetValue(new Vector3(0.5f, 0.5f, -0.5f), 6);
            allVertices.SetValue(new Vector3(-0.5f, 0.5f, -0.5f), 7);
        }

        private Vector3[] GetVertexCollection(int[] indices)
        {
            Vector3[] toReturn = new Vector3[4];

            for (int i = 0; i < indices.Length; i++)
            {
                toReturn.SetValue(allVertices[indices[i]], i);
            }

            return toReturn;
        }

        private Vector3[] GetNormalCollection(Vector3 normal)
        {
            return new Vector3[4]
            {
                normal,
                normal,
                normal,
                normal
            };
        }
    }
}
