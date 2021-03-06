﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using System.Data.SQLite;

namespace TriviaData.Repos
{
    public class ScorerRepository : IScorerRepository
    {
        public void Add()
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO Scorers (name, team_id_list, created_at) VALUES (\'\', \'\', {dateString})";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Add(Scorer scorer)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string teamIds = " ";
            if (scorer.Teams.Count != 0)
            {
                string[] teamIdsList = new string[scorer.Teams.Count];
                for (int i = 0; i < teamIdsList.Length; i++)
                {
                    teamIdsList[i] = scorer.Teams[i].Id.ToString();
                }
                teamIds = string.Join(",", teamIdsList);
            }
            string sql = $"INSERT INTO Scorers (name, team_id_list, created_at) VALUES (\'{scorer.Name}\', \'{teamIds}\', {dateString})";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public Scorer GetScorerById(long id)
        {
            Scorer s = new Scorer();
            string sql = $"SELECT * FROM Scorers WHERE id={id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        s.Id = id;
                        s.Name = (string)reader["name"];
                        s.Teams = new List<Team>();
                        string teamIdsString = (string)reader["team_id_list"];
                        if (teamIdsString != " ")
                        {
                            TeamRepository teamRepo = new TeamRepository();
                            long[] teamIds = Array.ConvertAll(teamIdsString.Split(','), x => long.Parse(x));
                            foreach (var t in teamIds)
                            {
                                Team team = teamRepo.GetTeamById(t);
                                s.Teams.Add(team);
                            }
                        }
                        s.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
                    }                        
                }
            }
            return s;
        }

        public List<Scorer> GetAllScorers()
        {
            List<Scorer> scorerList = new List<Scorer>();
            string sql = $"SELECT * FROM Scorers";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        TeamRepository teamRepo = new TeamRepository();

                        while (reader.Read())
                        {
                            Scorer s = new Scorer();
                            s.Id = (long)reader["id"];
                            s.Name = (string)reader["name"];
                            s.Teams = new List<Team>();
                            string teamIdsString = (string)reader["team_id_list"];
                            if (teamIdsString != " ")
                            {
                                long[] teamIds = Array.ConvertAll(teamIdsString.Split(','), x => long.Parse(x));
                                foreach (var t in teamIds)
                                {
                                    Team team = teamRepo.GetTeamById(t);
                                    s.Teams.Add(team);
                                }
                            }
                            s.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));
                            scorerList.Add(s);
                        }
                    }                        
                }
            }
            return scorerList;
        }

        public void Remove(Scorer scorer)
        {
            if (scorer.Teams.Count > 0)
            {
                TeamRepository teamRepo = new TeamRepository();
                foreach (var t in scorer.Teams)
                {
                    Team team = teamRepo.GetTeamById(t.Id);
                    team.NumScorers -= 1;
                    if (team.NumScorers == 0) team.HasScorer = false;
                    teamRepo.Update(team);
                }
            }
            long id = scorer.Id;
            string sql = $"DELETE FROM Scorers WHERE id={id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Scorer scorer)
        {
            long id = scorer.Id;
            string teamIds = " ";
            if (scorer.Teams.Count != 0)
            {
                string[] teamIdList = new string[scorer.Teams.Count];
                for (int i = 0; i < teamIdList.Length; i++)
                {
                    teamIdList[i] = scorer.Teams[i].Id.ToString();
                }
                teamIds = string.Join(",", teamIdList);
            }
            string sql = $"UPDATE Scorers SET name=\'{scorer.Name}\', team_id_list=\'{teamIds}\' WHERE id={id}";
            using (var _dbConn = new TriviaDbContext())
            {
                _dbConn.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, _dbConn.Connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddTeamToScorer(Scorer scorer, Team team)
        {
            team.NumScorers += 1;
            team.HasScorer = true;
            TeamRepository teamRepo = new TeamRepository();
            teamRepo.Update(team);
            Update(scorer);
        }

        public void RemoveTeamFromScorer(Scorer scorer, Team team)
        {
            team.NumScorers -= 1;
            if (team.NumScorers == 0) team.HasScorer = false;
            TeamRepository teamRepo = new TeamRepository();
            teamRepo.Update(team);
            scorer.Teams.Remove(team);
            Update(scorer);
        }
    }
}
