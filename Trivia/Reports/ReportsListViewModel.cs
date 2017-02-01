using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.Reports
{
    public class ReportsListViewModel : BindableBase
    {
        private ReportFileHandler _fileHandler;

        private ObservableCollection<string> _savedReportNames;
        public ObservableCollection<string> SavedReportNames
        {
            get { return _savedReportNames; }
            set { SetProperty(ref _savedReportNames, value); }
        }

        private string _selectedReportName;
        public string SelectedReportName
        {
            get { return _selectedReportName; }
            set
            {
                SetProperty(ref _selectedReportName, value);
                OpenReportCommand.RaiseCanExecuteChanged();
                DeleteReportCommand.RaiseCanExecuteChanged();
            }
        }

        private List<string> _filesToDelete;

        public ReportsListViewModel()
        {
            _fileHandler = new ReportFileHandler();
            _filesToDelete = new List<string>();
            OpenReportCommand = new RelayCommand(OnOpenReport, CanOpenReport);
            DeleteReportCommand = new RelayCommand(OnDeleteReport, CanDeleteReport);
            UndoDeleteCommand = new RelayCommand(OnUndoDelete, CanUndoDelete);
            ReturnToMainCommand = new RelayCommand(OnReturnToMain);
            SelectedReportName = string.Empty;
        }

        public void PopulateFileList()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            string dirName = ConfigurationManager.AppSettings["report_save_config"].ToString();
            string[] rawReportNames = Directory.GetFiles(dirName);
            List<string> reportNames = new List<string>();
            foreach (var r in rawReportNames)
            {
                string[] splitFileName = r.Split('\\');
                reportNames.Add(splitFileName[splitFileName.Length - 1]);
            }
            SavedReportNames = new ObservableCollection<string>(reportNames);
        }

        private bool CanOpenReport()
        {
            return SelectedReportName != string.Empty;
        }

        private void OnOpenReport()
        {
            _fileHandler.OpenReport(SelectedReportName);
        }

        private bool CanDeleteReport()
        {
            return SelectedReportName != string.Empty;
        }

        private void OnDeleteReport()
        {
            _filesToDelete.Add(SelectedReportName);
            UndoDeleteCommand.RaiseCanExecuteChanged();
            SavedReportNames.Remove(SelectedReportName);
            SelectedReportName = string.Empty;
        }

        private bool CanUndoDelete()
        {
            return _filesToDelete.Count > 0;
        }

        private void OnUndoDelete()
        {
            foreach (var r in _filesToDelete)
            {
                SavedReportNames.Add(r);
            }
            _filesToDelete = new List<string>();
            UndoDeleteCommand.RaiseCanExecuteChanged();
        }

        private void OnReturnToMain()
        {
            foreach (var r in _filesToDelete) _fileHandler.DeleteReport(r);
            _filesToDelete = new List<string>();
            UndoDeleteCommand.RaiseCanExecuteChanged();
            Done();
        }

        public RelayCommand OpenReportCommand { get; private set; }
        public RelayCommand DeleteReportCommand { get; private set; }
        public RelayCommand UndoDeleteCommand { get; private set; }
        public RelayCommand ReturnToMainCommand { get; private set; }

        public Action Done = delegate { };
    }
}
