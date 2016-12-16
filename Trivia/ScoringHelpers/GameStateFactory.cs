using System.Collections.Generic;
using Trivia.Sessions;

namespace Trivia.ScoringHelpers
{
    public static class GameStateFactory
    {
        public static GameState GetNewGameState(SessionConfigParams config)
        {
            GameState gs = new GameState();
            gs.NumRounds = config.NumberOfRounds;
            gs.NumQuestionsPerRound = config.NumberOfQuestions;
            gs.FileName = config.FileName;
            List<ActiveScorer> activeScorers = new List<ActiveScorer>();
            foreach (var s in config.Scorers)
            {
                activeScorers.Add(new ActiveScorer(s, config.NumberOfRounds, config.NumberOfQuestions, config.PointsPerQuestion));
            }
            gs.ActiveScorers = activeScorers;

            return gs;
        }
    }
}
