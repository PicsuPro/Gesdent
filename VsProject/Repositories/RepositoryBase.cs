using System;
using System.Collections.ObjectModel;
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

            if (typeof(T) == typeof(DateOnly) && obj is DateTime dt1)
                return (T)(object)DateOnly.FromDateTime(dt1);
            
            if (typeof(T) == typeof(TimeOnly) && obj is TimeSpan ts)
                return (T)(object)TimeOnly.FromTimeSpan(ts);

            return (T)obj;
        }

        public static object? DBNullOrWS(this string? s)
        {
            return string.IsNullOrWhiteSpace(s) ? DBNull.Value : s;
        }
        public static DateTime DBToDateTime(this DateOnly d)
        {
            return d.ToDateTime(new TimeOnly());
        }
        public static TimeSpan DBToTimeSpan(this TimeOnly t)
        {
            return t.ToTimeSpan();
        }

        public static ObservableCollection<string>? ConvertToObservableCollection(string separator, string? joinedString)
        {
            if (joinedString == null)
                return null;
            ObservableCollection<string> collection = new ObservableCollection<string>();

            string[] strings = joinedString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in strings)
            {
                collection.Add(value);
            }

            return collection;
        }
    }

}
