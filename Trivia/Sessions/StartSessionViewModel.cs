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

        private Scorer _selectedScorer;
        public Scorer SelectedScorer
        {
            get { return _selectedScorer; }
            set { SetProperty(ref _selectedScorer, value); }
        }

        private ObservableCollection<Scorer> _selectedScorers;
        public ObservableCollection<Scorer> SelectedScorers
        {
            get { return _selectedScorers; }
            set { SetProperty(ref _selectedScorers, value); }
        }

        private ObservableCollection<ScoringRound> _scoringRounds;
        public ObservableCollection<ScoringRound> ScoringRounds
        {
            get { return _scoringRounds; }
            set { SetProperty(ref _scoringRounds, value); }
        }

        private int _numTeams;
        public int NumTeams
        {
            get { return _numTeams; }
            set { SetProperty(ref _numTeams, value); }
        }

        public StartSessionViewModel(ITeamRepository teamRepo, IScorerRepository scorerRepo)
        {
            _teamRepo = teamRepo;
            _scorerRepo = scorerRepo;
            SelectScorerCommand = new RelayCommand(OnSelectScorer);
            ResetCommand = new RelayCommand(OnReset);
            StartCommand = new RelayCommand(OnStart);
            SaveConfigCommand = new RelayCommand(OnSaveConfig);
            LoadConfigCommand = new RelayCommand(OnLoadConfig);
            LoadSavedSessionCommand = new RelayCommand(OnLoadSavedSession);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void BuildScoringRounds(int numRounds)
        {
            ScoringRounds = new ObservableCollection<ScoringRound>();
            ObservableCollection<ScoringRound> srBuilder = new ObservableCollection<ScoringRound>();

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
            SelectedScorers = new ObservableCollection<Scorer>();
        }

        private void OnSelectScorer()
        {
            SelectedScorers.Add(SelectedScorer);
            Scorers.Remove(SelectedScorer);
            SelectedScorer = null;
            int count = 0;
            foreach (var s in SelectedScorers) count += s.Teams.Count;
            NumTeams = count;
        }

        private void OnReset()
        {
            LoadScorers();
            UserNumRounds = 0;
            NumTeams = 0;
        }

        private void OnStart()
        {
            SessionConfigParams sessionConfig = new SessionConfigParams();
            sessionConfig.Scorers = SelectedScorers;
            sessionConfig.ScoringRounds = ScoringRounds;
            StartSessionRequested(sessionConfig);
        }

        private void OnSaveConfig()
        {
            SessionConfigParams sessionConfig = new SessionConfigParams();
            sessionConfig.Scorers = SelectedScorers;
            sessionConfig.ScoringRounds = ScoringRounds;
            SaveConfigRequested(sessionConfig);
        }

        private void OnLoadConfig()
        {
            LoadConfigRequested();
        }

        private void OnLoadSavedSession()
        {
            LoadSavedSessionRequested();
        }

        private void OnCancel()
        {
            Done();
        }

        public RelayCommand SelectScorerCommand { get; private set; }
        public RelayCommand SaveConfigCommand { get; private set; }
        public RelayCommand LoadConfigCommand { get; private set; }
        public RelayCommand LoadSavedSessionCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand ResetCommand { get; private set; }
        public RelayCommand StartCommand { get; private set; }

        public event Action<SessionConfigParams> SaveConfigRequested = delegate { };
        public event Action<SessionConfigParams> StartSessionRequested = delegate { };
        public event Action LoadConfigRequested = delegate { };
        public event Action LoadSavedSessionRequested = delegate { };
        public event Action Done = delegate { };
    }
}
