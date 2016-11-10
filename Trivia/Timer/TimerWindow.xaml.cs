using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Trivia.Timer
{
    /// <summary>
    /// Interaction logic for Timer.xaml
    /// </summary>
    public partial class TimerWindow : Window
    {
        public TimerWindow()
        {
            InitializeComponent();

            StartTimerCommand = new RelayCommand(OnStartTimer);
            ResetTimerCommand = new RelayCommand(OnResetTimer);
        }

        private void OnStartTimer()
        {

        }

        private void OnResetTimer()
        {

        }

        public RelayCommand StartTimerCommand { get; private set; }
        public RelayCommand ResetTimerCommand { get; private set; }
    }
}
