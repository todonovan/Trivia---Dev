using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scoring;
using TriviaData.Repos;

namespace Trivia.Sessions
{
    public class StartSessionViewModel : BindableBase
    {
        private ITeamRepository _teamRepo;
        private IScorerRepository _scorerRepo;

        private int _userNumRounds;
        public int UserNumRounds
        {
            get { return _userNumRounds; }
            set
            {
                SetProperty(ref _userNumRounds, value);
                BuildScoringRounds(UserNumRounds);
            }
        }

        private List<ScoringRound> _scoringRounds;
        public List<ScoringRound> ScoringRounds
        {
            get { return _scoringRounds; }
            set { SetProperty(ref _scoringRounds, value); }
        }

        public StartSessionViewModel(ITeamRepository teamRepo, IScorerRepository scorerRepo)
        {
            _teamRepo = teamRepo;
            _scorerRepo = scorerRepo;
        }

        private void BuildScoringRounds(int numRounds)
        {
            ScoringRounds = new List<ScoringRound>();
            List<ScoringRound> srBuilder = new List<ScoringRound>();

            for (int i = 0; i < numRounds; i++)
            {
                ScoringRound sr = new ScoringRound(i, false, 5, 0);
                srBuilder.Add(sr);
            }

            srBuilder.Add(new ScoringRound(numRounds - 1, true, 1, 0));
            ScoringRounds = srBuilder;
        }
    }
}
