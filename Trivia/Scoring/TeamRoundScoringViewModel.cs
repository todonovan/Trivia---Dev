using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.Scoring
{
    public class TeamRoundScoringViewModel : BindableBase
    {
        public enum Question { NotAnswered, Correct, Incorrect };

        public int NumQuestions { get; private set; }
        public string TeamName { get; private set; }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set { SetProperty(ref _questions, value); }
        }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set { SetProperty(ref _currentQuestion, value); }
        }

        public TeamRoundScoringViewModel()
        {

        }
        
    }
}
