using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; 
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows; 

/*
Although this class is currently unused, it has potential to be used later
for connecting and interacting with the database 
*/

namespace FeTool
{
    static class SQLiteDB
    {
        private static SQLiteConnection sql_con;
        private static SQLiteCommand sql_cmd;
        private static SQLiteDataAdapter DB;
        private static DataSet DS = new DataSet();
        private static DataTable DT = new DataTable();

        private static void SetConnection()
        {

            sql_con = new SQLiteConnection("Data Source=FE_Database.db;Version=3;New=False;Compress=True;");
        }

        public static void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

        public static DataTable Execute(string sql)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = sql;
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            sql_con.Close();

            return DT;
        }

        

        
        
        
    }

    
}
