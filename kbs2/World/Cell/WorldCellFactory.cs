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
        private static FastNoise myNoise = new FastNoise();

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
            else if(currentNoise < 0)
            {
                textureType = TerrainType.Soil;
            }
            else if(currentNoise < 0.5)
            {
                textureType = (currentNoise < 0.45 && currentNoise > 0.35 && new Random().Next(1, 5) > 2)? TerrainType.Trees: TerrainType.Grass;
            }
            else if(currentNoise < 0.70)
            {
                textureType = TerrainType.Rock;
            }
            else
            {
                textureType = TerrainType.Snow;
            }

            
            
            string texture = TerrainDef.TerrainDef.TerrainDictionary[textureType];
            return new WorldCellController(coords, texture);
        }
    }
}
