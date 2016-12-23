using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Practices.Unity;

namespace Trivia.Thermometer
{
    public class ThermometerWindowViewModel : BindableBase
    {
        private AdminPanelViewModel _adminPanelViewModel;

        private int _targetValue;
        public int TargetValue
        {
            get { return _targetValue; }
            set { SetProperty(ref _targetValue, value); }
        }

        private int _currentValue;
        public int CurrentValue
        {
            get { return _currentValue; }
            set { SetProperty(ref _currentValue, value); }
        }

        public ThermometerWindowViewModel()
        {
            _adminPanelViewModel = ContainerHelper.Container.Resolve<AdminPanelViewModel>();

            ShowAdminPanelCommand = new RelayCommand(OnShowAdminPanel);
        }

        private void OnShowAdminPanel()
        {

        }

        public RelayCommand ShowAdminPanelCommand { get; private set; }
    }
}
