using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using VsProject.Models;



namespace VsProject.Repositories
{
    using BCrypt.Net;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Text;

    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                if (userModel == null)
                {
                    throw new ArgumentNullException("user");
                }

                // Check if the username already exists
                if (GetByUsername(userModel.UserName) != null)
                {
                    throw new ArgumentNullException("user already exists");
                }


                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [User] (username, hash, salt, email) " +
                                      "VALUES (@username, @hash, @salt, @email)";

                string salt = BCrypt.GenerateSalt();
                string hash = BCrypt.HashPassword(userModel.Hash, salt);

                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                byte[] hashBytes = Encoding.UTF8.GetBytes(hash);

                command.Parameters.AddWithValue("@username", userModel.UserName);
                command.Parameters.AddWithValue("@hash", hashBytes);
                command.Parameters.AddWithValue("@salt", saltBytes);
                command.Parameters.AddWithValue("@email", userModel.Email);

                command.ExecuteNonQuery();
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT hash, salt FROM [User] WHERE username=@username";
                command.Parameters.AddWithValue("@username", credential.UserName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return false;
                    }

                    string hash = Encoding.UTF8.GetString((byte[])reader["hash"]);
                    string salt = Encoding.UTF8.GetString((byte[])reader["salt"]);
                    string hashedPassword = BCrypt.HashPassword(credential.Password, salt);
                    validUser = string.Equals(hashedPassword, hash);
                }
            }

            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET username=@username, hash=@hash, salt=@halt, email=@email WHERE id=@id";

                string salt = BCrypt.GenerateSalt();
                string hash = BCrypt.HashPassword(userModel.Hash, salt);

                command.Parameters.AddWithValue("@username", userModel.UserName);
                command.Parameters.AddWithValue("@hash", hash);
                command.Parameters.AddWithValue("@salt", salt);
                command.Parameters.AddWithValue("@email", userModel.Email);
                command.Parameters.AddWithValue("@id", userModel.Id);

                command.ExecuteNonQuery();
            }
        }

        public UserModel GetByUsername(string username)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE username = @username";
                command.Parameters.AddWithValue("@username", username);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserModel userModel = new UserModel
                        {
                            Id = (SqlGuid)(reader["id"]),
                            UserName = (string)reader["username"],
                            Hash = Encoding.UTF8.GetString((byte[])reader["hash"]),
                            Email = (string)(reader["Email"]),
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

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            Id = (SqlGuid)reader["Id"],
                            UserName = (string)reader["username"],
                            Email = (string)reader["email"]
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public UserModel GetById(int id)
        {
            UserModel user = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel
                        {
                            Id = (SqlGuid)reader["Id"],
                            UserName = (string)reader["Username"],
                            Hash = (string)reader["hash"],
                            Email = (string)reader["Email"]
                        };
                        
                    }
                }
                return user;
            }

        }
            public void Remove(UserModel userModel)
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [User] WHERE Id=@id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = userModel.Id;
                    command.ExecuteNonQuery();
                }
            }
        }

    }