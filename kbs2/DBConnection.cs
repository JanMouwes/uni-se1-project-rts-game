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
    }
}
