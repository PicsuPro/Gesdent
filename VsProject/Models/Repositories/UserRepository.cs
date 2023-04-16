using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using VsProject.Models;


namespace VsProject.Models.Repositories
{
    

    public class UserRepository : RepositoryBase, IUserRepository
    {

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;

            using (var connection = GetPGConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT hash, salt FROM \"User\" WHERE username=@username";
                command.Parameters.AddWithValue("@username", credential.UserName);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return false;
                    }

                    string hash = System.Text.Encoding.UTF8.GetString((byte[])reader["hash"]);
                    string salt = System.Text.Encoding.UTF8.GetString((byte[])reader["salt"]);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(credential.Password, salt);
                    validUser = BCrypt.Net.BCrypt.Verify(credential.Password, hash);
                }
            }

            return validUser;
        }

        public void Add(UserModel userModel)
        {
            using (var connection = GetPGConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                if (userModel == null)
                {
                    throw new ArgumentNullException("user");
                }

                if (GetByUsername(userModel.UserName) == null)
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"User\" (username, hash, salt, email) " +
                                          "VALUES (@username, @hash, @salt, @email)";

                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hash = BCrypt.Net.BCrypt.HashPassword(userModel.Hash, salt);

                    byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
                    byte[] hashBytes = System.Text.Encoding.UTF8.GetBytes(hash);

                    command.Parameters.AddWithValue("@username", userModel.UserName);
                    command.Parameters.AddWithValue("@hash", hashBytes);
                    command.Parameters.AddWithValue("@salt", saltBytes);
                    command.Parameters.AddWithValue("@email", userModel.Email.DBNullOrWS());

                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new ArgumentNullException("user already exists");
                }
            }
        }



        public void Edit(UserModel userModel)
        {
            using (var connection = GetPGConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                if (!string.IsNullOrWhiteSpace(userModel.Hash))
                {
                    command.CommandText = "UPDATE \"User\" SET username=@username, hash=@hash, salt=@salt, email=@email WHERE id=@id";
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hash = BCrypt.Net.BCrypt.HashPassword(userModel.Hash, salt);
                    byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
                    byte[] hashBytes = System.Text.Encoding.UTF8.GetBytes(hash);
                    command.Parameters.AddWithValue("@hash", hashBytes);
                    command.Parameters.AddWithValue("@salt", saltBytes);
                }else
                {
                    command.CommandText = "UPDATE \"User\" SET username=@username, email=@email WHERE id=@id";
                }
                command.Parameters.AddWithValue("@username", userModel.UserName);
                command.Parameters.AddWithValue("@email", userModel.Email.DBNullOrWS());
                command.Parameters.AddWithValue("@id", userModel.Id);

                command.ExecuteNonQuery();
            }
        }

        public void Remove(UserModel userModel)
        {
            using (var connection = GetPGConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM \"User\" WHERE Id=@id";
                command.Parameters.AddWithValue("@id", userModel.Id);
                command.ExecuteNonQuery();
            }
        }

        public UserModel? GetByUsername(string? username)
        {
            if(!UsernameExists(username)) 
            {
                return null;
            }
            using (var connection = GetPGConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"User\" WHERE username = @username";
                command.Parameters.AddWithValue("@username", username);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserModel userModel = new UserModel
                        {
                            Id = reader["Id"].DBValue<Guid>(),
                            UserName = reader["username"].DBValue<string>(),
                            Hash = System.Text.Encoding.UTF8.GetString(reader["hash"].DBValue<byte[]>()),
                            Email = reader["Email"].DBValue<string>()
                        };
                        return userModel;
                    }
                }
            }
            return null;
        }


        public IEnumerable<UserModel> GetAll()
        {
            var users = new List<UserModel>();

            using (var connection = GetPGConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"User\"";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            Id = reader["Id"].DBValue<Guid>(),
                            UserName = reader["username"].DBValue<string>(),
                            Email = reader["Email"].DBValue<string>()
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public UserModel? GetById(Guid? id)
        {
            if (!IdExists(id))
            {
                return null;
            }
            using (var connection = GetPGConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"User\" WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            Id = reader["Id"].DBValue<Guid>(),
                            UserName = reader["username"].DBValue<string>(),
                            Hash = System.Text.Encoding.UTF8.GetString(reader["hash"].DBValue<byte[]>()),
                            Email = reader["Email"].DBValue<string>()
                        };
                        return user;
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

            using (var connection = GetPGConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM \"User\" WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                return (long)command.ExecuteScalar() > 0;
            }
        }


        private bool UsernameExists(string? username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            using (var connection = GetPGConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM \"User\" WHERE username = @username";
                command.Parameters.AddWithValue("@username", username);

                return (long)command.ExecuteScalar() > 0;
            }
        }

    }

}