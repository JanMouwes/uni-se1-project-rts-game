using kbs2.GamePackage;
using kbs2.World.Enums;
using kbs2.World.Structs;
using kbs2.World.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.World.Cell
{
    class WorldCellFactory
    {
        private static FastNoise myNoise = new FastNoise(WorldModel.seed);

        /// <summary>
        /// Generates a new chunk with new cells
        /// </summary>
        /// <param name="coords">The required chunk coordinates for setting the new chunks coordinates</param>
        /// <returns>returns a wordchunk filled with worldcells</returns>
        public static WorldCellController GetNewCell(FloatCoords coords) => new WorldCellController(coords, GetTypeFromCoords(coords.x, coords.y));

        /// <summary>
        /// This function generates the terrain type from its coords with noise
        /// </summary>
        /// <param name="x">Cell x position in float</param>
        /// <param name="y">Cell y position in float</param>
        /// <returns>returns the specific cell's terrain type</returns>
        public static TerrainType GetTypeFromCoords(float x, float y)
        {
            myNoise.SetNoiseType(WorldFactory.Noise); // Set the desired noise type
            float currentNoise = myNoise.GetNoise(x, y);

            TerrainType textureType;
            if (currentNoise < -0.10)
            {
                textureType = TerrainType.Water;
            }
            else if (currentNoise < 0)
            {
                textureType = TerrainType.Sand;
            }
            else if (currentNoise < 0.15)
            {
                textureType = TerrainType.Soil;
            }
            else if (currentNoise < 0.30)
            {
                textureType = TerrainType.Grass;
            }
            else if (currentNoise < 0.40)
            {
                textureType = TerrainType.Trees;
            }
            else if (currentNoise < 0.50)
            {
                textureType = TerrainType.Grass;
            }
            else if (currentNoise < 0.60)
            {
                textureType = TerrainType.Rock;
            }
            else
            {
                textureType = TerrainType.Snow;
            }

            return textureType;
        }
    }
}
