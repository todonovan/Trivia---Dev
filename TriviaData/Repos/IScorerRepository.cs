using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;

namespace TriviaData.Repos
{
    public interface IScorerRepository : IApplicationRepository
    {
        void Add();
        void Add(Scorer scorer);
        void Remove(Scorer scorer);
        void Update(Scorer scorer);
        void AddTeamToScorer(Scorer scorer, Team team);
        void RemoveTeamFromScorer(Scorer scorer, Team team);
        Scorer GetScorerById(long id);
        List<Scorer> GetAllScorers();
    }
}
