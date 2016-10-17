using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using System.Data.SQLite;
using System.Collections.ObjectModel;

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

        public void Add(Team team)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string members;
            if (team.Members == null) { members = "\'\'"; }
            else
            {
                string[] personIdList = new string[team.Members.Count];
                for (int i = 0; i < personIdList.Length; i++)
                {
                    personIdList[i] = team.Members[i].Id.ToString();
                }
                if (team.Members.Count == 0)
                {
                    members = "\'\'";
                }
                else
                {
                    members = string.Join("-", personIdList);
                }
            }
            string sql = $"INSERT INTO Teams (person_id_list, name, year, created_at) VALUES ({members}, \'{team.Name}\', {team.Year}, {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public void Add(string name)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO Teams (person_id_list, name, year, created_at) VALUES (\'\', \'{name}\', 0, {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void Add(List<Person> members)
        {
            string dateString = DateTime.Now.Ticks.ToString();
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
            // Queries the reader for list of person ids, separated by comma; splits into individual strings, then converts each one to a long.
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
            // Queries the reader for list of person ids, separated by comma; splits into individual strings, then converts each one to a long.
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

        public Team GetTeamByCompany(string companyName)
        {
            string sql = $"SELECT * FROM Teams WHERE company={companyName}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            Team t = new Team();
            t.Id = (long)reader["id"];
            t.Name = (string)reader["name"];
            t.Company = companyName;
            t.Year = (long)reader["year"];
            t.Members = new List<Person>();
            // Queries the reader for list of person ids, separated by comma; splits into individual strings, then converts each one to a long.
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
            t.CreatedAt = new DateTime(long.Parse(dateString));
            command.Dispose();
            return t;
        }

        public Team GetTeamByCompanyNoPlayers(string companyName)
        {
            string sql = $"SELECT * FROM Teams WHERE company={companyName}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            Team t = new Team();
            t.Id = (long)reader["id"];
            t.Name = (string)reader["name"];
            t.Year = (long)reader["year"];
            t.Company = companyName;
            t.Members = new List<Person>();
            string dateString = (string)reader["created_at"];
            t.CreatedAt = new DateTime(long.Parse(dateString));

            command.Dispose();
            return t;
        }

        /// <summary>
        /// GetAllTeams does NOT construct Player lists for the sake of efficiency -- cannot foresee any use case for this method that would require it to do so.
        /// </summary>
        /// <returns></returns>
        public List<Team> GetAllTeams()
        {
            List<Team> teamsList = new List<Team>();
            string sql = $"SELECT * FROM Teams";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();
            PersonRepository personRepo = new PersonRepository(_dbConn);

            while (reader.Read())
            {
                Team t = new Team();
                t.Id = (long)reader["id"];
                t.Name = (string)reader["name"];
                t.Year = (long)reader["year"];
                t.Members = new List<Person>();
                t.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
                teamsList.Add(t);
            }
            command.Dispose();
            return teamsList;
        }

        /// <summary>
        /// This method retrieves a team from the database but does not make calls to the People table. This will be a more efficient solution
        /// during most use cases as very few uses will require specific team members to be displayed (for example, scoring itself). A more
        /// robust solution might encapsulate a call to the regular GetTeam method and execute it the first time Team info is requested by the client...
        /// but at this stage, I'd rather be notified when the app is trying to make a DB transaction to get Person info that I don't need.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Team object with an empty Person list.</returns>
        public Team GetTeamByIdNoPlayers(long id)
        {
            string sql = $"SELECT * FROM Teams WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            Team t = new Team();
            t.Id = id;
            t.Name = (string)reader["name"];
            t.Year = (long)reader["year"];
            t.Members = new List<Person>();
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
                teamsByYear.Add(GetTeamById((long)reader["id"]));
            }
            command.Dispose();
            return teamsByYear;
        }

        /// <summary>
        /// Operates similarly to GetTeamByIdNoPlayers, but for year searching. Could be useful for GUI data controls -- organizing teams by year, etc.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Team> FindTeamsByYearNoPlayers(long year)
        {
            List<Team> teamsByYear = new List<Team>();

            string sql = $"SELECT * FROM Teams WHERE year={year}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                teamsByYear.Add(GetTeamByIdNoPlayers((long)reader["id"]));
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
