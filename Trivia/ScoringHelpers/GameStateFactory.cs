﻿using System.Collections.Generic;
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
            List<ActiveScorer> activeScorers = new List<ActiveScorer>();
            foreach (var s in config.Scorers)
            {
                activeScorers.Add(new ActiveScorer(s, config.NumberOfRounds, config.NumberOfQuestions, config.PointValuesPerRound));
            }
            gs.ActiveScorers = activeScorers;

            return gs;
        }
    }
}