using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Media;
using System.Configuration;

namespace Trivia.Timer
{
    public class TimerWindowViewModel : BindableBase
    {
        private string _timeString;
        public string TimeString
        {
            get { return _timeString; }
            set { SetProperty(ref _timeString, value); }
        }

        private DispatcherTimer _timer;

        private TimeSpan _time;

        private bool _isTimerRunning;

        private SoundPlayer _soundPlayer;

        public TimerWindowViewModel()
        {
            OnResetTimer();
            _soundPlayer = new SoundPlayer(ConfigurationManager.AppSettings["sound_file_path"]);
            PauseUnpauseTimerCommand = new RelayCommand(OnPauseUnpauseTimer);
            ResetTimerCommand = new RelayCommand(OnResetTimer);
        }

        private void OnTimerFinish()
        {
            _timer.Stop();
            _soundPlayer.Play();
        }

        private void OnPauseUnpauseTimer()
        {
            if (_isTimerRunning)
            {
                _timer.Stop();
                _isTimerRunning = false;
            }
            else
            {
                _timer.Start();
                _isTimerRunning = true;
            }
        }

        private void OnResetTimer()
        {
            if (_timer != null) _timer.Stop();
            _isTimerRunning = false;
            _time = TimeSpan.FromSeconds(60);
            TimeString = _time.ToString(@"m\:ss");
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                TimeString = _time.ToString(@"m\:ss");
                if (_time == TimeSpan.Zero) OnTimerFinish();
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            _timer.Stop();
        }

        public RelayCommand PauseUnpauseTimerCommand { get; private set; }
        public RelayCommand ResetTimerCommand { get; private set; }
    }
}
