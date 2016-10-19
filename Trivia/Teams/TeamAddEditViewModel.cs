using System;
using TriviaData.Models;
using TriviaData.Repos;
using TriviaData;

namespace Trivia.Teams
{
    public class TeamAddEditViewModel : BindableBase
    {
        private ITeamRepository _repo;

        private Team _editingTeam = null;

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        private SimpleEditableTeam _team;
        public SimpleEditableTeam Team
        {
            get { return _team; }
            set { SetProperty(ref _team, value); }
        }

        public TeamAddEditViewModel(ITeamRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void OnSave()
        {
            UpdateTeam(Team, _editingTeam);
            if (EditMode) _repo.Update(_editingTeam);
            else _repo.Add(_editingTeam);
            Done();
        }

        private bool CanSave()
        {
            return !Team.HasErrors;
        }

        private void OnCancel()
        {
            Done();
        }

        public void SetTeam(Team team)
        {
            _editingTeam = team;
            if (Team != null) Team.ErrorsChanged -= RaiseCanExecuteChanged;
            Team = new SimpleEditableTeam();
            Team.ErrorsChanged += RaiseCanExecuteChanged;
            CopyTeam(team, Team);
        }

        private void UpdateTeam(SimpleEditableTeam source, Team target)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.Year = source.Year;
            target.Company = source.Company;
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void CopyTeam(Team source, SimpleEditableTeam target)
        {
            if (EditMode)
            {
                target.Id = source.Id;
                target.Name = source.Name;
                target.Year = source.Year;
                target.Company = source.Company;
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public event Action Done = delegate { };
    }
}
