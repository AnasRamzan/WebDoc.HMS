using Microsoft.Data.SqlClient;
using WebDoc.HMS.Models;
using System.Data;

namespace WebDoc.HMS.Services
{
    public class AppointmentService
    {
        private readonly string _connectionString;

        public AppointmentService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            var appointments = new List<Appointment>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Appointments", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    appointments.Add(new Appointment
                    {
                        AppointmentId = reader.GetInt32("AppointmentId"),
                        DoctorId = reader.GetInt32("DoctorId"),
                        PatientId = reader.GetInt32("PatientId"),
                        AppointmentDate = reader.GetDateTime("AppointmentDate"),
                        Reason = reader.IsDBNull("Reason") ? null : reader.GetString("Reason"),
                        Status = reader.GetString("Status")
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving appointments from database.", ex);
            }
            return appointments;
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Appointments (DoctorId, PatientId, AppointmentDate, Reason, Status) VALUES (@DoctorId, @PatientId, @AppointmentDate, @Reason, @Status)", connection);
                command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@Reason", (object)appointment.Reason ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", appointment.Status);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error adding appointment to database.", ex);
            }
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Appointments SET DoctorId = @DoctorId, PatientId = @PatientId, AppointmentDate = @AppointmentDate, Reason = @Reason, Status = @Status WHERE AppointmentId = @AppointmentId", connection);
                command.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
                command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@Reason", (object)appointment.Reason ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", appointment.Status);
                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                    throw new Exception($"Appointment with ID {appointment.AppointmentId} not found.");
            }
            catch (SqlException ex)
            {
                throw new Exception("Error updating appointment in database.", ex);
            }
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Appointments WHERE AppointmentId = @AppointmentId", connection);
                command.Parameters.AddWithValue("@AppointmentId", appointmentId);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error deleting appointment from database.", ex);
            }
        }

        public async Task<List<Appointment>> GetAppointmentsOfAPatientAsync(int appointmentID)
        {
            var appointments = new List<Appointment>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Appointments WHERE PatientId = (SELECT PatientId FROM Appointments WHERE AppointmentId = @AppointmentId)", connection);
                command.Parameters.AddWithValue("@AppointmentID", appointmentID);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    appointments.Add(new Appointment
                    {
                        AppointmentId = reader.GetInt32("AppointmentId"),
                        DoctorId = reader.GetInt32("DoctorId"),
                        PatientId = reader.GetInt32("PatientId"),
                        AppointmentDate = reader.GetDateTime("AppointmentDate"),
                        Reason = reader.IsDBNull("Reason") ? null : reader.GetString("Reason"),
                        Status = reader.GetString("Status")
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving appointments from database.", ex);
            }
            return appointments;
        }


    }
}
