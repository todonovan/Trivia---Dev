using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TriviaData.Models
{
    [DataContract()]
    public class Team
    {
        [DataMember()]
        public long Id { get; set; }
        [DataMember()]
        public string Name { get; set; }
        [DataMember()]
        public long Year { get; set; }
        [DataMember()]
        public string Company { get; set; }
        [DataMember()]
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Company: {Company}";
        }
    }
}
