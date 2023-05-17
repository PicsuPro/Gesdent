using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models;

namespace VsProject.Repositories
{
    public class PatientRepository : RepositoryBase, IPatientRepository
    {
        private const string TABLENAME = "Patient";
        private const string ID = "id";
        private const string LASTNAME = "last_name";
        private const string FIRSTNAME = "first_name";
        private const string SURNAME = "surname";
        private const string SEX = "sex";
        private const string PHONE = "phone";
        private const string PHONEALT = "phone_alt";
        private const string EMAIL = "email";
        private const string BIRTHDATE = "birth_date";
        private const string PROFESSION = "profession";
        private const string ADDRESS = "address";
        private const string MOTIVE = "motive";
        private const string ORIENTEDBY = "oriented_by";
        private const string PREFERREDDAY = "preferred_day";
        private const string PARENTNAME = "parent_name";

        public int Add(PatientModel patientModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                if (patientModel == null)
                {
                    throw new ArgumentNullException("user");
                }

                if (GetById(patientModel.Id) == null)
                {

                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO {TABLENAME} ( {LASTNAME}, {FIRSTNAME}, {SURNAME},{SEX},{PHONE},{PHONEALT},{EMAIL},{BIRTHDATE},{PROFESSION},{ADDRESS},{MOTIVE},{ORIENTEDBY},{PREFERREDDAY},{PARENTNAME}) " +
                                            "VALUES ( @lastName, @firstName, @surname, @sex, @phone, @phoneAlt, @email, @birthDate,@profession,@adress,@motive,@orientedBy,@preferredDay ,@parentName)";
                    command.Parameters.AddWithValue("@lastName", patientModel.LastName);
                    command.Parameters.AddWithValue("@firstName", patientModel.FirstName);
                    command.Parameters.AddWithValue("@surname", patientModel.Surname.DBNullOrWS());
                    command.Parameters.AddWithValue("@sex", patientModel.Sex);
                    command.Parameters.AddWithValue("@phone", patientModel.Phone);
                    command.Parameters.AddWithValue("@phoneAlt", patientModel.PhoneAlt.DBNullOrWS());
                    command.Parameters.AddWithValue("@email", patientModel.Email);
                    command.Parameters.AddWithValue("@birthDate", patientModel.BirthDate);
                    command.Parameters.AddWithValue("@profession", patientModel.Profession);
                    command.Parameters.AddWithValue("@adress", patientModel.Adress);
                    command.Parameters.AddWithValue("@motive", patientModel.Motive.DBNullOrWS());
                    command.Parameters.AddWithValue("@orientedBy", patientModel.OrientedBy.DBNullOrWS());
                    command.Parameters.AddWithValue("@preferredDay", patientModel.PreferredDay.DBNullOrWS());
                    command.Parameters.AddWithValue("@parentName", patientModel.ParentName.DBNullOrWS());
                    command.ExecuteNonQuery();
                    //get the generated ID
                    command.CommandText = "SELECT @@IDENTITY";
                    return Convert.ToInt32(command.ExecuteScalar());

                }
                else
                {
                    throw new ArgumentNullException("Patient already exists");
                }
            }
        }

