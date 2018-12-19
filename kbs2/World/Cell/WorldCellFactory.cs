﻿using kbs2.World.Enums;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.Cell
{
    class WorldCellFactory
    {
        private static FastNoise myNoise = new FastNoise(new Random().Next(0, 50));

        /// <summary>
        /// Generates a new chunk with new cells
        /// </summary>
        /// <param name="coords">The required chunk coordinates for setting the new chunks coordinates</param>
        /// <returns>returns a wordchunk filled with worldcells</returns>
        public static WorldCellController GetNewCell(FloatCoords coords)
        {
            myNoise.SetNoiseType(FastNoise.NoiseType.Simplex); // Set the desired noise type
            float currentNoise = myNoise.GetNoise(coords.x, coords.y);

            TerrainType textureType;
            if (currentNoise < -0.45)
            {
                textureType = TerrainType.Water;
            }
            else if (currentNoise < -0.2)
            {
                textureType = TerrainType.Sand;
            }
            else if (currentNoise < 0)
            {
                textureType = TerrainType.Soil;
            }
            else if (currentNoise < 0.5)
            {
                double treeChance = ((currentNoise - 0.4) * 10) + 0.5;
                textureType = (currentNoise < 0.55 && currentNoise > 0.25 && treeChance > 0.5) ? TerrainType.Trees : TerrainType.Grass;
            }
            else if (currentNoise < 0.70)
            {
                textureType = TerrainType.Rock;
            }
            else
            {
                textureType = TerrainType.Snow;
            }

            return new WorldCellController(coords, textureType);
        }
    }
}
