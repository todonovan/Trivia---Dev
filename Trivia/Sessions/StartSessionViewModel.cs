﻿using System;
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
                SaveConfigCommand.RaiseCanExecuteChanged();
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private int _userNumQuestions;
        public int UserNumQuestions
        {
            get { return _userNumQuestions; }
            set
            {
                SetProperty(ref _userNumQuestions, value);
                SaveConfigCommand.RaiseCanExecuteChanged();
                StartCommand.RaiseCanExecuteChanged();
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
            set
            {
                SetProperty(ref _selectedScorers, value);
                SaveConfigCommand.RaiseCanExecuteChanged();
            }
        }

        private int _numTeams;
        public int NumTeams
        {
            get { return _numTeams; }
            set
            {
                SetProperty(ref _numTeams, value);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private string _userPointsPerQuestion;
        public string UserPointsPerQuestion
        {
            get { return _userPointsPerQuestion; }
            set
            {
                SetProperty(ref _userPointsPerQuestion, value);
                SaveConfigCommand.RaiseCanExecuteChanged();
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                SetProperty(ref _fileName, value);
                SaveConfigCommand.RaiseCanExecuteChanged();
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public StartSessionViewModel(ITeamRepository teamRepo, IScorerRepository scorerRepo)
        {
            _teamRepo = teamRepo;
            _scorerRepo = scorerRepo;
            _userPointsPerQuestion = string.Empty;
            SelectScorerCommand = new RelayCommand(OnSelectScorer);
            ResetCommand = new RelayCommand(OnReset);
            StartCommand = new RelayCommand(OnStart, CanStart);
            SaveConfigCommand = new RelayCommand(OnSaveConfig, CanSaveConfig);
            LoadConfigCommand = new RelayCommand(OnLoadConfig);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public void LoadScorers()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            Scorers = new ObservableCollection<Scorer>(_scorerRepo.GetAllScorers());
            SelectedScorers = new ObservableCollection<Scorer>();
            FileName = string.Empty;
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
            UserNumQuestions = 0;
            UserPointsPerQuestion = string.Empty;
            FileName = string.Empty;
            NumTeams = 0;
        }

        private void OnStart()
        {
            SessionConfigParams sessionConfig = new SessionConfigParams(UserNumRounds, UserNumQuestions, _userPointsPerQuestion, SelectedScorers.ToList(), FileName);
            List<string> doubledTeams = VerifyNoDoubleTeams();
            if (doubledTeams.Count == 0) StartSessionRequested(sessionConfig);
            else HandleDoubleTeams(doubledTeams);
        }

        private List<string> VerifyNoDoubleTeams()
        {
            List<Scorer> scorers = SelectedScorers.ToList();
            List<string> teams = new List<string>();
            List<string> doubledTeams = new List<string>();
            foreach (var s in scorers)
            {
                foreach (var t in s.Teams)
                {
                    if (teams.Contains(t.Name) && !doubledTeams.Contains(t.Name)) doubledTeams.Add(t.Name);
                    teams.Add(t.Name);
                }
            }
            return doubledTeams;
        }

        private void HandleDoubleTeams(List<string> doubledTeams)
        {
            string userMessageString = "Warning! This config contains some teams that are included more than once. Please return and set a new configuration.\n\nTeams affected: ";
            foreach (var t in doubledTeams)
            {
                userMessageString += t + "; ";
            }
            userMessageString = userMessageString.Substring(0, userMessageString.Length - 2);
            var messageBox = System.Windows.MessageBox.Show(userMessageString, "Some teams included more than once!", System.Windows.MessageBoxButton.OK);
            if (messageBox == System.Windows.MessageBoxResult.OK) OnReset();
        }

        private bool CanStart()
        {
            if (UserPointsPerQuestion == string.Empty) return false;
            try
            {
                return UserNumQuestions > 0 && UserNumRounds > 0 && Convert.ToInt32(UserPointsPerQuestion) > 0 && NumTeams > 0 && FileName != string.Empty;
            }
            catch
            {
                return false;
            }
        }

        private bool CanSaveConfig()
        {
            return UserPointsPerQuestion != string.Empty && UserNumRounds != 0 && UserNumQuestions != 0 && SelectedScorers.Count != 0 && FileName != string.Empty;
        }

        private void OnSaveConfig()
        {
            SessionConfigParams sessionConfig = new SessionConfigParams(UserNumRounds, UserNumQuestions, _userPointsPerQuestion, SelectedScorers.ToList(), FileName);
            List<string> doubledTeams = VerifyNoDoubleTeams();
            if (doubledTeams.Count == 0) SaveConfigRequested(sessionConfig);
            else HandleDoubleTeams(doubledTeams);
        }

        private void OnLoadConfig()
        {
            LoadConfigRequested();
        }

        private void OnCancel()
        {
            Done();
        }

        public RelayCommand SelectScorerCommand { get; private set; }
        public RelayCommand SaveConfigCommand { get; private set; }
        public RelayCommand LoadConfigCommand { get; private set; }
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
