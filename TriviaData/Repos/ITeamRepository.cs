using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;

namespace TriviaData.Repos
{
    public interface ITeamRepository : IApplicationRepository
    {
        void Add();
        void Add(string name);
        void Add(Team team);
        void Remove(Team team);
        void Update(Team team);
        Team GetTeamById(long id);
        Team GetTeamByCompany(string companyName);
        List<Team> GetAllTeams();
        Team GetTeamByName(string name);
        List<Team> FindTeamsByYear(long year);
        void AddTeamToScorer(Scorer scorer, Team team);
        void RemoveTeamFromScorer(Scorer scorer, Team team);
    }
}
