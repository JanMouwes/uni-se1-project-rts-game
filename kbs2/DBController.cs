using System;
using System.Collections.Generic;
using System.IO;
using kbs2.Unit.Model;
using kbs2.Unit.Unit;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Health;
using Mono.Data.Sqlite;

namespace kbs2
{
    public static class DBController
    {
        public static SqliteConnection DBConn { get; set; }

        // Open a connection with the given database file
        public static SqliteConnection OpenConnection(string databaseFileName)
        {
            if (DBConn != null && DBConn.State == System.Data.ConnectionState.Open)
                return DBConn;
            

            //TODO: Tempfix so we can continue copied DefDex.db to bin/DesktopGL/AnyCPU/debug/

            string directoryName = Path.GetFullPath(databaseFileName);
            // gives /Users/Username/Github/Project/bin/DesktopGL/AnyCPU/debug/Defdex.db
            // Should give /Users/Username/Github/Project/DefDex.db

            DBConn = new SqliteConnection(
                "Data Source= " + directoryName + "; Version=3;");
            DBConn.Open();

            return DBConn;
        }

        // Close the current connection with the database
        public static void CloseConnection()
        {
            DBConn.Close();
        }

        // get the Def from a given building
        public static BuildingDef GetDefinitionBuilding(int building)
        {
            BuildingDef BuildingDef = new BuildingDef
            {
                HPDef = new HPDef()
            };

            string query =
                "SELECT * FROM BuildingDef WHERE Id=@i";

            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", building));

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BuildingDef.HPDef.CurrentHP = int.Parse(reader["CurrentHp"].ToString());
                        BuildingDef.HPDef.MaxHP = int.Parse(reader["MaxHp"].ToString());

                        BuildingDef.width = float.Parse(reader["width"].ToString());
                        BuildingDef.height = float.Parse(reader["height"].ToString());

                        BuildingDef.imageSrc = reader["image"].ToString();
                        BuildingDef.AddShapeFromString(reader["shape"].ToString());
                    }
                }
            }

            return BuildingDef;
        }


        // Get the Def from the given unit
        public static UnitDef GetDefinitionFromUnit(int unit)
        {
            UnitDef returnedUnitDef = new UnitDef();

            string query = $"SELECT * FROM UnitDef WHERE Id=@i";


            using (SqliteCommand cmd = new SqliteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SqliteParameter("@i", unit));

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnedUnitDef.Speed = float.Parse(reader["Speed"].ToString());
                        returnedUnitDef.Image = reader["Image"].ToString();
                        returnedUnitDef.Width = float.Parse(reader["Width"].ToString());
                        returnedUnitDef.Height = float.Parse(reader["Height"].ToString());

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