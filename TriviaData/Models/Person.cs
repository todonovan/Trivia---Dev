using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaData.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string FullName { get; set; } //Last, First
        public string Address { get; set; }  //No formal structure provided
        public DateTime CreatedAt { get; set; }
    }
}
