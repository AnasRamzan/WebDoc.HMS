using Microsoft.Data.SqlClient;
using WebDoc.HMS.Models;
using System.Data;

namespace WebDoc.HMS.Services
{
    public class PatientService
    {
        private readonly string _connectionString;

        public PatientService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<Patient>> GetPatientsAsync()
        {
            var patients = new List<Patient>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Patients", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    patients.Add(new Patient
                    {
                        PatientId = reader.GetInt32("PatientId"),
                        Name = reader.GetString("Name"),
                        DateOfBirth = reader.GetDateTime("DateOfBirth"),
                        Gender = reader.IsDBNull("Gender") ? null : reader.GetString("Gender"),
                        Contact = reader.IsDBNull("Contact") ? null : reader.GetString("Contact"),
                        Address = reader.IsDBNull("Address") ? null : reader.GetString("Address")
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving patients from database.", ex);
            }
            return patients;
        }

        public async Task AddPatientAsync(Patient patient)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Patients (Name, DateOfBirth, Gender, Contact, Address) VALUES (@Name, @DateOfBirth, @Gender, @Contact, @Address)", connection);
                command.Parameters.AddWithValue("@Name", patient.Name);
                command.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", (object)patient.Gender ?? DBNull.Value);
                command.Parameters.AddWithValue("@Contact", (object)patient.Contact ?? DBNull.Value);
                command.Parameters.AddWithValue("@Address", (object)patient.Address ?? DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error adding patient to database.", ex);
            }
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Patients SET Name = @Name, DateOfBirth = @DateOfBirth, Gender = @Gender, Contact = @Contact, Address = @Address WHERE PatientId = @PatientId", connection);
                command.Parameters.AddWithValue("@PatientId", patient.PatientId);
                command.Parameters.AddWithValue("@Name", patient.Name);
                command.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", (object)patient.Gender ?? DBNull.Value);
                command.Parameters.AddWithValue("@Contact", (object)patient.Contact ?? DBNull.Value);
                command.Parameters.AddWithValue("@Address", (object)patient.Address ?? DBNull.Value);
                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                    throw new Exception($"Patient with ID {patient.PatientId} not found.");
            }
            catch (SqlException ex)
            {
                throw new Exception("Error updating patient in database.", ex);
            }
        }

        public async Task DeletePatientAsync(int patientId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Patients WHERE PatientId = @PatientId", connection);
                command.Parameters.AddWithValue("@PatientId", patientId);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error deleting patient from database.", ex);
            }
        }

        public async Task<Patient> GetPatientByAppointmentIDAsync(int appointmentID)
        {
            var patient = new Patient();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Patients p INNER JOIN Appointments a ON p.PatientId = a.PatientId WHERE a.AppointmentId = @appointmentID", connection);
                command.Parameters.AddWithValue("@appointmentID", appointmentID);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {

                    patient.PatientId = reader.GetInt32("PatientId");
                    patient.Name = reader.GetString("Name");
                    patient.DateOfBirth = reader.GetDateTime("DateOfBirth");
                    patient.Gender = reader.IsDBNull("Gender") ? null : reader.GetString("Gender");
                    patient.Contact = reader.IsDBNull("Contact") ? null : reader.GetString("Contact");
                    patient.Address = reader.IsDBNull("Address") ? null : reader.GetString("Address");
                    
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving patients from database.", ex);
            }
            return patient;
        }

    }
}
