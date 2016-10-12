using System;
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
    public class TeamAddViewModel : INotifyPropertyChanged
    {
        private TriviaDbContext _dbContext;
        private ITeamRepository _teamRepo;
        private Team _team;

        public ICommand SaveCommand { get; private set; }

        public Team Team
        {
            get { return _team; }
            set
            {
                if (value != _team)
                {
                    _team = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Team"));
                }
            }
        }

        public TeamAddViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            _dbContext = new TriviaDbContext();
            _dbContext.Open();
            _teamRepo = new TeamRepository(_dbContext.Connection);
            Team = new Team();
            SaveCommand = new RelayCommand(OnSave);
        }

        private void OnSave()
        {
            _teamRepo.Add(Team);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
