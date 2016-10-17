using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Trivia.Teams
{
    public class SimpleEditableTeam : ValidatableBindableBase
    {
        private long _id;
        public long Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private long _year;
        [Required]
        [MaxLength(4)]
        public long Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }

        private string _company;
        [Required]
        public string Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }
    }
}
