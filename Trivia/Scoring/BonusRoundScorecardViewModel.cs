using System;
using System.Collections.ObjectModel;
using System.Linq;
using Trivia.ScoringHelpers;

namespace Trivia.Scoring
{
    public class BonusRoundScorecardViewModel : BindableBase
    {
        private GameState _gameState;

        private int _bonusRoundNumber;
        public int BonusRoundNumber
        {
            get { return _bonusRoundNumber; }
            set { SetProperty(ref _bonusRoundNumber, value); }
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

        private string _scorerName;
        public string ScorerName
        {
            get { return _scorerName; }
            set { SetProperty(ref _scorerName, value); }
        }

        private ObservableCollection<BonusRoundScorecardTeam> _teams;
        public ObservableCollection<BonusRoundScorecardTeam> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }

        private BonusRoundScorecardTeam _selectedTeam;
        public BonusRoundScorecardTeam SelectedTeam
        {
            get { return _selectedTeam; }
            set { SetProperty(ref _selectedTeam, value); }
        }

        private bool _allWagersValid;
        public bool AllWagersValid
        {
            get { return _allWagersValid; }
            set { SetProperty(ref _allWagersValid, value); }
        }

        public BonusRoundScorecardViewModel()
        {
            ScorerName = string.Empty;
            BonusRoundNumber = 0;

            SetWagerAttemptCommand = new RelayCommand<string>(OnSetWagerAttempt);
            IncrementQuestionCommand = new RelayCommand(OnIncrementQuestion);
            IncrementAndAdvanceCommand = new RelayCommand<string>(OnIncrementAndAdvance);
            NextQuestionCommand = new RelayCommand(OnNextQuestion);
            PrevQuestionCommand = new RelayCommand(OnPrevQuestion);
            SaveChangesCommand = new RelayCommand(OnSaveChanges);
        }

        public void SetRoundAndScorer(RoundScoringParams roundParams, ActiveScorer s)
        {
            BonusRoundNumber = roundParams.RoundNumber;
            _gameState = roundParams.GameState;
            Scorer = s;
            ScorerName = s.Name;
            Teams = new ObservableCollection<BonusRoundScorecardTeam>();
            foreach (var t in s.ScoringTeams)
            {
                Teams.Add(new BonusRoundScorecardTeam(t.Team.Name, t.GetBonusRoundAnswer(BonusRoundNumber), t.GetScore()));
            }
            SelectedTeam = Teams[0];
            AllWagersValid = true;
        }

        private void OnSetWagerAttempt(string requestedWager)
        {
            int requestedWagerNum = int.Parse(requestedWager);
            SelectedTeam.Wager = requestedWagerNum;
            if (!SelectedTeam.WagerValid) AllWagersValid = false;
        }

        private void RecheckWagers()
        {
            foreach (var t in Teams)
            {
                if (!t.WagerValid)
                {
                    AllWagersValid = false;
                    return;
                }
            }
            AllWagersValid = true;
        }

        private void OnIncrementQuestion()
        {
            Question q = SelectedTeam.RoundAnswer;
            if (q == Question.NotJudged) SelectedTeam.RoundAnswer = Question.Correct;
            else if (q == Question.Correct) SelectedTeam.RoundAnswer = Question.Incorrect;
            else if (q == Question.Incorrect) SelectedTeam.RoundAnswer = Question.NotAnswered;
            else if (q == Question.NotAnswered) SelectedTeam.RoundAnswer = Question.Correct;
        }

        private void OnIncrementAndAdvance(string input)
        {
            if (input == "g")
            {
                SelectedTeam.RoundAnswer = Question.Correct;
                OnNextQuestion();
            }
            else if (input == "r")
            {
                SelectedTeam.RoundAnswer = Question.Incorrect;
                OnNextQuestion();
            }
            else if (input == "y")
            {
                SelectedTeam.RoundAnswer = Question.NotAnswered;
                OnNextQuestion();
            }            
        }

        private void OnNextQuestion()
        {
            if (SelectedTeam == Teams[Teams.Count - 1])
            {
                OnNextScorer();
            }
            else
            {
                int curTeamIndex = Teams.IndexOf(SelectedTeam);
                SelectedTeam = Teams[curTeamIndex + 1];
            }
        }

        private void OnPrevQuestion()
        {
            if (SelectedTeam == Teams[0])
            {
                return;
            }
            else
            {
                int curTeamIndex = Teams.IndexOf(SelectedTeam);
                SelectedTeam = Teams[curTeamIndex - 1];
            }
        }

        private void OnNextScorer()
        {
            OnSaveChanges();
            NextScorerRequested(_gameState);
        }

        public void OnSaveChanges()
        {
            foreach (var t in Teams)
            {
                Scorer.ScoringTeams.Where(x => x.Team.Name == t.TeamName).Single().SetBonusRoundAnswer(new BonusRoundAnswer(t.Wager, t.RoundAnswer, BonusRoundNumber));
            }
        }

        public RelayCommand<string> SetWagerAttemptCommand { get; private set; }
        public RelayCommand IncrementQuestionCommand { get; private set; }
        public RelayCommand<string> IncrementAndAdvanceCommand { get; private set; }
        public RelayCommand NextQuestionCommand { get; private set; }
        public RelayCommand PrevQuestionCommand { get; private set; }
        public RelayCommand SaveChangesCommand { get; private set; }

        public event Action<GameState> NextScorerRequested = delegate { };
    }
}
