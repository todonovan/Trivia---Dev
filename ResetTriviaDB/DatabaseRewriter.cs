using System;
using System.Data.SQLite;

namespace ResetTriviaDB
{
    public class DatabaseRewriter
    {
        private string _path;

        public DatabaseRewriter(string p)
        {
            _path = p;
        }

        public void RewriteDatabase()
        {
            SQLiteConnection dbConn = StartNewDatabaseFile();
            dbConn.Open();
            string sql;
            string drop;
            SQLiteCommand command;

           
            // Scorers

            drop = "DROP TABLE IF EXISTS Scorers";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Scorers...");
            sql = "CREATE TABLE \"Scorers\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, \"name\" VARCHAR(125), \"team_id_list\" VARCHAR(125), \"created_at\" VARCHAR(125))";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            // Teams

            drop = "DROP TABLE IF EXISTS Teams";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Teams...");
            sql = "CREATE TABLE \"Teams\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , \"name\" VARCHAR(125) UNIQUE, \"year\" INTEGER, \"company\" VARCHAR(125), \"num_scorers\" INTEGER, \"created_at\" VARCHAR(125))";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();


            // Wrap up

            dbConn.Close();
            command.Dispose();
        }

        private SQLiteConnection StartNewDatabaseFile()
        {
            SQLiteConnection.CreateFile($"{_path}\\trivia.sqlite");

            SQLiteConnection dbConn = new SQLiteConnection($"Data Source={_path}\\trivia.sqlite;Version=3");
            return dbConn;
        }
    }
}
