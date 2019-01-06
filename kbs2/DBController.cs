using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using kbs2.Resources.Enums;
using kbs2.utils;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.Structures;
using kbs2.WorldEntity.Structures.Defs;
using kbs2.WorldEntity.Unit;
using Mono.Data.Sqlite;

namespace kbs2
{
    public class BuildingNotFoundException : DataNotFoundException
    {
        public BuildingNotFoundException(int buildingId) : base($"Building with id {buildingId}")
        {
        }
    }

    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string notFoundDescription) : base($"{notFoundDescription} not found")
        {
        }
    }

    public static class DBController
    {
        private static SqliteConnection DBConn { get; set; }

        // Open a connection with the given database file
        public static SqliteConnection OpenConnection(string databaseFileName)
        {
            if (DBConn != null && DBConn.State == System.Data.ConnectionState.Open)
                return DBConn;

            string directoryName = Path.GetFullPath(databaseFileName);

            DBConn = new SqliteConnection("Data Source= " + directoryName + "; Version=3;");
            DBConn.Open();

            return DBConn;
        }

        // Close the current connection with the database
        public static void CloseConnection()
        {
            DBConn.Close();
        }


        private static bool IsBuildingResourceFactory(int id)
        {
            DBConn = OpenConnection("DefDex.db");
            
            const string query = "SELECT * FROM Def_ResourceFactory WHERE id=@i";

            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", id));

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        private static bool IsStructureTrainingEntity(int id)
        {
            return IsWorldEntityTrainingEntity(id, 1);
        }

        private static bool IsUnitTrainingEntity(int id)
        {
            return IsWorldEntityTrainingEntity(id, 2);
        }

        private static bool IsWorldEntityTrainingEntity(int id, int worldEntityTypeId)
        {
            DBConn = OpenConnection("DefDex.db");
            
            const string query = "SELECT * FROM Def_TrainingEntity WHERE entity_id=@i AND entity_type_id = @j";

            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", id));
                cmd.Parameters.Add(new SqliteParameter("@j", worldEntityTypeId));

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        private static IEnumerable<ITrainableDef> GetTrainingEntityTrainables(int trainingEntityId)
        {
            DBConn = OpenConnection("DefDex.db");
            
            const string query = "SELECT unit_id FROM Def_TrainingEntity_Trainable WHERE training_entity_id = @i";

            List<ITrainableDef> returnList = new List<ITrainableDef>();

            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", trainingEntityId));


                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int unit_id = int.Parse(reader["unit_id"].ToString());

                        returnList.Add(GetUnitDef(unit_id));
                    }
                }
            }

            return returnList;
        }

        private static ResourceType GetResourceBuildingValues(int structureId)
        {
            DBConn = OpenConnection("DefDex.db");
            
            const string query = "SELECT Def_ResourceType.name as resource_type" +
                                 "FROM BuildingDef " +
                                 "JOIN Def_ResourceFactory ON BuildingDef.Id = Def_ResourceFactory.id " +
                                 "JOIN Def_ResourceType ON Def_ResourceFactory.resource_type_id = Def_ResourceType.id WHERE BuildingDef.Id = @i";
            ResourceType resourceType;

            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", structureId));


                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) throw new DataNotFoundException($"ResourceBuilding with id {structureId}");

                    ResourceFactoryDef resourceFactoryDef = new ResourceFactoryDef();
                    switch (reader["resource_type"].ToString().ToLower())
                    {
                        case "wood":
                            resourceType = ResourceType.Wood;
                            break;
                        case "stone":
                            resourceType = ResourceType.Stone;
                            break;
                        case "food":
                            resourceType = ResourceType.Food;
                            break;
                        default: throw new DataException($"Invalid ResourceType {reader["resource_type"]}");
                    }
                }
            }

            return resourceType;
        }

        public static BuildingDef GetBuildingDef(int buildingId) => GetBuildingDef<BuildingDef>(buildingId);

        //FIXME split into multiple methods, GetBuildingDef, GetResourceFactoryDef, etc.
        //FIXME this is horribly designed. Do the above.   
        // get the Def from a given building
        public static TStructureDef GetBuildingDef<TStructureDef>(int buildingId) where TStructureDef : BuildingDef
        {
            DBConn = OpenConnection("DefDex.db");
            
            string query = "SELECT * FROM BuildingDef WHERE Id=@i";

            bool isResourceFactory = IsBuildingResourceFactory(buildingId);

            BuildingDef buildingDef = new BuildingDef();

            if (isResourceFactory)
            {
                ResourceFactoryDef factoryDef = new ResourceFactoryDef
                {
                    ResourceType = GetResourceBuildingValues(buildingId)
                };
                buildingDef = factoryDef;
            }

            bool isTrainingStructure = IsStructureTrainingEntity(buildingId);

            if (isTrainingStructure)
            {
                TrainingStructureDef factoryDef = new TrainingStructureDef
                {
                    TrainableDefs = GetTrainingEntityTrainables(buildingId).ToList()
                };
                buildingDef = factoryDef;
            }

            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", buildingId));

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) throw new BuildingNotFoundException(buildingId);


                    //buildingDef.HPDef.CurrentHP = int.Parse(reader["CurrentHp"].ToString());
                    //buildingDef.HPDef.MaxHP = int.Parse(reader["MaxHp"].ToString());

                    ViewValues viewValues = new ViewValues(
                        reader["image"].ToString(),
                        float.Parse(reader["width"].ToString()),
                        float.Parse(reader["height"].ToString())
                    );

                    buildingDef.ViewValues = viewValues;

                    buildingDef.BuildingShape = BuildingShapeCalculator.GetShapeFromString(reader["shape"].ToString());

                    buildingDef.Cost = float.Parse(reader["Cost"].ToString());
                    buildingDef.UpkeepCost = float.Parse(reader["Upkeep"].ToString());

                    buildingDef.ViewRange = int.Parse(reader["ViewRange"].ToString());

                    buildingDef.ConstructionTime = uint.Parse(reader["ConstructionTime"].ToString());
                }
            }

            return (TStructureDef) buildingDef;
        }


        // Get the Def from the given unit
        public static UnitDef GetUnitDef(int unitId)
        {
            UnitDef returnedUnitDef = new UnitDef();

            string query = $"SELECT * FROM UnitDef WHERE Id=@i";


            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", unitId));

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnedUnitDef.Speed = float.Parse(reader["Speed"].ToString());
                        returnedUnitDef.Image = reader["Image"].ToString();
                        returnedUnitDef.Width = float.Parse(reader["Width"].ToString());
                        returnedUnitDef.Height = float.Parse(reader["Height"].ToString());
                        returnedUnitDef.Upkeep = float.Parse(reader["Upkeep"].ToString());
                        returnedUnitDef.PurchaseCost = float.Parse(reader["Cost"].ToString());
                        returnedUnitDef.ViewRange = int.Parse(reader["ViewRange"].ToString());

                        // This is for the different defs not implemented yet
                        /*returnedUnitDef.BattleDef.AttackModifier = double.Parse(reader["BattleDef.AttackModifier"].ToString());
                        returnedUnitDef.BattleDef.DefenseModifier = double.Parse(reader["BattleDef.DefenseModifier"].ToString());
                        returnedUnitDef.BattleDef.Accuracy = double.Parse(reader["BattleDef.Accuracy"].ToString());
                        returnedUnitDef.BattleDef.DodgeChance = double.Parse(reader["BattleDef.DodgeChance"].ToString());
                        returnedUnitDef.BattleDef.RangeModifier = double.Parse(reader["BattleDef.RangeModifier"].ToString());

                        returnedUnitDef.HPDef.CurrentHP = int.Parse(reader["HPDef.CurrentHP"].ToString());
                        returnedUnitDef.HPDef.MaxHP = int.Parse(reader["HPDef.MaxHP"].ToString());

                        returnedUnitDef.LevelXPDef.Level = int.Parse(reader["LevelXPDef.Level"].ToString());
                        returnedUnitDef.LevelXPDef.XP = int.Parse(reader["LevelXPDef.XP"].ToString());
                        returnedUnitDef.LevelXPDef.XPNeed = int.Parse(reader["LevelXPDef.XPNeed"].ToString());*/
                    }
                }
            }

            return returnedUnitDef;
        }

        //// Retrieves all units assigned to the given faction    CHANGE int factionName to string factionName if easier
        //public static List<Unit_Model> GetUnitsFromFaction(int factionName)
        //{
        //    // Creates a Unit_Model list to store all the retrieved units
        //    List<Unit_Model> units = new List<Unit_Model>();

        //    // Query to select all units from the given faction
        //    string query =
        //        "SELECT * " +
        //        "FROM Faction_Unit " +
        //        "WHERE Faction_Unit.Faction_Id = @name " + // CHANGE @name to @id
        //        "JOIN Unit ON Factoin_Unit.UnitId = Unit.Id " +
        //        "JOIN UnitLocation ON UnitLocation.UnitId = Faction_Unit.UnitId";

        //    using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
        //    {
        //        // Add the faction's name parameter
        //        cmd.Parameters.Add(new SqliteParameter("@name", factionName));

        //        using (SqliteDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                // Create and insert values into a new Unit_Model and add the Unit_Model to the Unit_Model list afterwards
        //                Unit_Model unit = new Unit_Model(2,2);

        //                unit.BattleModel.Accuracy = (double)reader["Unit.Accuracy"];
        //                unit.BattleModel.AttackModifier = (double)reader["Unit.AttackModifier"];
        //                unit.BattleModel.DefenseModifier = (double)reader["Unit.DefenseModifier"];
        //                unit.BattleModel.DodgeChance = (double)reader["Unit.DodgeChance"];
        //                unit.BattleModel.RangeModifier = (double)reader["Unit.RangeModifier"];

        //                unit.HPModel.CurrentHP = (int)reader["Unit.CurrentHP"];
        //                unit.HPModel.MaxHP = (int)reader["Unit.MaxHP"];

        //                unit.XPModel.LvlModel.Level = (int)reader["Unit.Level"];
        //                unit.XPModel.LvlModel.XPNeed = (int)reader["Unit.XPNeed"];
        //                unit.XPModel.XP = (int)reader["Unit.XP"];

        //                unit.LocationModel.floatCoords.x = (float)reader["UnitLocation.FloatCoordX"];
        //                unit.LocationModel.floatCoords.y = (float)reader["UnitLocation.FloatCoordY"];

        //                units.Add(unit);
        //            }
        //        }
        //    }

        //    // Return the Unit_Model list
        //    return units;
        //}
    }
}