using System;
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
    public class ScorerSetTeamsViewModel : BindableBase
    {
        private ITeamRepository _repo;

        private ObservableCollection<Team> _teams;
        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }

        private ObservableCollection<Team> _teamsToAssociate = new ObservableCollection<Team>();
        public ObservableCollection<Team> TeamsToAssociate
        {
            get { return _teamsToAssociate; }
            set { SetProperty(ref _teamsToAssociate, value); }
        }

        public ScorerSetTeamsViewModel(ITeamRepository repo)
        {
            _repo = repo;
            AssociateTeamsCommand = new RelayCommand(OnAssociateTeams, CanAssociateTeams);
        }

        public void LoadTeams()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            Teams = new ObservableCollection<Team>(_repo.GetAllTeams());
        }

        private bool CanAssociateTeams()
        {
            return _teamsToAssociate.Count != 0;
        }

        private void OnAssociateTeams()
        {

        }
    }
}
