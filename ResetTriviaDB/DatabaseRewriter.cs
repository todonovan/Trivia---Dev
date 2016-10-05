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

            
            // People

            drop = "DROP TABLE IF EXISTS People";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("People...");
            sql = "CREATE TABLE \"People\"(\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, \"full_name\" VARCHAR(125) UNIQUE, \"address\" TEXT, \"created_at\" VARCHAR(125))";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();
            /*
            // Questions

            drop = "DROP TABLE IF EXISTS Questions";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Questions..");
            sql = "CREATE TABLE \"Questions\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , \"round_id_list\" VARCHAR(125), \"question_text\" TEXT, \"answer_text\" TEXT, \"created_at\" VARCHAR(125))";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            // Rounds

            drop = "DROP TABLE IF EXISTS Rounds";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Rounds...");
            sql = "CREATE TABLE \"Rounds\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , \"year\" INTEGER, \"round_ordering_number\" INTEGER, \"num_questions\" INTEGER, \"question_id_list\" VARCHAR(125), \"point_value\" INTEGER, \"created_at\" VARCHAR(125))";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            // Scores

            drop = "DROP TABLE IF EXISTS Scores";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Scores...");
            sql = "CREATE TABLE \"Scores\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , \"value\" INTEGER, \"session_id\" INTEGER, \"team_id\" INTEGER, \"created_at\" VARCHAR(125))";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            // Sessions

            drop = "DROP TABLE IF EXISTS Sessions";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Sessions...");
            sql = "CREATE TABLE \"Sessions\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,\"team_id_list\" VARCHAR(125),\"round_id_list\" VARCHAR(125),\"score_id_list\" VARCHAR(125),\"created_at\" VARCHAR(125), \"year\" INTEGER)";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();
            */
            // Teams

            drop = "DROP TABLE IF EXISTS Teams";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Teams...");
            sql = "CREATE TABLE \"Teams\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , \"person_id_list\" VARCHAR(125), \"name\" VARCHAR(125) UNIQUE, \"year\" INTEGER, \"created_at\" VARCHAR(125))";

            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();

            /*
            // Team histories

            drop = "DROP TABLE IF EXISTS Team_histories";
            command = new SQLiteCommand(drop, dbConn);
            command.ExecuteNonQuery();

            Console.WriteLine("Team histories...");
            sql = "CREATE TABLE \"Team_histories\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , \"person_id\" INTEGER, \"team_id_list\" VARCHAR(125), \"created_at\" VARCHAR(125))";


            command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();
            */

            // Wrap up

            dbConn.Close();
            dbConn.Dispose();
            command.Dispose();

            Console.WriteLine("\nDone! Any key to continue...");
            Console.ReadLine();
        }

        private SQLiteConnection StartNewDatabaseFile()
        {
            SQLiteConnection.CreateFile($"{_path}\\trivia.sqlite");

            SQLiteConnection dbConn = new SQLiteConnection($"Data Source={_path}\\trivia.sqlite;Version=3");
            return dbConn;
        }
    }
}
