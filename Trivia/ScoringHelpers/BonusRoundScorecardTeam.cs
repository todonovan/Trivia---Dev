using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.ScoringHelpers
{
    public class BonusRoundScorecardTeam : BindableBase
    {
        public int BonusRoundNumber { get; set; }
        public string TeamName { get; set; }

        private int _maxWager;
        public int MaxWager
        {
            get { return _maxWager; }
            set { SetProperty(ref _maxWager, value); }
        }

        private bool _wagerValid;
        public bool WagerValid
        {
            get { return _wagerValid; }
            set { SetProperty(ref _wagerValid, value); }
        }

        private int _wager;
        public int Wager
        {
            get { return _wager; }
            set { SetProperty(ref _wager, value); }
        }

        private Question _roundAnswer;
        public Question RoundAnswer
        {
            get { return _roundAnswer; }
            set { SetProperty(ref _roundAnswer, value); }
        }

        public BonusRoundScorecardTeam(string teamName, BonusRoundAnswer roundAnswer, int maxWager)
        {
            BonusRoundNumber = roundAnswer.BonusRoundNumber;
            TeamName = teamName;
            RoundAnswer = roundAnswer.Answer;
            Wager = roundAnswer.Wager;
            MaxWager = maxWager;
            WagerValid = roundAnswer.Wager <= MaxWager;
        }
    }
}
