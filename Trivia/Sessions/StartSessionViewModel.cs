using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using Trivia.Scoring;
using TriviaData.Repos;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using TriviaData.Models;
using System.ComponentModel;

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

        private ObservableCollection<Scorer> _scorers;
        public ObservableCollection<Scorer> Scorers
        {
            get { return _scorers; }
            set { SetProperty(ref _scorers, value); }
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
            ResetCommand = new RelayCommand(OnReset);
            StartCommand = new RelayCommand(OnStart);
            LoadConfigCommand = new RelayCommand(OnLoadConfig);
            LoadSavedSessionCommand = new RelayCommand(OnLoadSavedSession);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void BuildScoringRounds(int numRounds)
        {
            ScoringRounds = new List<ScoringRound>();
            List<ScoringRound> srBuilder = new List<ScoringRound>();

            for (int i = 1; i < numRounds; i++)
            {
                ScoringRound sr = new ScoringRound(i, false, 5, 0);
                srBuilder.Add(sr);
            }

            srBuilder.Add(new ScoringRound(numRounds, true, 1, 0));
            ScoringRounds = srBuilder;
        }

        public void LoadScorers()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            Scorers = new ObservableCollection<Scorer>(_scorerRepo.GetAllScorers());

        }

        private void OnReset()
        {
            LoadScorers();
            UserNumRounds = 0;
        }

        private void OnStart()
        {

        }

        private void OnLoadConfig()
        {

        }

        private void OnLoadSavedSession()
        {

        }

        private void OnCancel()
        {
            Done();
        }

        public RelayCommand LoadConfigCommand { get; private set; }
        public RelayCommand LoadSavedSessionCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand ResetCommand { get; private set; }
        public RelayCommand StartCommand { get; private set; }

        public event Action Done = delegate { };
    }
}
