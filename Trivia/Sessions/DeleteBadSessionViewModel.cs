using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace Trivia.Sessions
{
    public class DeleteBadSessionViewModel : BindableBase
    {
        private string _badSessionName;
        public string BadSessionName
        {
            get { return _badSessionName; }
            set { SetProperty(ref _badSessionName, value); }
        }

        public DeleteBadSessionViewModel()
        {
            ConfirmDeleteCommand = new RelayCommand(OnConfirmDelete);
            CancelDeleteCommand = new RelayCommand(OnCancelDelete);
        }

        public void SetSessionName(string name)
        {
            BadSessionName = name;
        }

        private void OnConfirmDelete()
        {
            File.Delete(ConfigurationManager.AppSettings["session_config"].ToString() + BadSessionName);
            File.Delete(ConfigurationManager.AppSettings["game_save_config"].ToString() + BadSessionName);
            Done();
        }

        private void OnCancelDelete()
        {
            Done();
        }

        public RelayCommand ConfirmDeleteCommand { get; private set; }
        public RelayCommand CancelDeleteCommand { get; private set; }

        public Action Done = delegate { };
    }
}
