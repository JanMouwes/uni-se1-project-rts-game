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
            myNoise.SetNoiseType(FastNoise.NoiseType.Perlin); // Set the desired noise type

            TerrainType textureType = (myNoise.GetNoise(coords.x, coords.y) > 0) ? TerrainType.Grass: TerrainType.Water;
            string texture = TerrainDef.TerrainDef.TerrainDictionary[textureType];
            return new WorldCellController(coords, texture);
        }
    }
}
