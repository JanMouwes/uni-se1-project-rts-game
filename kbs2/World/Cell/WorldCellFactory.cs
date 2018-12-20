using kbs2.World.Enums;
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
        public static WorldCellController GetNewCell(FloatCoords coords) => new WorldCellController(coords, GetTypeFromCoords(coords.x, coords.y, FastNoise.NoiseType.Simplex));

        /// <summary>
        /// This function generates the terrain type from its coords with noise
        /// </summary>
        /// <param name="x">Cell x position in float</param>
        /// <param name="y">Cell y position in float</param>
        /// <param name="noise">Desired noise type</param>
        /// <returns>returns the specific cell's terrain type</returns>
        public static TerrainType GetTypeFromCoords(float x, float y, FastNoise.NoiseType noise)
        {
            myNoise.SetNoiseType(noise); // Set the desired noise type
            float currentNoise = myNoise.GetNoise(x, y);

            TerrainType textureType;
            if (currentNoise < -0.3)
            {
                textureType = TerrainType.Water;
            }
            else if (currentNoise < 0)
            {
                textureType = TerrainType.Sand;
            }
            else if (currentNoise < 0.2)
            {
                textureType = TerrainType.Soil;
            }
            else if (currentNoise < 0.35)
            {
                textureType = TerrainType.Grass;
            }
            else if (currentNoise < 0.45)
            {
                textureType = TerrainType.Trees;
            }
            else if (currentNoise < 0.55)
            {
                textureType = TerrainType.Grass;
            }
            else if (currentNoise < 0.65)
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
