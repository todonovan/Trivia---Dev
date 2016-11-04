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
            string[] lines = new string[2];
            string scorerString = string.Empty;
            foreach (var s in sessionConfig.Scorers)
            {
                scorerString += s.Id.ToString();
                scorerString += ";";
            }

            List<ScoringRound> srList = sessionConfig.ScoringRounds.ToList();
            string roundString = string.Empty;
            foreach (var r in srList)
            {
                roundString += r.OrderOfRound + "," + r.NumQuestions + "," + r.PointValue + "," + r.IsBonusRound + ";";
            }
            lines[0] = scorerString;
            lines[1] = roundString;


            using (var writer = new StreamWriter(@"C:\Users\Terrence\Documents\TriviaDev\Trivia\Trivia\SessionConfigs\" + name))
            {
                foreach (var l in lines)
                {
                    writer.WriteLine(l);
                }
            }
        }

        public static SessionConfigParams LoadSession(IScorerRepository scorerRepo, string name)
        {
            SessionConfigParams session = new SessionConfigParams();
            List<Scorer> scorers = new List<Scorer>();
            List<ScoringRound> scoringRounds = new List<ScoringRound>();
            string[] lines = File.ReadAllLines(ConfigurationManager.ConnectionStrings["session_config"] + name);
            string scorerString = lines[0], roundString = lines[1];
            string[] scorerStrings = scorerString.Split(';');
            foreach (var s in scorerStrings)
            {
                Scorer scorer = scorerRepo.GetScorerById(long.Parse(s));
                scorers.Add(scorer);
            }
            session.Scorers = new ObservableCollection<Scorer>(scorers);

            string[] roundStrings = roundString.Split(';');
            foreach (var r in roundStrings)
            {
                string[] substrings = r.Split(',');
                int orderOfRound = int.Parse(substrings[0]);
                int numQuestions = int.Parse(substrings[1]);
                int pointValue = int.Parse(substrings[2]);
                bool isBonusRound = bool.Parse(substrings[3]);
                ScoringRound sr = new ScoringRound(orderOfRound, isBonusRound, numQuestions, pointValue);
                scoringRounds.Add(sr);
            }
            session.ScoringRounds = new ObservableCollection<ScoringRound>(scoringRounds);

            return session;
        }
    }
}
