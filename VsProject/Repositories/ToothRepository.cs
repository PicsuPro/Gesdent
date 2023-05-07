using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models;

namespace VsProject.Repositories
{
    public class ToothRepository : RepositoryBase, IToothRepository
    {
        private const string TABLENAME = "tooth";
        private const string PATIENTRECORDID = "patient_record_id";
        private const string NUMBER = "number";
        private const string APICALREACTION = "apical_reaction";
        private const string DECAY = "decay";
        private const string TIMESTART = "time_start";
        private const string TIMEEND = "time_end";




        public void AddAll(IEnumerable<ToothModel> teethList,int? patientRecordId)
        {
            if(IdExists(patientRecordId))
            {
                throw new ArgumentNullException("Teeth already exists");
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Begin transaction
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    // Update each tooth in the input list
                    foreach (var tooth in teethList)
                    {
                        command.CommandText = $"INSERT INTO {TABLENAME} ( {PATIENTRECORDID}, {NUMBER}, {APICALREACTION},{DECAY}) " +
                                            "VALUES ( @patientRecordId, @number, @apicalReaction, @decay)"; 

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@patientRecordId", patientRecordId);
                        command.Parameters.AddWithValue("@number", tooth.Number);
                        command.Parameters.AddWithValue("@apicalReaction", tooth.ApicalReaction);
                        command.Parameters.AddWithValue("@decay", tooth.Decay);

                        command.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Rollback transaction in case of an exception
                    transaction.Rollback();
                    throw;
                }
            }
        }

        

        public void EditAll(IEnumerable<ToothModel> teethList,int? patientRecordId)
        {
            

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Begin transaction
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    // Update each tooth in the input list
                    foreach (var tooth in teethList)
                    {
                        command.CommandText = $"UPDATE {TABLENAME} SET {APICALREACTION} = @apicalReaction, {DECAY} = @decay WHERE {PATIENTRECORDID} = @patientRecordId AND {NUMBER} = @number ";

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@apicalReaction", tooth.ApicalReaction);
                        command.Parameters.AddWithValue("@decay", tooth.Decay);
                        command.Parameters.AddWithValue("@patientRecordId", patientRecordId);
                        command.Parameters.AddWithValue("@number", tooth.Number);

                        command.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Rollback transaction in case of an exception
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public ToothModel? GetByNumber(int? number, int patientRecordId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {APICALREACTION}, {DECAY} FROM {TABLENAME} WHERE {PATIENTRECORDID} = @patientRecordId AND {NUMBER} = @number";

                command.Parameters.AddWithValue("@patientRecordId", patientRecordId);
                command.Parameters.AddWithValue("@number", number);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tooth = new ToothModel
                        {
                            Number = (int)number,
                            ApicalReaction = reader[APICALREACTION].DBValue<bool>(),
                            Decay = reader[DECAY].DBValue<bool>(),
                        };
                        return tooth;
                    }
                    return null;
                }
            }
        }

        public IEnumerable<ToothModel> GetAll(int patientRecordId)
        {
            var teeth = new List<ToothModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {PATIENTRECORDID}, {NUMBER}, {APICALREACTION}, {DECAY} FROM {TABLENAME} WHERE {PATIENTRECORDID} = @patientRecordId";

                command.Parameters.AddWithValue("@patientRecordId", patientRecordId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tooth = new ToothModel
                        {
                            Number = reader[NUMBER].DBValue<int>(),
                            ApicalReaction = reader[APICALREACTION].DBValue<bool>(),
                            Decay = reader[DECAY].DBValue<bool>(),
                        };

                        teeth.Add(tooth);
                    }
                    return teeth;
                }
            }
        }

        public IEnumerable<IEnumerable<ToothModel>> GetAllFromHistory( int patientRecordId)
        {
            var teethHistory = new List<IEnumerable<ToothModel>>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {PATIENTRECORDID}, {NUMBER}, {APICALREACTION}, {DECAY} FROM {TABLENAME} FOR SYSTEM_TIME ALL WHERE {PATIENTRECORDID} = @patientRecordId ORDER BY  {TIMESTART} DESC , {NUMBER}";

                command.Parameters.AddWithValue("@patientRecordId", patientRecordId);

                using (var reader = command.ExecuteReader())
                {
                    var currentTeeth = new List<ToothModel>();

                    while (reader.Read())
                    {
                        var tooth = new ToothModel
                        {
                            Number = reader[NUMBER].DBValue<int>(),
                            ApicalReaction = reader[APICALREACTION].DBValue<bool>(),
                            Decay = reader[DECAY].DBValue<bool>(),
                        };

                        if (currentTeeth.Count == 32)
                        {
                            teethHistory.Add(currentTeeth);
                            currentTeeth = new List<ToothModel>();
                        }
                        currentTeeth.Add(tooth);
                    }

                    if (currentTeeth.Count == 32)
                    {
                        teethHistory.Add(currentTeeth);
                    }
                }
                return teethHistory;

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
                command.CommandText = $"SELECT COUNT(*) FROM {TABLENAME} WHERE {PATIENTRECORDID}=@patientRecordId";
                command.Parameters.AddWithValue("@patientRecordId", id);

                return (int)command.ExecuteScalar() > 0;
                //return (long)command.ExecuteScalar() > 0;
            }
        }


    }
}

