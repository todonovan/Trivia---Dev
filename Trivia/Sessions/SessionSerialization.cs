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
using Trivia.ScoringHelpers;

namespace Trivia.Sessions
{
    public static class SessionSerialization
    {

        public static void SaveConfig(SessionConfigParams sessionConfig)
        {
            string[] lines = new string[5];
            string scorerString = string.Empty;
            foreach (var s in sessionConfig.Scorers)
            {
                scorerString += s.Id.ToString();
                scorerString += ";";
            }

            string numRoundString = sessionConfig.NumberOfRounds.ToString();
            string numQuestionString = sessionConfig.NumberOfQuestions.ToString();
            string pointValString = sessionConfig.PointsPerQuestion.ToString();
            lines[0] = scorerString;
            lines[1] = numRoundString;
            lines[2] = numQuestionString;
            lines[3] = pointValString;
            lines[4] = sessionConfig.FileName;

            using (var writer = new StreamWriter(ConfigurationManager.AppSettings["session_config"] + sessionConfig.FileName))
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
            string scorerString = lines[0], roundString = lines[1], numQuestionString = lines[2], pointValString = lines[3], fileName = lines[4];
            string[] scorerStrings = scorerString.Split(';');
            for (int i = 0; i < scorerStrings.Length - 1; i++)
            {
                Scorer scorer = scorerRepo.GetScorerById(long.Parse(scorerStrings[i]));
                scorers.Add(scorer);
            }      

            int numRounds = int.Parse(roundString);
            int numQuestions = int.Parse(numQuestionString);

            SessionConfigParams session = new SessionConfigParams(numRounds, numQuestions, pointValString, scorers, fileName);

            return session;
        }
    }
}
