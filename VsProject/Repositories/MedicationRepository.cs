using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VsProject.Models;

namespace VsProject.Repositories
{
    public class MedicationRepository : RepositoryBase, IMedicationRepository
    {
        private const string TABLENAME = "medication";
        private const string ID = "id";
        private const string NAME = "name";

        public int Add(MedicationModel medicationModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                if (medicationModel == null)
                {
                    throw new ArgumentNullException("medication");
                }

                if (GetById(medicationModel.Id) == null)
                {

                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO {TABLENAME} ( {NAME}) " +
                                            "VALUES ( @name)";
                    command.Parameters.AddWithValue("@name", medicationModel.Name);
                    command.ExecuteNonQuery();
                    //get the generated ID
                    command.CommandText = "SELECT @@IDENTITY";
                    int id = Convert.ToInt32(command.ExecuteScalar());
                    medicationModel.Id = id;
                    return id;

                }
                else
                {
                    throw new ArgumentNullException("medication already exists");
                }
            }
        }

        public void Edit(MedicationModel medicationModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"UPDATE {TABLENAME} SET {NAME}=@name WHERE {ID}=@id";

                command.Parameters.AddWithValue("@id", medicationModel.Id);
                command.Parameters.AddWithValue("@name", medicationModel.Name);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<MedicationModel> GetAll()
        {
            var medications = new List<MedicationModel>();

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
                        MedicationModel medicationModel = new MedicationModel
                        {
                            Id = reader[ID].DBValue<int>(),
                            Name = reader[NAME].DBValue<string>(),
                        };
                        medications.Add(medicationModel);
                    }
                }
            }

            return medications;
        }

        public MedicationModel? GetById(int? id)
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
                        MedicationModel medicationModel = new MedicationModel
                        {
                            Id = reader[ID].DBValue<int>(),
                            Name = reader[NAME].DBValue<string>(),
                        };
                        return medicationModel;
                    }
                }
            }
            return null;
        }

        public void Remove(MedicationModel medicationModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM {TABLENAME} WHERE {ID}=@id";
                command.Parameters.AddWithValue("@id", medicationModel.Id);
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
