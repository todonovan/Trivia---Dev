using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia.ScoringHelpers
{
    public class RoundScoringParams
    {
        public GameState GameState { get; private set; }
        public int RoundNumber { get; private set; }

        public RoundScoringParams(GameState gs, int roundNumber)
        {
            GameState = gs;
            RoundNumber = roundNumber;
        }
    }
}
