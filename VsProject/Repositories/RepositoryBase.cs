using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace VsProject.Repositories
{

    public static class DbExtensions
    {

        public static T? DBValue<T>(this object obj)
        {
            if (obj == DBNull.Value)
                return default;

            return (T)obj;
        }
        public static object? DBNullOrWS(this string? s)
        {
            return string.IsNullOrWhiteSpace(s) ? DBNull.Value : s;
        }
    }

    public abstract class RepositoryBase
    {
        private const string _connectionString = "Server=(local)\\SQLEXPRESS;Database=Gesdentdb;Trusted_Connection=True;";

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
            //return new NpgsqlConnection(_connectionString);
        }


    }
}
