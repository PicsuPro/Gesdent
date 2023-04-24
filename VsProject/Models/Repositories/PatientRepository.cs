using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models;

namespace VsProject.Models.Repositories
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
                    var id = UserPrincipal.Repository.GetById(patientModel.Id).Id;
                    if (id != null)
                    {

                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO " + TableName + " (id, lastName, firstName, sex, phone, phoneAlt, email, birthDate,profession,adresse,pattern,preferredDay,parentName) " +
                                              "VALUES (@id, @lastName, @firstName, @sex, @phone, @phoneAlt, @email, @birthDate,@profession,@adresse,@pattern,@preferredDay,@parentName)";


                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@lastName", patientModel.LastName);
                        command.Parameters.AddWithValue("@firstName", patientModel.FirstName);
                        command.Parameters.AddWithValue("@sex", patientModel.Sex);
                        command.Parameters.AddWithValue("@phone", patientModel.Phone);
                        command.Parameters.AddWithValue("@phoneAlt", patientModel.PhoneAlt.DBNullOrWS());
                        command.Parameters.AddWithValue("@email", patientModel.Email);
                        command.Parameters.AddWithValue("@birthDate", patientModel.BirthDate);
                        command.Parameters.AddWithValue("@profession", patientModel.Profession);
                        command.Parameters.AddWithValue("@adresse", patientModel.Adresse);
                        command.Parameters.AddWithValue("@pattern", patientModel.Pattern);
                        command.Parameters.AddWithValue("@preferredDay", patientModel.PreferredDay);
                        command.Parameters.AddWithValue("@parentName", patientModel.ParentName);


                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new ArgumentNullException("Patient does not exist");
                    }
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
                command.CommandText = "UPDATE " + TableName + " SET lastName=@lastName, firstName=@firstName, sex=@sex, phone=@phone, phoneAlt=@phoneAlt, email=@email, birthDate=@birthDate,profession=@profession,adresse=@adresse,pattern=@pattern,preferredDay=@preferredDay,parentName=@parentName WHERE id=@id";

                command.Parameters.AddWithValue("@id", patientModel.Id);
                command.Parameters.AddWithValue("@lastName", patientModel.LastName);
                command.Parameters.AddWithValue("@firstName", patientModel.FirstName);
                command.Parameters.AddWithValue("@sex", patientModel.Sex);
                command.Parameters.AddWithValue("@phone", patientModel.Phone);
                command.Parameters.AddWithValue("@phoneAlt", patientModel.PhoneAlt.DBNullOrWS());
                command.Parameters.AddWithValue("@email", patientModel.Email);
                command.Parameters.AddWithValue("@birthDate", patientModel.BirthDate);
                command.Parameters.AddWithValue("@profession", patientModel.Profession);
                command.Parameters.AddWithValue("@adresse", patientModel.Adresse);
                command.Parameters.AddWithValue("@pattern", patientModel.Pattern);
                command.Parameters.AddWithValue("@preferredDay", patientModel.PreferredDay);
                command.Parameters.AddWithValue("@parentName", patientModel.ParentName);

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
                            Id = reader["Id"].DBValue<Guid>(),
                            LastName = reader["lastName"].DBValue<string>(),
                            FirstName = reader["firstName"].DBValue<string>(),
                            Sex = reader["sex"].DBValue<bool>(),
                            Phone = reader["phone"].DBValue<string>(),
                            PhoneAlt = reader["phoneAlt"].DBValue<string>(),
                            Email = reader["Email"].DBValue<string>(),
                            BirthDate = reader["birthDate"].DBValue<DateTime>(),
                            Profession = reader["profession"].DBValue<string>(),
                            Adresse = reader["adresse"].DBValue<string>(),
                            Pattern = reader["pattern"].DBValue<string>(),
                            PreferredDay = reader["preferredDay"].DBValue<string>(),
                            ParentName = reader["parentName"].DBValue<string>()
                        };
                        patients.Add(patient);
                    }
                }
            }

            return patients;
        }

        PatientModel? IPatientRepository.GetById(Guid? id)
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
                            Id = reader["Id"].DBValue<Guid>(),
                            LastName = reader["lastName"].DBValue<string>(),
                            FirstName = reader["firstName"].DBValue<string>(),
                            Sex = reader["sex"].DBValue<bool>(),
                            Phone = reader["phone"].DBValue<string>(),
                            PhoneAlt = reader["phoneAlt"].DBValue<string>(),
                            Email = reader["Email"].DBValue<string>(),
                            BirthDate = reader["birthDate"].DBValue<DateTime>(),
                            Profession = reader["profession"].DBValue<string>(),
                            Adresse = reader["adresse"].DBValue<string>(),
                            Pattern = reader["pattern"].DBValue<string>(),
                            PreferredDay = reader["preferredDay"].DBValue<string>(),
                            ParentName = reader["parentName"].DBValue<string>()
                        };
                        return patient;
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
                command.CommandText = "SELECT COUNT(*) FROM " + TableName + " WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                //return (int)command.ExecuteScalar() > 0;
                return (long)command.ExecuteScalar() > 0;
            }
        }
    } 
}
