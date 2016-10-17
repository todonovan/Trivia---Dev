using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel;

namespace Trivia.Login
{
    public class LoginViewModel : BindableBase
    {
        private UserSession _userSession = new UserSession();

        private string _typedPassword;
        public string TypedPassword
        {
            get { return _typedPassword; }
            set { SetProperty(ref _typedPassword, value); }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public LoginViewModel()
        {
            IsLoggedIn = _userSession.IsAuthenticated;
            LoginCommand = new RelayCommand<string>(AttemptAuthenticate, CanAuthenticate);
        }

        private bool CanAuthenticate(string password)
        {
            return (password != null && password.Length > 6 && IsLoggedIn != true);
        }

        private void AttemptAuthenticate(string password)
        {
            _userSession.AuthenticateUser(password);
            if (_userSession.IsAuthenticated) IsLoggedIn = true;
        }

        public RelayCommand<string> LoginCommand { get; private set; }
    }
}
