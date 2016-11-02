using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TriviaData.Models
{
    [DataContract()]
    public class Scorer
    {
        [DataMember()]
        public long Id { get; set; }
        [DataMember()]
        public string Name { get; set; }
        [DataMember()]
        public List<Team> Teams { get; set; }
        [DataMember()]
        public DateTime CreatedAt { get; set; }
    }
}
