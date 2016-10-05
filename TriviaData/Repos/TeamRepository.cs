using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using System.Data.SQLite;

namespace TriviaData.Repos
{
    public class TeamRepository : ITeamRepository
    {
        private SQLiteConnection _dbConn;

        public TeamRepository(SQLiteConnection dbConn)
        {
            _dbConn = dbConn;
        }

        public void Add()
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO Teams (person_id_list, name, year, created_at) VALUES (\'\', \'\', 0, {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void Add(string name)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO Teams (person_id_list, name, year, created_at) VALUES (\'\', {name}, 0, {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void Add(List<Person> members)
        {
            string dateString = DateTime.Now.ToString();
            string[] memberIdList = new string[members.Count];
            for (int i = 0; i < memberIdList.Length; i++)
            {
                memberIdList[i] = members[i].Id.ToString();
            }
            string sql = $"INSERT INTO Teams (person_id_list, name, year created_at) VALUES ({memberIdList}, \'\', 0, {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public Team GetTeamById(long id)
        {
            string sql = $"SELECT * FROM Teams WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            Team t = new Team();
            t.Id = id;
            t.Name = (string)reader["name"];
            t.Year = (long)reader["year"];
            t.Members = new List<Person>();
            // Queries the reader for list of person ids, separated by comma; splits into individual strings, then converts each one to an int.
            string dbMemberIds = (string)reader["person_id_list"];
            if (dbMemberIds != "")
            {
                long[] memberIds = Array.ConvertAll(((string)reader["person_id_list"]).Split('-'), x => long.Parse(x));

                PersonRepository personRepo = new PersonRepository(_dbConn);
                foreach (var i in memberIds)
                {
                    Person p = personRepo.GetPersonById(i);
                    t.Members.Add(p);
                }
            }
            
            string dateString = (string)reader["created_at"];
            t.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
            command.Dispose();
            return t;
        }

        public Team GetTeamByName(string name)
        {
            string sql = $"SELECT * FROM Teams WHERE name={name}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            Team t = new Team();
            t.Id = (long)reader["id"];
            t.Name = (string)reader["name"];
            t.Year = (long)reader["year"];
            t.Members = new List<Person>();
            // Queries the reader for list of person ids, separated by comma; splits into individual strings, then converts each one to an int.
            string dbMemberIds = (string)reader["person_id_list"];
            if (dbMemberIds != "")
            {
                long[] memberIds = Array.ConvertAll(dbMemberIds.Split('-'), x => long.Parse(x));
                PersonRepository personRepo = new PersonRepository(_dbConn);
                foreach (var i in memberIds)
                {
                    Person p = personRepo.GetPersonById(i);
                    t.Members.Add(p);
                }
            }            
            string dateString = (string)reader["created_at"];
            t.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
            command.Dispose();
            return t;
        }
        public List<Team> FindTeamsByYear(long year)
        {
            List<Team> teamsByYear = new List<Team>();

            string sql = $"SELECT * FROM Teams WHERE year={year}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if ((long)reader["year"] == year) teamsByYear.Add(GetTeamById((long)reader["id"]));
            }
            command.Dispose();
            return teamsByYear;
        }

        public void Remove(Team team)
        {
            long id = team.Id;
            string sql = $"DELETE FROM Teams WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void Update(Team team)
        {
            string[] memberIdList = new string[team.Members.Count];
            for (int i = 0; i < memberIdList.Length; i++)
            {
                memberIdList[i] = team.Members[i].Id.ToString();
            }
            string dateString = team.CreatedAt.Ticks.ToString();
            string sql = $"UPDATE Teams SET name=\'{team.Name}\', year={team.Year}, person_id_list=\'{string.Join("-", memberIdList)}\', created_at={dateString} WHERE id={team.Id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();
            command.Dispose();
        }
    }
}
