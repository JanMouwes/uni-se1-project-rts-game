using System.Data.Entity;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using kbs2.World.World;
using kbs2.WorldEntity.Structures;

namespace kbs2.GamePackage.GameSaveManager
{
    public class DataBaseContext : DbContext
    {
        public DbSet<WorldCellModel> save_world_cell;
        public DbSet<WorldChunkModel> save_world_chunk;
        public DbSet<WorldModel> save_world;
        public DbSet<GameModel> save_game;
        public DbSet<BuildingDef> BuildingDef;

        public DataBaseContext() : base("DefDex")
        {
        }
    }
}