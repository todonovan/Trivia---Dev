﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        void Add(Team team);
        void Add(List<Person> members);
        void Remove(Team team);
        void Update(Team team);
        Team GetTeamById(long id);
        Team GetTeamByIdNoPlayers(long id);
        ObservableCollection<Team> GetAllTeams();
        Team GetTeamByName(string name);
        ObservableCollection<Team> FindTeamsByYear(long year);
        ObservableCollection<Team> FindTeamsByYearNoPlayers(long year);
    }
}