using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaData.Models
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Year { get; set; }
        public List<Person> Members { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            string members = string.Join(", ", Members);
            return $"Name: {Name}, Members: {members}";
        }
    }
}
