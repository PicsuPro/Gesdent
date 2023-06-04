using System;
using System.Data.SqlClient;

namespace VsProject.Repositories
{

    public abstract class RepositoryBase
    {
        private const string _connectionString = "Server=(local)\\SQLEXPRESS;Database=Gesdentdb;Trusted_Connection=True;";

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
            //return new NpgsqlConnection(_connectionString);
        }


    }

    static class DBExtensions
    {

        public static T? DBValue<T>(this object obj)
        {
            if (obj == DBNull.Value)
                return default;

            if (typeof(T) == typeof(DateOnly) && obj is DateTime dt)
                return (T)(object)DateOnly.FromDateTime(dt);

            return (T)obj;
        }

        public static object? DBNullOrWS(this string? s)
        {
            return string.IsNullOrWhiteSpace(s) ? DBNull.Value : s;
        }
        public static DateTime? DBToDateTime(this DateOnly d)
        {
            return d.ToDateTime(new TimeOnly());
        }
    }

}
