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
        public void Add()
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO Teams (name, year, company, num_scorers, created_at) VALUES (\'\', 0, \'\', 0, {dateString})";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
                _dbConn.Close();
            }
        }

        public void Add(Team team)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string scorerIds = " ";
            string sql = $"INSERT INTO Teams (name, year, company, num_scorers, created_at) VALUES (\'{team.Name}\', {team.Year}, \'{team.Company}\', \'{team.NumScorers}\', {dateString})";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
                _dbConn.Close();
            }
        }

        public void Add(string name)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO Teams (name, year, company, num_scorers, created_at) VALUES (\'{name}\', 0, \'\', 0, {dateString})";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
                _dbConn.Close();
            }
        }

        public Team GetTeamById(long id)
        {
            Team t = new Team();
            string sql = $"SELECT * FROM Teams WHERE id={id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                SQLiteDataReader reader = command.ExecuteReader();

                t.Id = id;
                t.Name = (string)reader["name"];
                t.Year = (long)reader["year"];
                t.Company = (string)reader["company"];
                t.NumScorers = (long)reader["num_scorers"];

                if (t.NumScorers > 0)
                {
                    t.HasScorer = true;
                }
                else
                {
                    t.HasScorer = false;
                }

                t.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
                command.Dispose();
                _dbConn.Close();
            }
            return t;
        }

        public Team GetTeamByName(string name)
        {
            Team t = new Team();
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                string sql = $"SELECT * FROM Teams WHERE name={name}";
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                t.Id = (long)reader["id"];
                t.Name = (string)reader["name"];
                t.Year = (long)reader["year"];
                t.Company = (string)reader["company"];
                t.NumScorers = (long)reader["num_scorers"];
                if (t.NumScorers > 0)
                {
                    t.HasScorer = true;
                }
                else
                {
                    t.HasScorer = false;
                }
                t.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
                command.Dispose();
                _dbConn.Close();
            }
            return t;
        }

        public Team GetTeamByCompany(string companyName)
        {
            string sql = $"SELECT * FROM Teams WHERE company={companyName}";
            Team t = new Team();
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                t.Id = (long)reader["id"];
                t.Name = (string)reader["name"];
                t.Company = companyName;
                t.NumScorers = (long)reader["num_scorers"];
                if (t.NumScorers > 0)
                {
                    t.HasScorer = true;
                }
                else
                {
                    t.HasScorer = false;
                }
                t.Year = (long)reader["year"];
                string dateString = (string)reader["created_at"];
                t.CreatedAt = new DateTime(long.Parse(dateString));
                command.Dispose();
                _dbConn.Close();
            }
            return t;
        }

        public List<Team> GetAllTeams()
        {
            List<Team> teamsList = new List<Team>();
            string sql = $"SELECT * FROM Teams";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Team t = new Team();
                    t.Id = (long)reader["id"];
                    t.Name = (string)reader["name"];
                    t.Year = (long)reader["year"];
                    t.Company = (string)reader["company"];
                    t.NumScorers = (long)reader["num_scorers"];
                    if (t.NumScorers > 0)
                    {
                        t.HasScorer = true;
                    }
                    else
                    {
                        t.HasScorer = false;
                    }
                    t.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
                    teamsList.Add(t);
                }
                command.Dispose();
                _dbConn.Close();
            }
            return teamsList;
        }

        public List<Team> FindTeamsByYear(long year)
        {
            List<Team> teamsByYear = new List<Team>();
            string sql = $"SELECT * FROM Teams WHERE year={year}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    teamsByYear.Add(GetTeamById((long)reader["id"]));
                }
                command.Dispose();
                _dbConn.Close();
            }
            return teamsByYear;
        }

        public void Remove(Team team)
        {
            if (team.HasScorer)
            {
                ScorerRepository scorerRepo = new ScorerRepository();
                var scorers = scorerRepo.GetAllScorers();
                List<Scorer> scorersToUpdate = new List<Scorer>();
                foreach (var s in scorers)
                {
                    if (s.Teams.Contains(team))
                    {
                        s.Teams.Remove(team);
                        scorersToUpdate.Add(s);
                    }
                }
                foreach (var toUpdate in scorersToUpdate) scorerRepo.Update(toUpdate);
            }
            long id = team.Id;
            string sql = $"DELETE FROM Teams WHERE id={id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
                _dbConn.Close();
            }
        }

        public void Update(Team team)
        {
            string dateString = team.CreatedAt.Ticks.ToString();
            string sql = $"UPDATE Teams SET name=\'{team.Name}\', year={team.Year}, company=\'{team.Company}\', num_scorers=\'{team.NumScorers}\', created_at={dateString} WHERE id={team.Id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection);
                command.ExecuteNonQuery();
                command.Dispose();
                _dbConn.Close();
            }
        }
    }
}
