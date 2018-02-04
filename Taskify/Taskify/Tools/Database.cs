using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Tools
{
    public class Database
    {
        private OleDbConnection conn;

        public Database(String DBPath)
        {
            this.Conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0;"  + "Data Source=" + DBPath);
            this.Conn.Open();
        }

        public OleDbConnection Conn { get => conn; set => conn = value; }

        public bool Insert(String table, Dictionary<String, String> data)
        {
            String cols = "";
            String vals = "";

            bool first = true;
            foreach(var pair in data)
            {
                if (first)
                {
                    cols += "(" + pair.Key;
                    vals += "(" + pair.Value;
                    first = false;
                }
                else
                {
                    cols += ", " + pair.Key;
                    vals += ", " + pair.Value;
                }
            }

            cols += ")";
            vals += ")";

            String query = "INSERT INTO " + table + " " + cols + " VALUES " + vals;
            Console.WriteLine(query);

            using (OleDbCommand cmd = new OleDbCommand(query, this.Conn))
            {
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public OleDbDataReader Select(String table, String queryData, String additionalSearch)
        {
            OleDbCommand select = new OleDbCommand
            {
                Connection = conn,
                CommandText = "Select " + queryData + " From " + table + ((additionalSearch.Count() == 0)? "": " " + additionalSearch)
            };
            OleDbDataReader reader = select.ExecuteReader();
            return reader;
        }

        public void Update(String table, Dictionary<String, String> queryData, String additionalSearch)
        {
            String values = "";
            bool first = true;
            foreach (var col in queryData)
            {
                if (!first)
                    values += ", ";
                values += col.Key + "=" + col.Value;
                if (first)
                    first = false;
            }
            String query = "UPDATE " + table + " SET " + values + additionalSearch;
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
