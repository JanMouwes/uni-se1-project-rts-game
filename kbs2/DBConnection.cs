using kbs2.Faction.FactionMVC;
using kbs2.Unit.Model;
using kbs2.Unit.Unit;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2
{
    class DBConnection
    {
        public SQLiteConnection DBConn { get; set; }

        // Open a connection with the given database file
        public void OpenConnection(string dbName)
        {
            DBConn = new SQLiteConnection($"Data Source={dbName}.sqlite; Version=3;");
            DBConn.Open();
        }

        // Close the current connection with the database
        public void CloseConnection()
        {
            DBConn.Close();
        }

        // Get the Def(ault) from the given unit
        public UnitDef GetDefaultFromUnit(int unit)
        {
            UnitDef returnedUnitDef = new UnitDef();

            string query =
                "SELECT * " +
                "FROM UnitDef " +
                "WHERE Id=@i " + // CHANGE to Name or Id
                "JOIN BattleDef ON Id = BattleDef.UnitDefId " +
                "JOIN HPDef ON Id = HPDef.UnitDefId " +
                "JOIN LevelXPDef ON Id = LevelXPDef.UnitDefID";

            using (SQLiteCommand cmd = new SQLiteCommand(query, DBConn))
            {
                cmd.Parameters.Add(new SQLiteParameter("@i", unit));

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnedUnitDef.Speed = (float) reader["Speed"];

                        returnedUnitDef.BattleDef.AttackModifier = (double)reader["BattleDef.AttackModifier"];
                        returnedUnitDef.BattleDef.DefenseModifier = (double)reader["BattleDef.DefenseModifier"];
                        returnedUnitDef.BattleDef.Accuracy = (double)reader["BattleDef.Accuracy"];
                        returnedUnitDef.BattleDef.DodgeChance = (double)reader["BattleDef.DodgeChance"];
                        returnedUnitDef.BattleDef.RangeModifier = (double)reader["BattleDef.RangeModifier"];

                        returnedUnitDef.HPDef.CurrentHP = (int)reader["HPDef.CurrentHP"];
                        returnedUnitDef.HPDef.MaxHP = (int)reader["HPDef.MaxHP"];

                        returnedUnitDef.LevelXPDef.Level = (int)reader["LevelXPDef.Level"];
                        returnedUnitDef.LevelXPDef.XP = (int)reader["LevelXPDef.XP"];
                        returnedUnitDef.LevelXPDef.XPNeed = (int)reader["LevelXPDef.XPNeed"];
                    }
                }
            }

            return returnedUnitDef;
        }
        // Retrieves all units assigned to the given faction    CHANGE int factionName to string factionName if easier
        public List<Unit_Model> GetUnitsFromFaction(int factionName)
        {
            // Creates a Unit_Model list to store all the retrieved units
            List<Unit_Model> units = new List<Unit_Model>();
            
            // Query to select all units from the given faction
            string query = 
                "SELECT * " +
                "FROM Faction_Unit " +
                "WHERE Faction_Unit.Faction_Id = @name " + // CHANGE @name to @id
                "JOIN Unit ON Factoin_Unit.UnitId = Unit.Id " +
                "JOIN UnitLocation ON UnitLocation.UnitId = Faction_Unit.UnitId";

            using(SQLiteCommand cmd = new SQLiteCommand(query, DBConn))
            {
                // Add the faction's name parameter
                cmd.Parameters.Add(new SQLiteParameter("@name", factionName));

                using(SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Create and insert values into a new Unit_Model and add the Unit_Model to the Unit_Model list afterwards
                        Unit_Model unit = new Unit_Model();

                        unit.BattleModel.Accuracy = (double)reader["Unit.Accuracy"];
                        unit.BattleModel.AttackModifier = (double)reader["Unit.AttackModifier"];
                        unit.BattleModel.DefenseModifier = (double)reader["Unit.DefenseModifier"];
                        unit.BattleModel.DodgeChance = (double)reader["Unit.DodgeChance"];
                        unit.BattleModel.RangeModifier = (double)reader["Unit.RangeModifier"];

                        unit.HPModel.CurrentHP = (int)reader["Unit.CurrentHP"];
                        unit.HPModel.MaxHP = (int)reader["Unit.MaxHP"];

                        unit.XPModel.LvlModel.Level = (int)reader["Unit.Level"];
                        unit.XPModel.LvlModel.XPNeed = (int)reader["Unit.XPNeed"];
                        unit.XPModel.XP = (int)reader["Unit.XP"];

                        unit.LocationModel.floatCoords.x = (float)reader["UnitLocation.FloatCoordX"];
                        unit.LocationModel.floatCoords.y = (float)reader["UnitLocation.FloatCoordY"];

                        units.Add(unit);
                    } 
                }
            }

            // Return the Unit_Model list
            return units;
        }
    }
}
