using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia.Sessions;

namespace Trivia.Scoring
{
    public class ScoringMasterViewModel : BindableBase
    {
        private GameSession _gameSession;

        public int NumRounds
        {
            get { return _gameSession.NumRounds; }
        }

        public ScoringMasterViewModel()
        {

        }

        public void SetGameSession(GameSession gs)
        {
            _gameSession = gs;
        }
    }
}
