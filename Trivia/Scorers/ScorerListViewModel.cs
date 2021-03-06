﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using TriviaData.Repos;

namespace Trivia.Scorers
{
    public class ScorerListViewModel : BindableBase
    {
        private IScorerRepository _repo;

        private ObservableCollection<Scorer> _scorers;
        public ObservableCollection<Scorer> Scorers
        {
            get { return _scorers; }
            set { SetProperty(ref _scorers, value); }
        }

        private List<Scorer> _scorersToDelete;

        private Scorer _selectedScorer;
        public Scorer SelectedScorer
        {
            get { return _selectedScorer; }
            set
            {
                _selectedScorer = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public ScorerListViewModel(IScorerRepository repo)
        {
            _repo = repo;
            _scorersToDelete = new List<Scorer>();
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            UpdateDBCommand = new RelayCommand(OnUpdate, CanUpdate);
            EditScorerCommand = new RelayCommand<Scorer>(OnEditScorer);
            AssociateTeamsCommand = new RelayCommand<Scorer>(OnAssociateTeams);
            AddScorerCommand = new RelayCommand(OnAddScorer);
        }

        public void LoadScorers()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            Scorers = new ObservableCollection<Scorer>(_repo.GetAllScorers());
            _scorersToDelete = new List<Scorer>();
            UpdateDBCommand.RaiseCanExecuteChanged();
        }

        public void OnAddScorer()
        {
            AddScorerRequested(new Scorer());
        }

        public void OnEditScorer(Scorer scorer)
        {
            EditScorerRequested(scorer);
        }

        private bool CanDelete()
        {
            return SelectedScorer != null;
        }

        private void OnDelete()
        {
            _scorersToDelete.Add(SelectedScorer);
            Scorers.Remove(SelectedScorer);
            UpdateDBCommand.RaiseCanExecuteChanged();
        }

        private bool CanUpdate()
        {
            return _scorersToDelete.Count != 0;
        }

        private void OnUpdate()
        {
            foreach (var s in _scorersToDelete) { _repo.Remove(s); }
            _scorersToDelete = new List<Scorer>();
            UpdateDBCommand.RaiseCanExecuteChanged();
        }

        private void OnAssociateTeams(Scorer scorer)
        {
            AssociateTeamsRequested(scorer);
        }

        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand UpdateDBCommand { get; private set; }
        public RelayCommand AddScorerCommand { get; private set; }
        public RelayCommand<Scorer> EditScorerCommand { get; private set; }
        public RelayCommand<Scorer> AssociateTeamsCommand { get; private set; }

        public event Action<Scorer> AddScorerRequested = delegate { };
        public event Action<Scorer> EditScorerRequested = delegate { };
        public event Action<Scorer> AssociateTeamsRequested = delegate { };
    }
}
