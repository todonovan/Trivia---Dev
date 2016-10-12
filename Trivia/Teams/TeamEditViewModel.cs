using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using TriviaData.Repos;
using System.Configuration;
using System.ComponentModel;
using TriviaData;
using System.Windows.Input;

namespace Trivia.Teams
{
    public class TeamEditViewModel : INotifyPropertyChanged
    {
        private Team _team;
        private TriviaDbContext _dbContext;
        private ITeamRepository _teamRepo;
        public long TeamId
        {
            get
            { return 1; }
            set { }
        }

        public TeamEditViewModel()
        {
            _dbContext = new TriviaDbContext();
            _dbContext.Open();
            _teamRepo = new TeamRepository(_dbContext.Connection);
            SaveCommand = new RelayCommand(OnSave);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

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

        public ICommand SaveCommand { get; private set; }

        public void LoadTeam()
        {
            Team = _teamRepo.GetTeamById(TeamId);
        }

        private void OnSave()
        {
            _teamRepo.Update(Team);
        }
    }
}
