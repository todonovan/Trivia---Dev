using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaData.Models
{
    public class Round
    {
        public int RoundNumber { get; set; }
        public int NumQuestions { get; set; }
        public int PointValue { get; set; }
        public bool IsBonus { get; set; }
    }
}
