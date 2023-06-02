using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using VsProject.Models;


namespace VsProject.Repositories
{


    public class UserRepository : RepositoryBase, IUserRepository
    {
        private const string TABLENAME = "[user]";
        private const string ID = "id";
        private const string USERNAME = "username";
        private const string HASH = "hash";
        private const string SALT = "salt";
        private const string EMAIL = "email";
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {HASH}, {SALT} FROM {TABLENAME} WHERE {USERNAME}=@username";
                command.Parameters.AddWithValue("@username", credential.UserName);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return false;
                    }

                    string hash = System.Text.Encoding.UTF8.GetString((byte[])reader[HASH]);
                    string salt = System.Text.Encoding.UTF8.GetString((byte[])reader[SALT]);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(credential.Password, salt);
                    validUser = BCrypt.Net.BCrypt.Verify(credential.Password, hash);
                }
            }

            return validUser;
        }

        public void Add(UserModel userModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                if (userModel == null)
                {
                    throw new ArgumentNullException("user");
                }

                if (GetByUsername(userModel.UserName) == null)
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO {TABLENAME} ({USERNAME}, {HASH}, {SALT}, {EMAIL}) VALUES (@username, @hash, @salt, @email)";

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
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                if (!string.IsNullOrWhiteSpace(userModel.Hash))
                {
                    command.CommandText = $"UPDATE {TABLENAME} SET {USERNAME}=@username, {HASH}=@hash, {SALT}=@salt, {EMAIL}=@email WHERE {ID}=@id";
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hash = BCrypt.Net.BCrypt.HashPassword(userModel.Hash, salt);
                    byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
                    byte[] hashBytes = System.Text.Encoding.UTF8.GetBytes(hash);
                    command.Parameters.AddWithValue("@hash", hashBytes);
                    command.Parameters.AddWithValue("@salt", saltBytes);
                }
                else
                {
                    command.CommandText = $"UPDATE {TABLENAME} SET {USERNAME}=@username, {EMAIL}=@email WHERE {ID}=@id";
                }
                command.Parameters.AddWithValue("@username", userModel.UserName);
                command.Parameters.AddWithValue("@email", userModel.Email.DBNullOrWS());
                command.Parameters.AddWithValue("@id", userModel.Id);

                command.ExecuteNonQuery();
            }
        }

        public void Remove(UserModel userModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM {TABLENAME} WHERE {ID}=@id";
                command.Parameters.AddWithValue("@id", userModel.Id);
                command.ExecuteNonQuery();
            }
        }

        public UserModel? GetByUsername(string? username)
        {
            if (!UsernameExists(username))
            {
                return null;
            }
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM {TABLENAME} WHERE {USERNAME}=@username";
                command.Parameters.AddWithValue("@username", username);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserModel userModel = new UserModel
                        {
                            Id = reader[ID].DBValue<Guid>(),
                            UserName = reader[USERNAME].DBValue<string>(),
                            Hash = System.Text.Encoding.UTF8.GetString(reader[HASH].DBValue<byte[]>()),
                            Email = reader[EMAIL].DBValue<string>()
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
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM {TABLENAME}";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            Id = reader[ID].DBValue<Guid>(),
                            UserName = reader[USERNAME].DBValue<string>(),
                            Email = reader[EMAIL].DBValue<string>()
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
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM {TABLENAME} WHERE {ID}=@id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            Id = reader[ID].DBValue<Guid>(),
                            UserName = reader[USERNAME].DBValue<string>(),
                            Hash = System.Text.Encoding.UTF8.GetString(reader[HASH].DBValue<byte[]>()),
                            Email = reader[EMAIL].DBValue<string>()
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

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(*) FROM {TABLENAME} WHERE {ID}=@id";
                command.Parameters.AddWithValue("@id", id);

                return (int)command.ExecuteScalar() > 0;
            }
        }


        private bool UsernameExists(string? username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(*) FROM {TABLENAME} WHERE  {USERNAME}=@username";
                command.Parameters.AddWithValue("@username", username);

                return (int)command.ExecuteScalar() > 0;
            }
        }

    }

}