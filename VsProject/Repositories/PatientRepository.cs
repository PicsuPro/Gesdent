using Npgsql;
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
        private const string TableName = "\"Patient\"";

        public void Add(PatientModel patientModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new NpgsqlCommand())
            {
                if (patientModel == null)
                {
                    throw new ArgumentNullException("user");
                }

                if (((IPatientRepository)this).GetById(patientModel.Id) == null)
                {

                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO " + TableName + " ( lastName, firstName, surname,sex,phone,phoneAlt,email,birthDate,profession,adress,motive,orientedBy,preferredDay  ,parentName) " +
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
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE " + TableName + " SET lastName=@lastName, firstName=@firstName, surname=@surname, sex=@sex, phone=@phone, phoneAlt=@phoneAlt, email=@email, birthDate=@birthDate,profession=@profession,adress=@adress,motive=@motive,orientedBy=@orientedBy,preferredDay=@preferredDay,parentName=@parentName WHERE id=@id";

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
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM " + TableName;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PatientModel patient = new PatientModel
                        {
                            Id = reader["Id"].DBValue<int>(),
                            LastName = reader["lastName"].DBValue<string>(),
                            FirstName = reader["firstName"].DBValue<string>(),
                            Surname = reader["surname"].DBValue<string>(),
                            Sex = reader["sex"].DBValue<bool>(),
                            Phone = reader["phone"].DBValue<string>(),
                            PhoneAlt = reader["phoneAlt"].DBValue<string>(),
                            Email = reader["Email"].DBValue<string>(),
                            BirthDate = reader["birthDate"].DBValue<DateTime>(),
                            Profession = reader["profession"].DBValue<string>(),
                            Adress = reader["adress"].DBValue<string>(),
                            Motive = reader["motive"].DBValue<string>(),
                            OrientedBy = reader["orientedBy"].DBValue<string>(),
                            PreferredDay = reader["preferredDay"].DBValue<string>(),
                            ParentName = reader["parentName"].DBValue<string>()
                        };
                        patients.Add(patient);
                    }
                }
            }

            return patients;
        }

        PatientModel? IPatientRepository.GetById(int? id)
        {
            if (!IdExists(id))
            {
                return null;
            }
            using (var connection = GetConnection())
            using (var command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM " + TableName + " WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        PatientModel patient = new PatientModel
                        {
                            Id = reader["Id"].DBValue<int>(),
                            LastName = reader["lastName"].DBValue<string>(),
                            FirstName = reader["firstName"].DBValue<string>(),
                            Surname = reader["surname"].DBValue<string>(),
                            Sex = reader["sex"].DBValue<bool>(),
                            Phone = reader["phone"].DBValue<string>(),
                            PhoneAlt = reader["phoneAlt"].DBValue<string>(),
                            Email = reader["Email"].DBValue<string>(),
                            BirthDate = reader["birthDate"].DBValue<DateTime>(),
                            Profession = reader["profession"].DBValue<string>(),
                            Adress = reader["adress"].DBValue<string>(),
                            Motive = reader["motive"].DBValue<string>(),
                            OrientedBy = reader["orientedBy"].DBValue<string>(),
                            PreferredDay = reader["preferredDay"].DBValue<string>(),
                            ParentName = reader["parentName"].DBValue<string>()
                        };
                        return patient;
                    }
                }
            }
            return null;
            
        }
        private bool IdExists(int? id)
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
                command.CommandText = "SELECT COUNT(*) FROM " + TableName + " WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                //return (int)command.ExecuteScalar() > 0;
                return (long)command.ExecuteScalar() > 0;
            }
        }
    } 
}
