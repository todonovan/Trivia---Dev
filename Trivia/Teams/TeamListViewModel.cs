using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity;
using System.Collections.ObjectModel;
using TriviaData.Models;
using TriviaData;
using TriviaData.Repos;

namespace Trivia.Teams
{
    public class TeamListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Team> Teams { get; set; }
        private TriviaDbContext _dbcontext;
        private ITeamRepository _teamRepo;

        public TeamListViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            _dbcontext = new TriviaDbContext();
            _dbcontext.Open();
            _teamRepo = new TeamRepository(_dbcontext.Connection);
            Teams = _teamRepo.GetAllTeams();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
