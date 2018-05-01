using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; 
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows; 



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

        /*
       // Test code for querying database 
        private static string sqlSelect = "select * from Users;";
        private static SQLiteCommand command = new SQLiteCommand(sqlSelect, sql_con);
        MessageBox.Show("This is a test"); 
        */

        static void main()
        {
            //string sqlSelect = "select * from Users;";
            string sqlCreate = "create table testingTable (test1 text, test2 text);";
            //SQLiteCommand command = new SQLiteCommand(sqlSelect, sql_con);
            SQLiteCommand testCmd = new SQLiteCommand(sqlCreate, sql_con); 
            //MessageBox.Show(sqlSelect, "Output"); 
            

        }
        
        
    }

    
}
