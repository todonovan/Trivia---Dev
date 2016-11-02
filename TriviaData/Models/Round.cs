using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TriviaData.Models
{
    [DataContract()]
    public class Round
    {
        [DataMember()]
        public long Id { get; set; }
        [DataMember()]
        public int RoundNumber { get; set; }
        [DataMember()]
        public int NumQuestions { get; set; }
        [DataMember()]
        public int PointValue { get; set; }
        [DataMember()]
        public bool IsBonus { get; set; }
    }
}
