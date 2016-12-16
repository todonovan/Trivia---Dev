using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Scorers;
using TriviaData.Models;

namespace Trivia.Sessions
{
    public class SessionStartConfirmViewModel : BindableBase
    {
        private SessionConfigParams _sessionConfigParams;

        private ObservableCollection<Scorer> _scorers;
        public ObservableCollection<Scorer> Scorers
        {
            get { return _scorers; }
            set { SetProperty(ref _scorers, value); }
        }

        private int _numTeams;
        public int NumTeams
        {
            get { return _numTeams; }
            set { SetProperty(ref _numTeams, value); }
        }

        public SessionStartConfirmViewModel()
        {
            CancelCommand = new RelayCommand(OnCancel);
            StartCommand = new RelayCommand(OnStart);
        }

        public void SetSessionConfig(SessionConfigParams scp)
        {
            _sessionConfigParams = scp;
            Scorers = new ObservableCollection<Scorer>(scp.Scorers);
            NumTeams = 0;
            foreach (var s in scp.Scorers) NumTeams += s.Teams.Count;
        }

        private void OnCancel()
        {
            Done();
        }

        private void OnStart()
        {
            SessionSerialization.SaveConfig(_sessionConfigParams);
            StartSessionRequested(_sessionConfigParams);
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand StartCommand { get; private set; }

        public event Action Done = delegate { };
        public event Action<SessionConfigParams> StartSessionRequested = delegate { };
    }
}
