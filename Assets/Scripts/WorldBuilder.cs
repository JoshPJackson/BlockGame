using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Game.Chunks;
using Game.Blocks;

public class WorldBuilder
{
    // size of world (chunk column units)
    public Vector2 Size { get; set; }

    private Vector3 Offset { get; set; }

    public ChunkColumn[,] chunkColumns;

    public int worldMagnitude;

    public int heightLimit;

    public WorldBuilder()
    {
        Size = new Vector2(5, 5);
        Offset = Vector3.zero;
        chunkColumns = new ChunkColumn[(int) Size.x, (int) Size.y];
        worldMagnitude = (int) Size.x * Chunk.size;
        heightLimit = ChunkColumn.chunksPerColumn * Chunk.size;
    }

    public void Generate()
    {
        float smoothness = 0.4f;

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                ChunkColumn chunkColumn = new ChunkColumn();
                chunkColumn.localPosition = new Vector2(i, j);
                chunkColumn.globalPosition = new Vector3(
                    i * Chunk.size,
                    0,
                    j * Chunk.size
                );
                chunkColumn.parent = this;

                for (int k = 0; k < ChunkColumn.chunksPerColumn; k++)
                {
                    Chunk chunk = new Chunk();
                    chunk.parent = chunkColumn;
                    chunk.localPosition = k;
                    chunk.globalPosition = new Vector3(
                        chunkColumn.globalPosition.x,
                        Chunk.size * k,
                        chunkColumn.globalPosition.z
                    );

                    for (int l = 0; l < Chunk.size; l++)
                    {
                        for (int m = 0; m < Chunk.size; m++)
                        {
                            for (int n = 0; n < Chunk.size; n++)
                            {

                                Block block;

                                Vector3 positionInChunk = new Vector3(l, m, n);
                                Vector3 position = positionInChunk + chunk.globalPosition;
                                int maxHeight = 50 + (int)(50 * Mathf.PerlinNoise(position.x / worldMagnitude, position.z / worldMagnitude));
                                float magnitude = PerlinNoise3d.Make(position / (worldMagnitude * smoothness));

                                block = BlockDecider(magnitude, position.y, maxHeight);

                                block.chunk = chunk;
                                block.positionInChunk = positionInChunk;
                                block.globalPosition = position;
                                chunk.blocks[l, m, n] = block;
                            }
                        }
                    }

                    chunkColumn.chunks[k] = chunk;
                }
                chunkColumns[i, j] = chunkColumn;
               
            }
        }
    }

    public Block GetBlockAtGlobalPosition(Vector3 position)
    {
        // position of block inside chunk
        int u_b, v_b, w_b;

        // global position of chunk
        int x_c, y_c, z_c;

        // postion of chunk inside chunk column
        int p_c;

        // position of chunkColumn inside world
        int u_cc, v_cc;

        // global position of chunk column
        int x_cc, z_cc;

        // find position of block inside chunk
        u_b = (int) position.x % Chunk.size;
        v_b = (int) position.y % Chunk.size;
        w_b = (int) position.z % Chunk.size;

        // find global position of chunk
        x_c = (int) position.x - u_b;
        y_c = (int) position.y - v_b;
        z_c = (int) position.z - w_b;

        // find position of chunk inside chunk column
        p_c = y_c / Chunk.size;

        // find global position of chunk column; 
        x_cc = x_c;
        z_cc = z_c;

        // find local position of chunk column inside world
        u_cc = x_cc / Chunk.size;
        v_cc = z_cc / Chunk.size;

        return (Block) chunkColumns[u_cc, v_cc].chunks[p_c].blocks[u_b, v_b, w_b];
    }

    public Block BlockDecider(float magnitude, float height, int maxHeight)
    {
        // Magnitude is a normal distribution with sigma = 0.09 and mu = 0.47
        Block block;
       
        // is bedrock
        if (height == 0)
        {
            return new Bedrock();
        }

        // is air
        if (height > maxHeight)
        {
            return new Air();
        }

        block = new Stone();

        // work through height levels
        if (height < 10)
        {
            if (magnitude <= 0.38)
            {
                return new Air();
            }

            if (magnitude <= 0.4)
            {
                return new Stone();
            }

            if (magnitude <= 0.42)
            {
                return new RedstoneOre();
            }

            if (magnitude <= 0.43)
            {
                return new Stone();
            }

            if (magnitude <= 0.44)
            {
                return new DiamondOre();
            }

            if (magnitude <= 0.45)
            {
                return new Stone();
            }

            if (magnitude <= 0.46)
            {
                return new LapisLazuli();
            }

            if (magnitude <= 0.47)
            {
                return new Stone();
            }

            if (magnitude <= 0.48)
            {
                return new CoalOre();
            }

            if (magnitude <= 0.9)
            {
                return new Stone();
            }

            if (magnitude <= 0.5)
            {
                return new IronOre();
            }

            if (magnitude <= 0.51)
            {
                return new Stone();
            }

            if (magnitude <= 0.52)
            {
                return new GoldOre();
            }

            if (magnitude <= 0.525)
            {
                return new Stone();
            }

            if (magnitude <= 0.53)
            {
                return new Obsidian();
            }

            return new Stone();
        }

        // air, coal, iron. stone, grass, dirt
        if (height > maxHeight - 10)
        {
            if (magnitude <= 0.2 || magnitude >= 0.8)
            {
                return new Air();
            }

            if (magnitude <= 0.225)
            {
                return new Stone();
            }

            if (magnitude <= 0.25)
            {
                return new CoalOre();
            }

            if (magnitude <= 0.5)
            {
                return new Stone();
            }

            if (magnitude >= 0.75)
            {
                return new IronOre();
            }


            if (height == maxHeight)
            {
                return new Grass();
            }

            return new Dirt();
        }

        // the rest
        if (magnitude <= 0.38)
        {
            return new Air();
        }

        if (magnitude <= 0.39)
        {
            return new Stone();
        }

        if (magnitude <= 0.40)
        {
            return new CoalOre();
        }

        if (magnitude <= 0.41)
        {
            return new Stone();
        }

        if (magnitude <= 0.42)
        {
            return new IronOre();
        }

        if (magnitude <= 0.425)
        {
            return new Stone();
        }

        if (magnitude <= 0.43)
        {
            return new GoldOre();
        }

        return new Stone();
    }
}
