using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.ScoringHelpers
{
    public class ScorecardTeam : BindableBase
    {
        public int RoundNumber { get; set; }
        public string TeamName { get; set; }
        private ObservableCollection<Question> _roundAnswers;
        public ObservableCollection<Question> RoundAnswers
        {
            get { return _roundAnswers; }
            set { SetProperty(ref _roundAnswers, value); }
        }

        public ScorecardTeam(int roundNumber, string teamName, List<Question> roundAnswers)
        {
            RoundNumber = roundNumber;
            TeamName = teamName;
            RoundAnswers = new ObservableCollection<Question>(roundAnswers);
        }
    }
}
