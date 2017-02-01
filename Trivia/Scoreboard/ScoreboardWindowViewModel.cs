using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;

namespace Trivia.Scoreboard
{
    public class ScoreboardWindowViewModel : BindableBase
    {
        private ObservableCollection<ReportedScore> _scores;
        public ObservableCollection<ReportedScore> Scores
        {
            get { return _scores; }
            set { SetProperty(ref _scores, value); }
        }

        public ScoreboardWindowViewModel()
        {
            Scores = new ObservableCollection<ReportedScore>();
        }

        public void SetScores(List<ReportedScore> scores)
        {
            Scores = new ObservableCollection<ReportedScore>(scores.OrderBy(x => x.TeamName).OrderBy(s => (-1 * s.Score)).ToList());
        }
    }
}
