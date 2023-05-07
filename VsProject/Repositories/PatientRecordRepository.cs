using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VsProject.Repositories
{
    public class PatientRecordRepository : RepositoryBase, IPatientRecordRepository
    {
        private const string TABLENAME = "patient_record";
        private const string PATIENTID = "patient_id";
        private const string TIMESTART = "time_start";
        private const string TIMEEND = "time_end";
        public void Add(PatientRecordModel patientRecordModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            //using (var command = new SqlCommand())
            {

                if (GetByPatientId(patientRecordModel.PatientId) == null)
                {
                    var patientId = UserPrincipal.PatientRepository.GetById(patientRecordModel.PatientId)?.Id;
                    if (patientId != null)
                    {

                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = $"INSERT INTO {TABLENAME} ({PATIENTID})" +
                                              "VALUES (@patientId)";

                        command.Parameters.AddWithValue("@patientId", patientId);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new ArgumentNullException("patient does not exist");
                    }
                }
                else
                {
                    throw new ArgumentNullException("patient record already exists");
                }
            }
        }

        public void Edit(PatientRecordModel patientRecordModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PatientRecordModel> GetAllFromHistory(int? patientId)
        {
            var patientRecordHistory = new List<PatientRecordModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {PATIENTID} FROM {TABLENAME} FOR SYSTEM_TIME ALL WHERE {PATIENTID} = @patientId ORDER BY  {TIMESTART} DESC";
                command.Parameters.AddWithValue("@patientId", patientId);

                using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    PatientRecordModel patientRecord = new PatientRecordModel
                    {
                        PatientId = reader[PATIENTID].DBValue<int>(),
                    };
                    patientRecordHistory.Add(patientRecord);
                }
                return patientRecordHistory;

            }
        }

        public PatientRecordModel? GetByPatientId(int? patientId)
        {
            if (!IdExists(patientId))
            {
                return null;
            }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {PATIENTID} FROM {TABLENAME} WHERE {PATIENTID}=@patientId";
                command.Parameters.AddWithValue("@patientId", patientId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        PatientRecordModel patientRecord = new PatientRecordModel
                        {
                            PatientId = reader[PATIENTID].DBValue<int>(),
                        };
                        return patientRecord;
                    }
                }
            }
            return null;
        }
        private bool IdExists(int? patientId)
        {
            if (patientId == null)
            {
                return false;
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(*) FROM {TABLENAME} WHERE {PATIENTID}=@patientId";
                command.Parameters.AddWithValue("@patientId", patientId);

                return (int)command.ExecuteScalar() > 0;
                //return (long)command.ExecuteScalar() > 0;
            }
        }
    }
}
