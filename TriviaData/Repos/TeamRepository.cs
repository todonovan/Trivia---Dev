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
                using (var command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Add(Team team)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string modifiedName = team.Name.Replace("\'", "\'\'");
            string sql = $"INSERT INTO Teams (name, year, company, num_scorers, created_at) VALUES (\'{modifiedName}\', {team.Year}, \'{team.Company}\', \'{team.NumScorers}\', {dateString})";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (var command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Add(string name)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string modifiedName = name.Replace("\'", "\'\'");
            string sql = $"INSERT INTO Teams (name, year, company, num_scorers, created_at) VALUES (\'{modifiedName}\', 0, \'\', 0, {dateString})";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (var command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public Team GetTeamById(long id)
        {
            Team t = new Team();
            string sql = $"SELECT * FROM Teams WHERE id={id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (var command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

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
                    }                        
                }
            }
            t.Name = t.Name.Replace("\'\'", "\'");
            return t;
        }

        public Team GetTeamByName(string name)
        {
            Team t = new Team();
            string modifiedName = name.Replace("\'", "\'\'");
            string sql = $"SELECT * FROM Teams WHERE name={modifiedName}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (var command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        t.Id = (long)reader["id"];
                        t.Name = name;
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
                    }                        
                }
            }
            return t;
        }

        public Team GetTeamByCompany(string companyName)
        {
            Team t = new Team();
            string sql = $"SELECT * FROM Teams WHERE company={companyName}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (var command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

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
                    }                        
                }
            }
            t.Name = t.Name.Replace("\'\'", "\'");
            return t;
        }

        public List<Team> GetAllTeams()
        {
            List<Team> teamsList = new List<Team>();
            string sql = $"SELECT * FROM Teams";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
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
                            t.Name = t.Name.Replace("\'\'", "\'");
                            teamsList.Add(t);
                        }
                    }                        
                } 
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
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teamsByYear.Add(GetTeamById((long)reader["id"]));
                        }
                    }                        
                }
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
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Team team)
        {
            string dateString = team.CreatedAt.Ticks.ToString();
            string modifiedName = team.Name.Replace("\'", "\'\'");
            string sql = $"UPDATE Teams SET name=\'{modifiedName}\', year={team.Year}, company=\'{team.Company}\', num_scorers=\'{team.NumScorers}\', created_at={dateString} WHERE id={team.Id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
