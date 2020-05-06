using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Blocks;

namespace Game.Meshes
{
    public class BlockMesh
    {
        public List<QuadMesh> meshes = new List<QuadMesh>();

        public BlockMesh(Block.Cubeside[] sides)
        {
            for (int i = 0; i < sides.Length; i++)
            {
                new QuadMesh(sides[i]);
            }
        }
    }
}
