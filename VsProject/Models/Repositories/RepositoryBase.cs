using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Npgsql;

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
        public static object? DBNullOrWS(this string? s)
        {
            return string.IsNullOrWhiteSpace(s) ? DBNull.Value : s;
        }
    }

    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString = "Server=rogue.db.elephantsql.com;Database=mifzwhkc;User Id=mifzwhkc;Password=sD46Q-Pg0-r_KL0gTjx2lxXhqJAltQ77;";
        }

        protected NpgsqlConnection GetPGConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
