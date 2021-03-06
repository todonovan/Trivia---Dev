﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity;
using System.ComponentModel;
using TriviaData.Repos;
using TriviaData;
using TriviaData.Models;
using System.Windows.Input;

namespace Trivia.Teams
{
    public class TeamAddViewModel : BindableBase
    {
        private TriviaDbContext _dbConn;
        private ITeamRepository _teamRepo;

        public RelayCommand SaveCommand { get; private set; }

        private Team _team;
        public Team Team
        {
            get { return _team; }
            set { SetProperty(ref _team, value); }
        }

        public TeamAddViewModel(TriviaDbContext dbConn)
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            _dbConn = dbConn;
            _teamRepo = new TeamRepository();
            Team = new Team();
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        private bool CanSave()
        {
            return Team.Name != null && Team.Year != 0 && Team.Company != null;
        }

        private void OnSave()
        {
            _teamRepo.Add(Team);
            Team = new Team();
        }
    }
}
