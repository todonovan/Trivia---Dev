using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.Scoreboard
{
    public class ScoreboardWindowViewModel : BindableBase
    {
        private Dictionary<string, int> _scores;

        private List<string> _orderedNames;
        public List<string> OrderedNames
        {
            get { return _orderedNames; }
            set { SetProperty(ref _orderedNames, value); }
        }

        private List<int> _orderedScores;
        public List<int> OrderedScores
        {
            get { return _orderedScores; }
            set { SetProperty(ref _orderedScores, value); }
        }

        public ScoreboardWindowViewModel()
        {

        }

        public void DisplayScores()
        {
            var scoresDict = _scores.ToList();
            scoresDict.Sort((score1, score2) => score2.Value.CompareTo(score1.Value));
            foreach (var pair in scoresDict)
            {
                OrderedNames.Add(pair.Key);
                OrderedScores.Add(pair.Value);
            }
        }

        public void SetScores(Dictionary<string, int> scores)
        {
            _scores = scores;
        }
    }
}
