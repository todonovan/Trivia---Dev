using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;

namespace Trivia.Scorers
{
    public class SimpleEditableScorer : ValidatableBindableBase
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

        private List<Team> _teams = new List<Team>();
        public List<Team> Teams
        {
            get { return _teams; }
            set { SetProperty(ref _teams, value); }
        }
    }
}
