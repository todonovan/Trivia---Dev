using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;

namespace TriviaData.Repos
{
    public interface IPersonRepository
    {
        void Add();
        void Add(Person person);
        void Add(string fullName);
        void Remove(Person person);
        void Update(Person person);
        Person GetPersonById(long id);
        Person GetPersonByName(string fullName);
        List<Person> GetAllPeople();
    }
}
