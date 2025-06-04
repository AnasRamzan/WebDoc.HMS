using Microsoft.Data.SqlClient;
using WebDoc.HMS.Models;
using System.Data;//used for reading from database using functions like GetString("Name"), instead of GetString(0)


namespace WebDoc.HMS.Services
{
    public class DoctorService
    {
        private readonly string _connectionString;

        public DoctorService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }
        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            var doctors = new List<Doctor>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Doctors", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorId = reader.GetInt32("DoctorId"),
                        Name = reader.GetString("Name"),
                        Specialty = reader.GetString("Specialty"),
                        Fee = reader.GetDecimal("Fee"),
                        Timing = reader.GetString("Timing"),
                        Contact = reader.IsDBNull("Contact") ? null : reader.GetString("Contact"),
                        Email = reader.IsDBNull("Email") ? null : reader.GetString("Email")
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving doctors from database.", ex);
            }
            return doctors;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Doctors (Name, Specialty, Fee, Timing, Contact, Email) VALUES (@Name, @Specialty, @Fee, @Timing, @Contact, @Email)", connection);
                command.Parameters.AddWithValue("@Name", doctor.Name);
                command.Parameters.AddWithValue("@Specialty", doctor.Specialty);
                command.Parameters.AddWithValue("@Fee", doctor.Fee);
                command.Parameters.AddWithValue("@Timing", doctor.Timing);
                command.Parameters.AddWithValue("@Contact", (object)doctor.Contact ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", (object)doctor.Email ?? DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error adding doctor to database.", ex);
            }
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Doctors SET Name = @Name, Specialty = @Specialty, Fee = @Fee, Timing = @Timing, Contact = @Contact, Email = @Email WHERE DoctorId = @DoctorId", connection);
                command.Parameters.AddWithValue("@DoctorId", doctor.DoctorId);
                command.Parameters.AddWithValue("@Name", doctor.Name);
                command.Parameters.AddWithValue("@Specialty", doctor.Specialty);
                command.Parameters.AddWithValue("@Fee", doctor.Fee);
                command.Parameters.AddWithValue("@Timing", doctor.Timing);
                command.Parameters.AddWithValue("@Contact", (object)doctor.Contact ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", (object)doctor.Email ?? DBNull.Value);
                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                    throw new Exception($"Doctor with ID {doctor.DoctorId} not found.");
            }
            catch (SqlException ex)
            {
                throw new Exception("Error updating doctor in database.", ex);
            }
        }

        public async Task DeleteDoctorAsync(int doctorId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Doctors WHERE DoctorId = @DoctorId", connection);
                command.Parameters.AddWithValue("@DoctorId", doctorId);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error deleting doctor from database.", ex);
            }
        }


    }
}
