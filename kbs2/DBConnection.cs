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

        public void OpenConnection(string dbName)
        {
            DBConn = new SQLiteConnection($"Data Source={dbName}.sqlite; Version=3;");
            DBConn.Open();
        }

        public void CloseConnection()
        {
            DBConn.Close();
        }

        public UnitDef GetDefaultFromUnit(int unit)
        {
            UnitDef returnedUnitDef = new UnitDef();

            string query =
                "SELECT * " +
                "FROM UnitDef " +
                "WHERE Id=@i " +
                "JOIN BattleDef ON Id = BattleDef.UnitDefId " +
                "JOIN HPDef ON Id = HPDef.UnitDefId " +
                "JOIN LevelXPDef ON Id = LevelXPDef.UnitDefID";

            using (SQLiteCommand cmd = new SQLiteCommand(query, DBConn))
            {
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
    }
}
