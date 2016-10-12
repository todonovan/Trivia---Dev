using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData;
using TriviaData.Repos;
using System.Data.SQLite;
using TriviaData.Models;

namespace TriviaDataTester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TriviaDbContext context = new TriviaDbContext();
            context.Open();

            PersonRepository personRepo = new PersonRepository(context.Connection);

            Person p = new Person();
            p.FullName = @"'Diehl_Allison'";
            p.Address = @"'1204 Test Dr'";
            personRepo.Add(p);
            p = new Person();
            p.FullName = @"'ODonovan_Terry'";
            p.Address = @"'1114 Test Ave'";
            personRepo.Add(p);


            TeamRepository teamRepo = new TeamRepository(context.Connection);

            Team t = new Team();
            t.Name = @"'GPLCFun'";
            t.Members = new List<Person>();
            teamRepo.Add(t);

            t = teamRepo.GetTeamByName("\'GPLCFun\'");

            Person allison = personRepo.GetPersonByName("\'Diehl_Allison\'");
            Person terry = personRepo.GetPersonByName("\'ODonovan_Terry\'");

            t.Members = new List<Person>();
            t.Members.Add(allison);
            t.Members.Add(terry);

            teamRepo.Update(t);

            t = teamRepo.GetTeamByName("\'GPLCFun\'");

            ScorerRepository scorerRepo = new ScorerRepository(context.Connection);

            Scorer s = new Scorer();
            s.Teams = new List<Team>();
            s.Teams.Add(t);

            scorerRepo.Add(s);

            s = scorerRepo.GetScorerById(1);


            Console.WriteLine(t.Id + " " + t.Name + t.CreatedAt.ToString());
            foreach (var m in t.Members) Console.WriteLine(m.FullName + " ");
            Console.WriteLine(s.Id + " " + string.Join(" ", s.Teams) + " " + s.CreatedAt.ToString());
            Console.ReadLine();

            context.Close();
        }
    }
}
