using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using VsProject.Models;


namespace VsProject.Repositories
{
    public class StaffRepository : RepositoryBase, IStaffRepository
    {
        private const string TableName = "\"Staff\"";

        public StaffRepository()
        {
        }
        
        public void Add(StaffModel staffModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                if (staffModel == null)
                {
                    throw new ArgumentNullException("user");
                }

                if (GetById(staffModel.UserId) == null)
                {
                    var userId = UserPrincipal.Repository.GetById(staffModel.UserId).Id;
                    if (userId != null)
                    {

                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO "+ TableName + " (userId, lastName, firstName, sex, phone, phoneAlt, email, birthDate) " +
                                              "VALUES (@userId, @lastName, @firstName, @sex, @phone, @phoneAlt, @email, @birthDate)";


                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@lastName", staffModel.LastName);
                        command.Parameters.AddWithValue("@firstName", staffModel.FirstName);
                        command.Parameters.AddWithValue("@sex", staffModel.Sex);
                        command.Parameters.AddWithValue("@phone", staffModel.Phone);
                        command.Parameters.AddWithValue("@phoneAlt", staffModel.PhoneAlt.DBNullOrWS());
                        command.Parameters.AddWithValue("@email", staffModel.Email);
                        command.Parameters.AddWithValue("@birthDate", staffModel.BirthDate);

                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new ArgumentNullException("user does not exist");
                    }
                }
                else
                {
                    throw new ArgumentNullException("staff already exists");
                }
            }
        }

        public void Edit(StaffModel staffModel)
        {
            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE "+ TableName + " SET lastName=@lastName, firstName=@firstName, sex=@sex, phone=@phone, phoneAlt=@phoneAlt, email=@email, birthDate=@birthDate WHERE userId=@userId";
                command.Parameters.AddWithValue("@userId", staffModel.UserId);
                command.Parameters.AddWithValue("@lastName", staffModel.LastName);
                command.Parameters.AddWithValue("@firstName", staffModel.FirstName);
                command.Parameters.AddWithValue("@sex", staffModel.Sex);
                command.Parameters.AddWithValue("@phone", staffModel.Phone);
                command.Parameters.AddWithValue("@phoneAlt", staffModel.PhoneAlt.DBNullOrWS());
                command.Parameters.AddWithValue("@email", staffModel.Email);
                command.Parameters.AddWithValue("@birthDate", staffModel.BirthDate);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<StaffModel> GetAll()
        {
            var staffs = new List<StaffModel>();

            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM "+ TableName ;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StaffModel staff = new StaffModel
                        {
                            UserId = reader["UserId"].DBValue<Guid>(),
                            LastName = reader["lastName"].DBValue<string>(),
                            FirstName = reader["firstName"].DBValue<string>(),
                            Sex = reader["sex"].DBValue<bool>(),
                            Phone = reader["phone"].DBValue<string>(),
                            PhoneAlt = reader["phoneAlt"].DBValue<string>(),
                            Email = reader["Email"].DBValue<string>(),
                            BirthDate = reader["birthDate"].DBValue<DateTime>()
                        };
                        staffs.Add(staff);
                    }
                }
            }

            return staffs;
        }

        public StaffModel? GetById(Guid? userId)
        {
            if(!IdExists(userId))
            {
                return null;
            }
            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM "+ TableName + " WHERE UserId=@userId";
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        StaffModel staff = new StaffModel
                        {
                            UserId = reader["UserId"].DBValue<Guid>(),
                            LastName = reader["lastName"].DBValue<string>(),
                            FirstName = reader["firstName"].DBValue<string>(),
                            Sex = reader["sex"].DBValue<bool>(),
                            Phone = reader["phone"].DBValue<string>(),
                            PhoneAlt = reader["phoneAlt"].DBValue<string>(),
                            Email = reader["Email"].DBValue<string>(),
                            BirthDate = reader["birthDate"].DBValue<DateTime>()
                        };
                        return staff;
                    }
                }
            }
            return null;
        }
        private bool IdExists(Guid? userId)
        {
            if (userId == null)
            {
                return false;
            }

            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM "+ TableName + " WHERE UserId=@userId";
                command.Parameters.AddWithValue("@userId", userId);

                //return (int)command.ExecuteScalar() > 0;
                return (long)command.ExecuteScalar() > 0;
            }
        }
    }
}
