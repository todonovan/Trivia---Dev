using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;

namespace TriviaData.Repos
{
    public interface ITeamRepository
    {
        void Add();
        void Add(string name);
        void Add(List<Person> members);
        void Remove(Team team);
        void Update(Team team);
        Team GetTeamById(long id);
        Team GetTeamByName(string name);
        List<Team> FindTeamsByYear(long year);
    }
}
