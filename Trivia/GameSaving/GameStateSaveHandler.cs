using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trivia.ScoringHelpers;
using System.IO;
using System.Configuration;
using TriviaData.Repos;
using Trivia.Sessions;

namespace Trivia.GameSaving
{
    public class GameStateSaveHandler
    {
        private IScorerRepository _repo;

        public GameStateSaveHandler(IScorerRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// GameState saving is performed by serializing the Question lists representing the current
        /// mapping from Teams -> Answers, and by saving the file with the same filename where the
        /// session config paramaters are stored. Each successive save overwrite the previous save.
        /// </summary>
        /// <param name="gs"></param>
        public void SaveGame(GameState gs)
        {
            List<GameSaveParams> gameData = new List<GameSaveParams>();
            foreach (var s in gs.ActiveScorers)
            {
                foreach (var t in s.ScoringTeams)
                {
                    var answers = t.GetAllNonBonusAnswers();
                    gameData.Add(new GameSaveParams { TeamName = t.Team.Name, Answers = answers });
                }
            }
            using (var file = File.CreateText(ConfigurationManager.AppSettings["game_save_config"] + gs.FileName))
            {
                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(file, gameData);
            }
        }

        public GameState LoadGame(string name)
        {
            SessionConfigParams session = SessionSerialization.LoadSession(_repo, name);
            GameState gs = GameStateFactory.GetNewGameState(session);

            List<GameSaveParams> gameData = JsonConvert.DeserializeObject<List<GameSaveParams>>(File.ReadAllText(ConfigurationManager.AppSettings["game_save_config"] + name));

            Dictionary<string, List<List<Question>>> table = new Dictionary<string, List<List<Question>>>();
            foreach (var entry in gameData)
            {
                table.Add(entry.TeamName, entry.Answers);
            }

            foreach (var scorer in gs.ActiveScorers)
            {
                foreach (var team in scorer.ScoringTeams)
                {
                    team.SetAllAnswers(table[team.Team.Name]);
                }
            }

            return gs;
        }
    }
}
