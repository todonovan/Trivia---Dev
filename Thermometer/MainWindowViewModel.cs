using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermometer
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            DisplayThermometerCommand = new RelayCommand(OnDisplayThermometer);
        }

        private void OnDisplayThermometer()
        {

        }

        public RelayCommand DisplayThermometerCommand { get; private set; }
    }
}
