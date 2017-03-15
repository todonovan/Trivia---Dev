using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Thermometer
{
    public class MainWindowViewModel : BindableBase
    {
        private int _maxValue;
        public int MaxValue
        {
            get { return _maxValue; }
            set { SetProperty(ref _maxValue, value); }
        }

        private int _currentValue;
        public int CurrentValue
        {
            get { return _currentValue; }
            set { SetProperty(ref _currentValue, value); }
        }

        public MainWindowViewModel()
        {
            DisplayThermometerCommand = new RelayCommand(OnDisplayThermometer);
            ResetCommand = new RelayCommand(OnReset);
        }

        private void OnDisplayThermometer()
        {
            Window thermWindow = new ThermometerWindow();
            thermWindow.DataContext = this;
            thermWindow.Show();
        }

        private void OnReset()
        {
            MaxValue = 0;
            CurrentValue = 0;
        }

        public RelayCommand DisplayThermometerCommand { get; private set; }
        public RelayCommand ResetCommand { get; private set; }
    }
}
