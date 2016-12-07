using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TriviaData.Models
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Year { get; set; }
        public string Company { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Company: {Company}";
        }
    }
}
