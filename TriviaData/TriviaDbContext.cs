using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;

namespace TriviaData
{
    public sealed class TriviaDbContext : IDisposable
    {
        private SQLiteConnection _dbContext;

        public SQLiteConnection Connection
        {
            get
            {
                return _dbContext;
            }
            private set { }
        }

        public TriviaDbContext()
        {
            _dbContext = new SQLiteConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
        }

        public void Open()
        {
            _dbContext.Open();
        }

        public void Close()
        {
            _dbContext.Close();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
