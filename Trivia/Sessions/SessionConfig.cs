using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scoring;
using TriviaData.Models;
using TriviaData.Repos;
using System.IO;
using System.Configuration;

namespace Trivia.Sessions
{
    public static class SessionConfig
    {

        public static void SaveConfig(SessionConfigParams sessionConfig, string name)
        {
            string[] lines = new string[3];
            string scorerString = string.Empty;
            foreach (var s in sessionConfig.ActiveScorers)
            {
                scorerString += s.Scorer.Id.ToString();
                scorerString += ";";
            }

            string numRoundString = sessionConfig.NumRounds.ToString();
            string pointValString = sessionConfig.PointValue.ToString();
            lines[0] = scorerString;
            lines[1] = numRoundString;
            lines[2] = pointValString;

            using (var writer = new StreamWriter(ConfigurationManager.AppSettings["session_config"] + name))
            {
                foreach (var l in lines)
                {
                    writer.WriteLine(l);
                }
            }
        }

        public static SessionConfigParams LoadSession(IScorerRepository scorerRepo, string name)
        {
            List<Scorer> scorers = new List<Scorer>();
            List<ScoringRound> scoringRounds = new List<ScoringRound>();
            string[] lines = File.ReadAllLines(ConfigurationManager.AppSettings["session_config"] + name);
            string scorerString = lines[0], roundString = lines[1], pointValString = lines[2];
            string[] scorerStrings = scorerString.Split(';');
            for (int i = 0; i < scorerStrings.Length - 1; i++)
            {
                Scorer scorer = scorerRepo.GetScorerById(long.Parse(scorerStrings[i]));
                scorers.Add(scorer);
            }

            int numRounds = int.Parse(roundString);
            int pointValue = int.Parse(pointValString);

            SessionConfigParams session = new SessionConfigParams(new ObservableCollection<Scorer>(scorers), numRounds, pointValue);

            return session;
        }
    }
}
