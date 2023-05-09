using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace VsProject.Repositories
{

    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString = "Server=(local)\\SQLEXPRESS;Database=Gesdentdb;Trusted_Connection=True;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
            //return new NpgsqlConnection(_connectionString);
        }


    }

    public static class DBExtensions
    {

        public static T? DBValue<T>(this object obj)
        {
            return (obj == DBNull.Value) ? default : (T)obj;
        }
        public static object? DBNullOrWS(this string? s)
        {
            return string.IsNullOrWhiteSpace(s) ? DBNull.Value : s;
        }
    }

}
