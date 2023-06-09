using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Windows;
using VsProject.Models;

namespace VsProject.Repositories
{
    public class AppointmentRepository : RepositoryBase, IAppointmentRepository
    {
        private const string TABLENAME = "appointment";
        private const string ID = "id";
        private const string PATIENTID = "patient_id";
        private const string SUBJECT = "subject";
        private const string DATE = "date";
        private const string CREATEDTIME = "created_time";
        private const string STARTTIME = "start_time";
        private const string ENDTIME = "end_time";
        public int Add(AppointmentModel appointmentModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            //using (var command = new SqlCommand())
            {
                if (appointmentModel == null)
                {
                    throw new ArgumentNullException("patient");
                }

                var patientId = UserPrincipal.PatientRepository.GetById(appointmentModel.PatientId)?.Id;
                if (patientId != null)
                {

                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO {TABLENAME} ({PATIENTID}, {SUBJECT}, {DATE}, {STARTTIME}, {ENDTIME}) " +
                                            "VALUES (@patientId, @subject, @date, @startTime, @endTime)";

                    command.Parameters.AddWithValue("@patientId", patientId);
                    command.Parameters.AddWithValue("@subject", appointmentModel.Subject);
                    command.Parameters.AddWithValue("@date", appointmentModel.Date.DBToDateTime());
                    command.Parameters.AddWithValue("@startTime", appointmentModel.StartTime.DBToTimeSpan());
                    command.Parameters.AddWithValue("@endTime", appointmentModel.EndTime.DBToTimeSpan());
                    command.ExecuteNonQuery();
                    //get the generated ID
                    command.CommandText = "SELECT @@IDENTITY";
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                else
                {
                    throw new ArgumentNullException("patient does not exist");
                }
                
                
            }
        }

        public void Edit(AppointmentModel appointmentModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"UPDATE {TABLENAME} SET {SUBJECT}=@subject, {DATE}=@date, {STARTTIME}=@startTime, {ENDTIME}=@endTime WHERE {ID}=@id AND {PATIENTID}=@patientId";

                command.Parameters.AddWithValue("@id", appointmentModel.Id);
                command.Parameters.AddWithValue("@patientId", appointmentModel.PatientId);
                command.Parameters.AddWithValue("@subject", appointmentModel.Subject);
                command.Parameters.AddWithValue("@date", appointmentModel.Date.DBToDateTime());
                command.Parameters.AddWithValue("@startTime", appointmentModel.StartTime.DBToTimeSpan());
                command.Parameters.AddWithValue("@endTime", appointmentModel.EndTime.DBToTimeSpan());

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<AppointmentModel> GetAll()
        {
            var appointments = new List<AppointmentModel>();

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
                        AppointmentModel appointment = new AppointmentModel
                        {
                            Id = reader[ID].DBValue<int>(),
                            PatientId = reader[PATIENTID].DBValue<int>(),
                            Subject = reader[SUBJECT].DBValue<string>(),
                            Date = reader[DATE].DBValue<DateOnly>(),
                            StartTime = reader[STARTTIME].DBValue<TimeOnly>(),
                            EndTime = reader[ENDTIME].DBValue<TimeOnly>(),
                        };
                        appointments.Add(appointment);
                    }
                }
            }
            return appointments;
        }

        public AppointmentModel? GetByIdAndPatientId(int? id, int? patientId)
        {
            if (!IdExists(id, patientId))
            {
                return null;
            }
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT {SUBJECT}, {DATE}, {STARTTIME}, {ENDTIME} FROM {TABLENAME} WHERE {ID}=@id AND {PATIENTID}=@patientId";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@patientId", patientId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        AppointmentModel appointment = new AppointmentModel
                        {
                            Id = id,
                            PatientId = patientId,
                            Subject = reader[SUBJECT].DBValue<string>(),
                            Date = reader[DATE].DBValue<DateOnly>(),
                            StartTime = reader[STARTTIME].DBValue<TimeOnly>(),
                            EndTime = reader[ENDTIME].DBValue<TimeOnly>(),
                        };

                        return appointment;
                    }
                }
            }
            return null;
        }

        public void Remove(AppointmentModel appointmentModel)
        {
            using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM {TABLENAME} WHERE {ID}=@id AND {PATIENTID}=@patientId";
                command.Parameters.AddWithValue("@id", appointmentModel.Id);
                command.Parameters.AddWithValue("@patientId", appointmentModel.PatientId);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<AppointmentModel>? GetAllByPatientId(int? patientId, DateOnly? minDate = null, DateOnly? maxDate = null)
        {
            if (!IdExists(patientId))
            {
                return null;
            }
            var appointments = new List<AppointmentModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                var comString = $"SELECT * FROM {TABLENAME} WHERE {PATIENTID} = @patientId";
                if(minDate != null) 
                {
                    comString += $" AND {DATE} >= @minDate";
                }
                if(maxDate != null) 
                {
                    comString += $" AND {DATE} <= @maxDate";
                }

                command.CommandText = comString;
                command.Parameters.AddWithValue("@patientId", patientId);
                if (minDate != null) 
                {
                    command.Parameters.AddWithValue("@minDate", minDate.Value.DBToDateTime()); 
                }
                if (maxDate != null)
                {
                    command.Parameters.AddWithValue("@maxDate", maxDate.Value.DBToDateTime());
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AppointmentModel appointment = new AppointmentModel
                        {
                            Id = reader[ID].DBValue<int>(),
                            PatientId = reader[PATIENTID].DBValue<int>(),
                            Subject = reader[SUBJECT].DBValue<string>(),
                            Date = reader[DATE].DBValue<DateOnly>(),
                            StartTime = reader[STARTTIME].DBValue<TimeOnly>(),
                            EndTime = reader[ENDTIME].DBValue<TimeOnly>(),
                        };
                        appointments.Add(appointment);
                    }
                }
            }
            return appointments;
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

        private bool IdExists(int? id,int? patientId)
        {
            if (id == null)
            {
                return false;
            }
            if (patientId == null)
            {
                return false;
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(*) FROM {TABLENAME} WHERE {ID}=@id AND {PATIENTID}=@patientId";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@patientId", patientId);

                return (int)command.ExecuteScalar() > 0;
                //return (long)command.ExecuteScalar() > 0;
            }
        }
    }
}