        public void Edit(PatientModel patientModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"UPDATE {TABLENAME} SET {LASTNAME}=@lastName, {FIRSTNAME}=@firstName, {SURNAME}=@surname, {SEX}=@sex, {PHONE}=@phone, {PHONEALT}=@phoneAlt, {EMAIL}=@email, {BIRTHDATE}=@birthDate,{PROFESSION}=@profession,{ADDRESS}=@adress,{MOTIVE}=@motive,{ORIENTEDBY}=@orientedBy,{PREFERREDDAY}=@preferredDay,{PARENTNAME}=@parentName WHERE {ID}=@id";

                command.Parameters.AddWithValue("@id", patientModel.Id);
                command.Parameters.AddWithValue("@lastName", patientModel.LastName);
                command.Parameters.AddWithValue("@firstName", patientModel.FirstName);
                command.Parameters.AddWithValue("@surname", patientModel.Surname.DBNullOrWS());
                command.Parameters.AddWithValue("@sex", patientModel.Sex);
                command.Parameters.AddWithValue("@phone", patientModel.Phone);
                command.Parameters.AddWithValue("@phoneAlt", patientModel.PhoneAlt.DBNullOrWS());
                command.Parameters.AddWithValue("@email", patientModel.Email);
                command.Parameters.AddWithValue("@birthDate", patientModel.BirthDate);
                command.Parameters.AddWithValue("@profession", patientModel.Profession);
                command.Parameters.AddWithValue("@adress", patientModel.Adress);
                command.Parameters.AddWithValue("@motive", patientModel.Motive.DBNullOrWS());
                command.Parameters.AddWithValue("@orientedBy", patientModel.OrientedBy.DBNullOrWS());
                command.Parameters.AddWithValue("@preferredDay", patientModel.PreferredDay.DBNullOrWS());
                command.Parameters.AddWithValue("@parentName", patientModel.ParentName.DBNullOrWS());

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<PatientModel> GetAll()
        {
            var patients = new List<PatientModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM {TABLENAME}";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PatientModel patient = new PatientModel
                        {
                            Id = reader[ID].DBValue<int>(),
                            LastName = reader[LASTNAME].DBValue<string>(),
                            FirstName = reader[FIRSTNAME].DBValue<string>(),
                            Surname = reader[SURNAME].DBValue<string>(),
                            Sex = reader[SEX].DBValue<bool>(),
                            Phone = reader[PHONE].DBValue<string>(),
                            PhoneAlt = reader[PHONEALT].DBValue<string>(),
                            Email = reader[EMAIL].DBValue<string>(),
                            BirthDate = reader[BIRTHDATE].DBValue<DateTime>(),
                            Profession = reader[PROFESSION].DBValue<string>(),
                            Adress = reader[ADDRESS].DBValue<string>(),
                            Motive = reader[MOTIVE].DBValue<string>(),
                            OrientedBy = reader[ORIENTEDBY].DBValue<string>(),
                            PreferredDay = reader[PREFERREDDAY].DBValue<string>(),
                            ParentName = reader[PARENTNAME].DBValue<string>()
                        };
                        patients.Add(patient);
                    }
                }
            }

            return patients;
        }

        public PatientModel? GetById(int? id)
        {
            if (!IdExists(id))
            {
                return null;
            }
            using (var connection = GetConnection())
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
                        PatientModel patient = new PatientModel
                        {
                            Id = reader[ID].DBValue<int>(),
                            LastName = reader[LASTNAME].DBValue<string>(),
                            FirstName = reader[FIRSTNAME].DBValue<string>(),
                            Surname = reader[SURNAME].DBValue<string>(),
                            Sex = reader[SEX].DBValue<bool>(),
                            Phone = reader[PHONE].DBValue<string>(),
                            PhoneAlt = reader[PHONEALT].DBValue<string>(),
                            Email = reader[EMAIL].DBValue<string>(),
                            BirthDate = reader[BIRTHDATE].DBValue<DateTime>(),
                            Profession = reader[PROFESSION].DBValue<string>(),
                            Adress = reader[ADDRESS].DBValue<string>(),
                            Motive = reader[MOTIVE].DBValue<string>(),
                            OrientedBy = reader[ORIENTEDBY].DBValue<string>(),
                            PreferredDay = reader[PREFERREDDAY].DBValue<string>(),
                            ParentName = reader[PARENTNAME].DBValue<string>()
                        };
                        return patient;
                    }
                }
            }
            return null;
        }

        public void Remove(PatientModel patient)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM {TABLENAME} WHERE {ID}=@id";
                command.Parameters.AddWithValue("@id", patient.Id);
                command.ExecuteNonQuery();
            }
        }
        

        private bool IdExists(int? id)
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
                //return (long)command.ExecuteScalar() > 0;
            }
        }
    } 
}
