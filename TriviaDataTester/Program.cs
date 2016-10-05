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

            /*personRepo.Add("\'Diehl_Allison\'");
            Person p = personRepo.GetPersonByName("\'Diehl_Allison\'");
            p.Address = "\'Doofus_Ave\'";
            personRepo.Update(p);
            */

            TeamRepository teamRepo = new TeamRepository(context.Connection);


            Team t = teamRepo.GetTeamByName("\'GPLCFun\'");

            Person allison = personRepo.GetPersonByName("\'Diehl_Allison\'");
            Person terry = personRepo.GetPersonByName("\'ODonovan_Terry\'");

            t.Members = new List<Person>();
            t.Members.Add(allison);
            t.Members.Add(terry);

            teamRepo.Update(t);

            t = teamRepo.GetTeamByName("\'GPLCFun\'");

            Console.WriteLine(t.Id + " " + t.Name + t.CreatedAt.ToString());
            foreach (var m in t.Members) Console.WriteLine(m.FullName + " ");
            Console.ReadLine();

            context.Close();
        }
    }
}
