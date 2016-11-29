using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.ScoringHelpers;

namespace Trivia.Scoring
{
    public class ScorerRoundScorecardViewModel : BindableBase
    {
        private GameState _gameState;

        private int _roundNumber;
        public int RoundNumber
        {
            get { return _roundNumber; }
            set { SetProperty(ref _roundNumber, value); }
        }        

        private ActiveScorer _scorer;
        public ActiveScorer Scorer
        {
            get { return _scorer; }
            set
            {
                SetProperty(ref _scorer, value);
                ScorerName = value.Name;
            }
        }

        private ObservableCollection<ScorecardTeam> _teams;
        public ObservableCollection<ScorecardTeam> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }

        private ScorecardTeam _selectedTeam;
        public ScorecardTeam SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                SetProperty(ref _selectedTeam, value);
            }
        }

        private int _selectedQuestionIndex;
        public int SelectedQuestionIndex
        {
            get { return _selectedQuestionIndex; }
            set { SetProperty(ref _selectedQuestionIndex, value); }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set { SetProperty(ref _selectedQuestion, value); }
        }

        public string ScorerName { get; private set; }

        public ScorerRoundScorecardViewModel()
        {
            ScorerName = string.Empty;
            RoundNumber = 0;

            IncrementQuestionCommand = new RelayCommand(OnIncrementQuestion);
            SaveChangesCommand = new RelayCommand(OnSaveChanges);
        }

        public void SetRoundAndScorer(RoundScoringParams roundParams, ActiveScorer s)
        {
            RoundNumber = roundParams.RoundNumber;
            _gameState = roundParams.GameState;
            Scorer = s;
            ScorerName = s.Name;
            Teams = new ObservableCollection<ScorecardTeam>();
            foreach (var t in s.ScoringTeams)
            {
                Teams.Add(new ScorecardTeam(RoundNumber, t.Team.Name, t.GetRoundAnswers(RoundNumber)));
            }
            SelectedTeam = Teams[0];
        }

        private void OnIncrementQuestion()
        {
            int curIndex = SelectedQuestionIndex;
            Question q = SelectedTeam.RoundAnswers[SelectedQuestionIndex];
            if (q == Question.NotJudged) SelectedTeam.RoundAnswers[SelectedQuestionIndex] = Question.Correct;
            else if (q == Question.Correct) SelectedTeam.RoundAnswers[SelectedQuestionIndex] = Question.Incorrect;
            else if (q == Question.Incorrect) SelectedTeam.RoundAnswers[SelectedQuestionIndex] = Question.NotAnswered;
            else if (q == Question.NotAnswered) SelectedTeam.RoundAnswers[SelectedQuestionIndex] = Question.Correct;
            SelectedQuestionIndex = curIndex;
        }

        private void OnSaveChanges()
        {
            foreach (var t in Teams)
            {
                Scorer.ScoringTeams.Where(x => x.Team.Name == t.TeamName).Single().SetRoundAnswers(t.RoundAnswers.ToList(), RoundNumber);
            }
        }

        public RelayCommand IncrementQuestionCommand { get; private set; }
        public RelayCommand SaveChangesCommand { get; private set; }
    }
}
