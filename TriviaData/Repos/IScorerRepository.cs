using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;

namespace TriviaData.Repos
{
    public interface IScorerRepository
    {
        void Add();
        void Add(Scorer scorer);
        void Add(List<Team> teams);
        void Remove(Scorer scorer);
        void Update(Scorer scorer);
        Scorer GetScorerById(long id);
        List<Scorer> GetAllScorers();
    }
}
