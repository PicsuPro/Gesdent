using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace VsProject.Models.Repositories
{

    public static class DbExtensions
    {

        public static T? DBValue<T>(this object obj)
        {
            if (obj == DBNull.Value)
                return default(T);

            return (T)obj;
        }
    }

    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString = "Server=.\\SQLEXPRESS;Database=Gesdentdb;Trusted_Connection=True;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
