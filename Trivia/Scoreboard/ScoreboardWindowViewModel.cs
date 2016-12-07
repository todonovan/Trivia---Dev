using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.Scoreboard
{
    public class ScoreboardWindowViewModel : BindableBase
    {
        private ObservableCollection<ScoreboardScore> _scores;
        public ObservableCollection<ScoreboardScore> Scores
        {
            get { return _scores; }
            set { SetProperty(ref _scores, value); }
        }

        public ScoreboardWindowViewModel()
        {
            Scores = new ObservableCollection<ScoreboardScore>();
        }

        public void SetScores(List<ScoreboardScore> scores)
        {
            Scores = new ObservableCollection<ScoreboardScore>(scores.OrderBy(s => (-1 * s.Score)).ToList());
        }
    }
}
