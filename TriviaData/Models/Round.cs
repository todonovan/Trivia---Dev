using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TriviaData.Models
{
    public class Round
    {
        public long Id { get; set; }
        public int RoundNumber { get; set; }
        public int NumQuestions { get; set; }
        public int PointValue { get; set; }
        public bool IsBonus { get; set; }
    }
}
