using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaData.Models
{
    public class Scorer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
