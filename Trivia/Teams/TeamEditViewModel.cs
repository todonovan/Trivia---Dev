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
    public class TeamEditViewModel : BindableBase
    {
        private TriviaDbContext _dbConn;
        private ITeamRepository _teamRepo;
        public long TeamId
        {
            get
            { return 1; }
            set { }
        }

        public TeamEditViewModel(TriviaDbContext dbConn)
        {
            _dbConn = dbConn;
            _teamRepo = new TeamRepository(_dbConn.Connection);
            SaveCommand = new RelayCommand(OnSave);
        }

        private Team _team;
        public Team Team
        {
            get { return _team; }
            set { SetProperty(ref _team, value); }
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
