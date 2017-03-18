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
        private ObservableCollection<ReportedScore> _scoresFirst;
        public ObservableCollection<ReportedScore> ScoresFirst
        {
            get { return _scoresFirst; }
            set { SetProperty(ref _scoresFirst, value); }
        }

        private ObservableCollection<ReportedScore> _scoresSecond;
        public ObservableCollection<ReportedScore> ScoresSecond
        {
            get { return _scoresSecond; }
            set { SetProperty(ref _scoresSecond, value); }
        }

        public ScoreboardWindowViewModel()
        {
            ScoresFirst = new ObservableCollection<ReportedScore>();
            ScoresSecond = new ObservableCollection<ReportedScore>();
        }

        public void SetScores(List<ReportedScore> scores)
        {
            ScoresFirst = new ObservableCollection<ReportedScore>();
            ScoresSecond = new ObservableCollection<ReportedScore>();
            var scoresQuery = scores.OrderByDescending(s => s.Score).ThenBy(x => x.TeamName).ToList();
            int i = 0;
            while (i < scoresQuery.Count/2)
            {
                ScoresFirst.Add(scoresQuery[i++]);
            }
            if (scoresQuery.Count % 2 == 1)
            {
                ScoresFirst.Add(scoresQuery[i++]);
            }
            while (i < scoresQuery.Count)
            {
                ScoresSecond.Add(scoresQuery[i++]);
            }
        }
    }
}
