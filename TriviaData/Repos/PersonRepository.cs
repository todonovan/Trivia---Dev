using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaData.Models;
using System.Data.SQLite;

namespace TriviaData.Repos
{
    public class PersonRepository : IPersonRepository
    {
        private SQLiteConnection _dbConn;
        public PersonRepository(SQLiteConnection dbConn)
        {
            _dbConn = dbConn;
        }
        public void Add()
        {
            string dateString = DateTime.Now.ToString();
            string sql = $"INSERT INTO People (full_name, address, created_at) VALUES (\'\', \'\', {dateString}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public void Add(string fullName)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            string sql = $"INSERT INTO People (full_name, address, created_at) VALUES ({fullName}, \'\', {dateString})";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public Person GetPersonById(long id)
        {
            string sql = $"SELECT * FROM People WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            Person p = new Person();
            p.Id = id;
            p.FullName = (string)reader["full_name"];
            p.Address = (string)reader["address"];
            p.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));

            command.Dispose();
            return p;
        }

        public Person GetPersonByName(string fullName)
        {
            string sql = $"SELECT * FROM People WHERE full_name={fullName}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            SQLiteDataReader reader = command.ExecuteReader();

            Person p = new Person();
            p.Id = (long)reader["id"];
            p.FullName = fullName;
            p.Address = (string)reader["address"];
            p.CreatedAt = new DateTime(long.Parse((string)reader["created_at"]));

            command.Dispose();
            return p;
        }

        public void Remove(Person person)
        {
            long id = person.Id;
            string sql = $"DELETE FROM People WHERE id={id}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public void Update(Person person)
        {
            string dateString = person.CreatedAt.Ticks.ToString();
            string sql = $"UPDATE People SET full_name={person.FullName}, address={person.Address}, created_at={dateString} WHERE id={person.Id.ToString()}";
            SQLiteCommand command = new SQLiteCommand(sql, _dbConn);
            command.ExecuteNonQuery();

            command.Dispose();
        }
    }
}
