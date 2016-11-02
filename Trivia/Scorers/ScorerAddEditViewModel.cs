using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using TriviaData.Repos;

namespace Trivia.Scorers
{
    public class ScorerAddEditViewModel : BindableBase
    {
        private IScorerRepository _repo;

        private Scorer _editingScorer = null;
        public Scorer EditingScorer
        {
            get { return _editingScorer; }
            set { SetProperty(ref _editingScorer, value); }
        }

        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        private SimpleEditableScorer _scorer;
        public SimpleEditableScorer Scorer
        {
            get { return _scorer; }
            set { SetProperty(ref _scorer, value); }
        }

        public ScorerAddEditViewModel(IScorerRepository repo)
        {
            _repo = repo;
            SaveCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand(OnCancel);
            AssociateTeamsCommand = new RelayCommand<Scorer>(OnAssociateTeams);
        }

        public void SetScorer(Scorer scorer)
        {
            EditingScorer = scorer;
            if (Scorer != null) Scorer.ErrorsChanged -= RaiseCanExecuteChanged;
            Scorer = new SimpleEditableScorer();
            Scorer.ErrorsChanged += RaiseCanExecuteChanged;
            CopyScorer(scorer, Scorer);
        }

        private void UpdateScorer(SimpleEditableScorer source, Scorer target)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.Teams = source.Teams;
        }

        private void CopyScorer(Scorer source, SimpleEditableScorer target)
        {
            if (EditMode)
            {
                target.Id = source.Id;
                target.Name = source.Name;
                target.Teams = source.Teams;
            }
        }

        private void OnSave()
        {
            UpdateScorer(Scorer, _editingScorer);
            if (EditMode) _repo.Update(_editingScorer);
            else _repo.Add(_editingScorer);
            Done();
        }

        private bool CanSave()
        {
            return !Scorer.HasErrors;
        }

        private void OnCancel()
        {
            Done();
        }

        private void OnAssociateTeams(Scorer scorer)
        {
            AssociateTeamsRequested(scorer);
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<Scorer> AssociateTeamsCommand { get; private set; }

        public event Action<Scorer> AssociateTeamsRequested = delegate { };
        public event Action Done = delegate { };
    }
}
