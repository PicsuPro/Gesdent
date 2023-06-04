using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VsProject.Models;


namespace VsProject.Repositories
{

    public class StaffRepository : RepositoryBase, IStaffRepository
    {
        private const string TABLENAME = "staff";
        private const string USERID = "user_id";
        private const string SURNAME = "surname";
        private const string LASTNAME = "last_name";
        private const string FIRSTNAME = "first_name";
        private const string SEX = "sex";
        private const string PHONE = "phone";
        private const string PHONEALT = "phone_alt";
        private const string EMAIL = "email";
        private const string BIRTHDATE = "birth_date";

        public StaffRepository()
        {
        }

        public void Add(StaffModel staffModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            //using (var command = new SqlCommand())
            {
                if (staffModel == null)
                {
                    throw new ArgumentNullException("user");
                }

                if (GetByUserId(staffModel.UserId) == null)
                {
                    var userId = UserPrincipal.UserRepository.GetById(staffModel.UserId)?.Id;
                    if (userId != null)
                    {

                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = $"INSERT INTO {TABLENAME} ({USERID}, {SURNAME}, {LASTNAME}, {FIRSTNAME}, {SEX}, {PHONE}, {PHONEALT}, {EMAIL},{BIRTHDATE}) " +
                                              "VALUES (@userId, @surname, @lastName, @firstName, @sex, @phone, @phoneAlt, @email, @birthDate)";


                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@surname", staffModel.Surname);
                        command.Parameters.AddWithValue("@lastName", staffModel.LastName);
                        command.Parameters.AddWithValue("@firstName", staffModel.FirstName);
                        command.Parameters.AddWithValue("@sex", staffModel.Sex);
                        command.Parameters.AddWithValue("@phone", staffModel.Phone);
                        command.Parameters.AddWithValue("@phoneAlt", staffModel.PhoneAlt.DBNullOrWS());
                        command.Parameters.AddWithValue("@email", staffModel.Email);
                        command.Parameters.AddWithValue("@birthDate", staffModel.BirthDate.DBToDateTime());

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
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"UPDATE {TABLENAME} SET {SURNAME}=@surname,  {LASTNAME}=@lastName, {FIRSTNAME}=@firstName, {SEX}=@sex, {PHONE}=@phone, {PHONEALT}=@phoneAlt, {EMAIL}=@email, {BIRTHDATE}=@birthDate WHERE {USERID}=@userId";
                command.Parameters.AddWithValue("@userId", staffModel.UserId);
                command.Parameters.AddWithValue("@surname", staffModel.Surname);
                command.Parameters.AddWithValue("@lastName", staffModel.LastName);
                command.Parameters.AddWithValue("@firstName", staffModel.FirstName);
                command.Parameters.AddWithValue("@sex", staffModel.Sex);
                command.Parameters.AddWithValue("@phone", staffModel.Phone);
                command.Parameters.AddWithValue("@phoneAlt", staffModel.PhoneAlt.DBNullOrWS());
                command.Parameters.AddWithValue("@email", staffModel.Email);
                command.Parameters.AddWithValue("@birthDate", staffModel.BirthDate.DBToDateTime());

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<StaffModel> GetAll()
        {
            var staffs = new List<StaffModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM " + TABLENAME;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var staff = new StaffModel
                        {
                            UserId = reader[USERID].DBValue<Guid>(),
                            Surname = reader[SURNAME].DBValue<string>(),
                            LastName = reader[LASTNAME].DBValue<string>(),
                            FirstName = reader[FIRSTNAME].DBValue<string>(),
                            Sex = reader[SEX].DBValue<bool>(),
                            Phone = reader[PHONE].DBValue<string>(),
                            PhoneAlt = reader[PHONEALT].DBValue<string>(),
                            Email = reader[EMAIL].DBValue<string>(),
                            BirthDate = reader[BIRTHDATE].DBValue<DateOnly>()
                        };
                        staffs.Add(staff);
                    }
                }
            }

            return staffs;
        }

        public StaffModel? GetByUserId(Guid? userId)
        {
            if (!IdExists(userId))
            {
                return null;
            }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {SURNAME}, {LASTNAME}, {FIRSTNAME}, {SEX}, {PHONE}, {PHONEALT}, {EMAIL},{BIRTHDATE} FROM {TABLENAME} WHERE {USERID}=@userId";
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        StaffModel staff = new StaffModel
                        {
                            UserId = userId,
                            Surname = reader[SURNAME].DBValue<string>(),
                            LastName = reader[LASTNAME].DBValue<string>(),
                            FirstName = reader[FIRSTNAME].DBValue<string>(),
                            Sex = reader[SEX].DBValue<bool>(),
                            Phone = reader[PHONE].DBValue<string>(),
                            PhoneAlt = reader[PHONEALT].DBValue<string>(),
                            Email = reader[EMAIL].DBValue<string>(),
                            BirthDate = reader[BIRTHDATE].DBValue<DateOnly>()
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
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(*) FROM {TABLENAME} WHERE {USERID}=@userId";
                command.Parameters.AddWithValue("@userId", userId);

                return (int)command.ExecuteScalar() > 0;
                //return (long)command.ExecuteScalar() > 0;
            }
        }
    }
}
