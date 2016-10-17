using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;

namespace Trivia.Login
{
    public class UserSession
    {
        private byte[] _hashedPasswordBytes = Array.ConvertAll(ConfigurationManager.AppSettings["password"].ToCharArray(), x => (byte)x);
        private byte[] _saltBytes = Array.ConvertAll(ConfigurationManager.AppSettings["salt"].ToCharArray(), y => (byte)y);
        private RNGCryptoServiceProvider _crypto = new RNGCryptoServiceProvider();

        public bool IsAuthenticated { get; private set; }

        public UserSession()
        {
            IsAuthenticated = false;
        }

        public void AuthenticateUser(string password)
        {
            if (_hashedPasswordBytes.Length == 0) throw new SettingsPropertyNotFoundException("Password not yet set");
            if (_saltBytes.Length == 0) throw new SettingsPropertyNotFoundException("No salt present; contact admin");

            string correctString = Convert.ToBase64String(_saltBytes) + "|" + Convert.ToBase64String(_hashedPasswordBytes);

            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, _saltBytes);
            deriveBytes.IterationCount = 10000;
            byte[] toValidateHash = deriveBytes.GetBytes(20);

            string toValidate = Convert.ToBase64String(_saltBytes) + "|" + Convert.ToBase64String(toValidateHash);

            bool? isValid = null;
            for (int i = 0; i < toValidateHash.Length; i++)
            {
                if (toValidate[i] != correctString[i]) isValid = false;
            }

            if (isValid == null) IsAuthenticated = true;
        }
    }
}
