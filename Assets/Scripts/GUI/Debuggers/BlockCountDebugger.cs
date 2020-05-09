using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Chunks;
using Game.Blocks;
using System.Globalization;

namespace Game.GUI.Debuggers
{
    public class BlockCountDebugger : AbstractDebugger
    {
        public string label = "";

        Dictionary<string, int> blockCounts;

        public BlockCountDebugger()
        {
            blockCounts = new Dictionary<string, int>();
        }

        public override void Draw(Vector2 position)
        {
            if (label.Length == 0)
            {
                WorldBuilder world = GetWorldBuilder();
                int total = 0;

                // count blocks
                for (int i = 0; i < world.Size.x; i++)
                {
                    for (int j = 0; j < world.Size.y; j++)
                    {
                        for (int k = 0; k < ChunkColumn.chunksPerColumn; k++)
                        {
                            if (world.chunkColumns[i, j].chunks[k] != null)
                            {
                                for (int l = 0; l < Chunk.size; l++)
                                {
                                    for (int m = 0; m < Chunk.size; m++)
                                    {
                                        for (int n = 0; n < Chunk.size; n++)
                                        {
                                            Block block = world.chunkColumns[i, j].chunks[k].blocks[l, m, n];
                                            string type = block.GetType().ToString();

                                            if (!blockCounts.ContainsKey(type))
                                            {
                                                blockCounts[type] = 0;
                                            }

                                            total++;
                                            blockCounts[type]++;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                int totalSolid = total - blockCounts[typeof(Air).ToString()];

                foreach (KeyValuePair<string, int> blockCount in blockCounts)
                {
                    if (blockCount.Key != typeof(Air).ToString())
                    {
                        label += blockCount.Key + ": " + blockCount.Value.ToString("#,##") + " (" + ((float)blockCount.Value / (float)totalSolid).ToString("P", CultureInfo.InvariantCulture) + ")" + "\n";
                    }
                }

                label += "Total: " + totalSolid.ToString("#,##");
            }

            content.text = label;
            style.CalcSize(content);
            UnityEngine.GUI.Label(new Rect(position.x, position.y + 40, 20, 20), label, style);        
        }

        public WorldBuilder GetWorldBuilder()
        {
            return Game.currentGame.world;
        }
    }
}
