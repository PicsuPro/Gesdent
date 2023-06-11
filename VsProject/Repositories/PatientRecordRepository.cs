using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using VsProject.Models;

namespace VsProject.Repositories
{
    public class PatientRecordRepository : RepositoryBase, IPatientRecordRepository
    {
        private const string TABLENAME = "patient_record";
        private const string PATIENTID = "patient_id";
        private const string PROBLEMS = "problems";
        private const string PROBLEMSSEPARATOR = "{}-";
        private const string DIAGNOSTIC = "diagnostic";
        private const string TREATMENTPLAN = "treatment_plan";
        private const string NOTES = "notes";
        private const string TIMESTART = "time_start";
        private const string TIMEEND = "time_end";
        public void Add(PatientRecordModel patientRecordModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            //using (var command = new SqlCommand())
            {

                if (!IdExists(patientRecordModel.PatientId))
                {
                    var patientId = UserPrincipal.PatientRepository.GetById(patientRecordModel.PatientId)?.Id;
                    if (patientId != null)
                    {

                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = $"INSERT INTO {TABLENAME} ({PATIENTID},{PROBLEMS},{DIAGNOSTIC},{TREATMENTPLAN},{NOTES})" +
                                              "VALUES (@patientId,@problems,@diagnostic,@treatmentPlan,@notes)";

                        command.Parameters.AddWithValue("@patientId", patientId);
                        command.Parameters.AddWithValue("@problems", string.Join(PROBLEMSSEPARATOR,patientRecordModel.Problems?? new ObservableCollection<string>()).DBNullOrWS());
                        command.Parameters.AddWithValue("@diagnostic", patientRecordModel.Diagnostic.DBNullOrWS());
                        command.Parameters.AddWithValue("@treatmentPlan", patientRecordModel.TreatmentPlan.DBNullOrWS());
                        command.Parameters.AddWithValue("@notes", patientRecordModel.Notes.DBNullOrWS());
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
            if (IdExists(patientRecordModel.PatientId))
            {
                    using (var connection = GetConnection())
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = $"UPDATE {TABLENAME} SET  {PROBLEMS}=@problems, {DIAGNOSTIC}=@diagnostic, {TREATMENTPLAN}=@treatmentPlan, {NOTES}=@notes WHERE {PATIENTID}=@patientId";
                        command.Parameters.AddWithValue("@patientId", patientRecordModel.PatientId);
                        command.Parameters.AddWithValue("@problems", string.Join(PROBLEMSSEPARATOR, patientRecordModel.Problems ?? new ObservableCollection<string>()).DBNullOrWS());
                        command.Parameters.AddWithValue("@diagnostic", patientRecordModel.Diagnostic.DBNullOrWS());
                        command.Parameters.AddWithValue("@treatmentPlan", patientRecordModel.TreatmentPlan.DBNullOrWS());
                        command.Parameters.AddWithValue("@notes", patientRecordModel.Notes.DBNullOrWS());
                        command.ExecuteNonQuery();
                    }
            }
            else
            {
                throw new ArgumentNullException("patient record does not exist");
            }
        }
        public IEnumerable<PatientRecordModel> GetAllFromHistory(int? patientId)
        {
            var patientRecordHistory = new List<PatientRecordModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {PROBLEMS},{DIAGNOSTIC}, {TREATMENTPLAN}, {NOTES} FROM {TABLENAME} FOR SYSTEM_TIME ALL WHERE {PATIENTID} = @patientId ORDER BY  {TIMESTART} DESC";
                command.Parameters.AddWithValue("@patientId", patientId);

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                    {
                        PatientRecordModel patientRecord = new PatientRecordModel
                        {
                            PatientId = patientId,
                            Problems = DBExtensions.ConvertToObservableCollection(PROBLEMSSEPARATOR, reader[PROBLEMS].DBValue<string>()),
                            Diagnostic = reader[DIAGNOSTIC].DBValue<string>(),
                            TreatmentPlan = reader[TREATMENTPLAN].DBValue<string>(),
                            Notes = reader[NOTES].DBValue<string>(),
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
                command.CommandText = $"SELECT {PROBLEMS},{DIAGNOSTIC}, {TREATMENTPLAN}, {NOTES} FROM {TABLENAME} WHERE {PATIENTID}=@patientId";
                command.Parameters.AddWithValue("@patientId", patientId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        PatientRecordModel patientRecord = new PatientRecordModel
                        {
                            PatientId = patientId,
                            Problems = DBExtensions.ConvertToObservableCollection(PROBLEMSSEPARATOR, reader[PROBLEMS].DBValue<string>()),
                            Diagnostic = reader[DIAGNOSTIC].DBValue<string>(),
                            TreatmentPlan = reader[TREATMENTPLAN].DBValue<string>(),
                            Notes = reader[NOTES].DBValue<string>(),
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
