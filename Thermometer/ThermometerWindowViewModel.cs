using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermometer
{
    public class ThermometerWindowViewModel : BindableBase
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

        public void InitializeParameters(int maxValue, int currentValue)
        {
            MaxValue = maxValue;
            CurrentValue = currentValue;
        }
    }
}
