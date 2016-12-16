using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.ScoringHelpers;

namespace Trivia.GameSaving
{
    public class GameSaveParams
    {
        public string TeamName { get; set; }
        public List<List<Question>> Answers { get; set; }
    }
}
