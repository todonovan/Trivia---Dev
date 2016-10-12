using System;
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
        private SQLiteConnection _dbconn;

        public ScorerRepository(SQLiteConnection dbConn)
        {
            _dbconn = dbConn;
        }

        public void Add()
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO Scorers (team_id_list, created_at) VALUES (\'\', {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbconn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public void Add(Scorer scorer)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string[] teamIdsList = new string[scorer.Teams.Count];
            for (int i = 0; i < teamIdsList.Length; i++)
            {
                teamIdsList[i] = scorer.Teams[i].Id.ToString();
            }
            string sql = $"INSERT INTO Scorers (team_id_list, created_at) VALUES ({string.Join("-", teamIdsList)}, {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbconn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public void Add(List<Team> teams)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            long[] teamIds = teams.Select(t => t.Id).ToArray();
            string[] teamIdList = Array.ConvertAll(teamIds, x => x.ToString());
            string sql = $"INSERT INTO Scorers (team_id_list, created_at) VALUES ({string.Join("-", teamIdList)}, {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbconn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public Scorer GetScorerById(long id)
        {
            string sql = $"SELECT * FROM Scorers WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbconn);
            SQLiteDataReader reader = command.ExecuteReader();


            Scorer s = new Scorer();
            s.Id = id;
            s.Teams = new List<Team>();
            string teamIdsString = (string)reader["team_id_list"];
            if (teamIdsString != "")
            {
                TeamRepository teamRepo = new TeamRepository(_dbconn);
                long[] teamIds = Array.ConvertAll(teamIdsString.Split('_'), x => long.Parse(x));
                foreach (var t in teamIds)
                {
                    Team team = teamRepo.GetTeamById(t);
                    s.Teams.Add(team);
                }
            }
            s.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));

            command.Dispose();
            return s;
        }

        public List<Scorer> GetAllScorers()
        {
            List<Scorer> scorerList = new List<Scorer>();
            string sql = $"SELECT * FROM Scorers";
            SQLiteCommand command = new SQLiteCommand(sql, _dbconn);
            SQLiteDataReader reader = command.ExecuteReader();
            TeamRepository teamRepo = new TeamRepository(_dbconn);

            while (reader.Read())
            {
                Scorer s = new Scorer();
                s.Id = (long)reader["id"];
                s.Teams = new List<Team>();
                string teamIdsString = (string)reader["team_id_list"];
                if (teamIdsString != "")
                {
                    long[] teamIds = Array.ConvertAll(teamIdsString.Split('_'), x => long.Parse(x));
                    foreach (var t in teamIds)
                    {
                        Team team = teamRepo.GetTeamById(t);
                        s.Teams.Add(team);
                    }
                }
                s.CreatedAt = new DateTime((long)reader["created_at"]);
                scorerList.Add(s);
            }

            command.Dispose();
            return scorerList;
        }

        public void Remove(Scorer scorer)
        {
            long id = scorer.Id;
            string sql = $"DELETE FROM Scorers WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbconn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public void Update(Scorer scorer)
        {
            long id = scorer.Id;
            string[] memberIdList = new string[scorer.Teams.Count];
            for (int i = 0; i < memberIdList.Length; i++)
            {
                memberIdList[i] = scorer.Teams[i].Id.ToString();
            }
            string sql = $"UPDATE Scorer SET team_id_list={string.Join("-", memberIdList)} WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbconn);
            command.ExecuteNonQuery();

            command.Dispose();
        }
    }
}
