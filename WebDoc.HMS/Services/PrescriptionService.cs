using Microsoft.Data.SqlClient;
using WebDoc.HMS.Models;
using System.Data;

namespace WebDoc.HMS.Services
{
    public class PrescriptionService
    {
        private readonly string _connectionString;

        public PrescriptionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<Prescription>> GetPrescriptionsAsync()
        {
            var prescriptions = new List<Prescription>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Prescriptions", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    prescriptions.Add(new Prescription
                    {
                        PrescriptionId = reader.GetInt32("PrescriptionId"),
                        AppointmentId = reader.GetInt32("AppointmentId"),
                        Medication = reader.GetString("Medication"),
                        Dosage = reader.IsDBNull("Dosage") ? null : reader.GetString("Dosage"),
                        Instructions = reader.IsDBNull("Instructions") ? null : reader.GetString("Instructions"),
                        IssueDate = reader.GetDateTime("IssueDate")
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving prescriptions from database.", ex);
            }
            return prescriptions;
        }

        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Prescriptions (AppointmentId, Medication, Dosage, Instructions, IssueDate) VALUES (@AppointmentId, @Medication, @Dosage, @Instructions, @IssueDate)", connection);
                command.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                command.Parameters.AddWithValue("@Medication", prescription.Medication);
                command.Parameters.AddWithValue("@Dosage", (object)prescription.Dosage ?? DBNull.Value);
                command.Parameters.AddWithValue("@Instructions", (object)prescription.Instructions ?? DBNull.Value);
                command.Parameters.AddWithValue("@IssueDate", prescription.IssueDate);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error adding prescription to database.", ex);
            }
        }

        public async Task UpdatePrescriptionAsync(Prescription prescription)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Prescriptions SET AppointmentId = @AppointmentId, Medication = @Medication, Dosage = @Dosage, Instructions = @Instructions, IssueDate = @IssueDate WHERE PrescriptionId = @PrescriptionId", connection);
                command.Parameters.AddWithValue("@PrescriptionId", prescription.PrescriptionId);
                command.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                command.Parameters.AddWithValue("@Medication", prescription.Medication);
                command.Parameters.AddWithValue("@Dosage", (object)prescription.Dosage ?? DBNull.Value);
                command.Parameters.AddWithValue("@Instructions", (object)prescription.Instructions ?? DBNull.Value);
                command.Parameters.AddWithValue("@IssueDate", prescription.IssueDate);
                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                    throw new Exception($"Prescription with ID {prescription.PrescriptionId} not found.");
            }
            catch (SqlException ex)
            {
                throw new Exception("Error updating prescription in database.", ex);
            }
        }

        public async Task DeletePrescriptionAsync(int prescriptionId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Prescriptions WHERE PrescriptionId = @PrescriptionId", connection);
                command.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error deleting prescription from database.", ex);
            }
        }

        public async Task<List<Prescription>> GetPrescriptionsOfAPatientAsync(int AppointmentID)
        {
            var prescriptions = new List<Prescription>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT p.PrescriptionId, p.AppointmentId, p.Medication, p.Dosage, p.Instructions, p.IssueDate FROM Prescriptions p INNER JOIN Appointments a ON p.AppointmentId = a.AppointmentId WHERE a.PatientId = ( SELECT PatientId FROM Appointments WHERE AppointmentId = @appointmentID)", connection);
                command.Parameters.AddWithValue("@appointmentID", AppointmentID);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    prescriptions.Add(new Prescription
                    {
                        PrescriptionId = reader.GetInt32("PrescriptionId"),
                        AppointmentId = reader.GetInt32("AppointmentId"),
                        Medication = reader.GetString("Medication"),
                        Dosage = reader.IsDBNull("Dosage") ? null : reader.GetString("Dosage"),
                        Instructions = reader.IsDBNull("Instructions") ? null : reader.GetString("Instructions"),
                        IssueDate = reader.GetDateTime("IssueDate")
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving prescriptions from database.", ex);
            }
            return prescriptions;
        }

    }
}
