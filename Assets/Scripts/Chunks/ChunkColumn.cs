using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Chunks
{
    public class ChunkColumn
    {
        public Vector2 localPosition;

        public Vector3 globalPosition;

        public static int chunksPerColumn = 16;

        public Chunk[] chunks;

        public ChunkColumn()
        {
            chunks = new Chunk[chunksPerColumn];
        }

        public WorldBuilder parent;

        public void SetChunk(Chunk chunk, int position)
        {
            chunks[position] = chunk;
        }
    }
}