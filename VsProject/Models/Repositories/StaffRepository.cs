using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using VsProject.Models;


namespace VsProject.Models.Repositories
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

                if (GetById(staffModel.Id) == null)
                {
                    var id = UserPrincipal.Repository.GetById(staffModel.Id).Id;
                    if (id != null)
                    {

                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO "+ TableName + " (id, lastName, firstName, sex, phone, phoneAlt, email, birthDate) " +
                                              "VALUES (@id, @lastName, @firstName, @sex, @phone, @phoneAlt, @email, @birthDate)";


                        command.Parameters.AddWithValue("@id", id);
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
                command.CommandText = "UPDATE "+ TableName + " SET lastName=@lastName, firstName=@firstName, sex=@sex, phone=@phone, phoneAlt=@phoneAlt, email=@email, birthDate=@birthDate WHERE id=@id";
                command.Parameters.AddWithValue("@id", staffModel.Id);
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
                            Id = reader["Id"].DBValue<Guid>(),
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

        public StaffModel? GetById(Guid? id)
        {
            if(!IdExists(id))
            {
                return null;
            }
            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM "+ TableName + " WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        StaffModel staff = new StaffModel
                        {
                            Id = reader["Id"].DBValue<Guid>(),
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
        private bool IdExists(Guid? id)
        {
            if (id == null)
            {
                return false;
            }

            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM "+ TableName + " WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                //return (int)command.ExecuteScalar() > 0;
                return (long)command.ExecuteScalar() > 0;
            }
        }
    }
}
