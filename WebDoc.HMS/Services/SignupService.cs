using Microsoft.Data.SqlClient; // Or Microsoft.Data.SqlClient
using WebDoc.HMS.Models; // Assuming your SignupModel is in this namespace
using WebDoc.HMS.Interfaces;
using System.Text;


namespace WebDoc.HMS.Services
{
    public class SignupService : IUserService
    {
        private readonly string _connectionString;

        // Constructor to inject IConfiguration to get the connection string
        public SignupService(IConfiguration configuration)
        {
            // Assumes your connection string is named "DefaultConnection" in appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> RegisterUserAsync(User model)
        {
            if (model == null)
            {
                return false;
            }

            if (model.Password != model.ConfirmPassword)
            {
                return false; // This should ideally be caught by model validation
            }
            
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Check if email already exists
                    string checkUserSql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserSql, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@Email", model.Email);
                        int userExists = (int)await checkUserCommand.ExecuteScalarAsync();
                        if (userExists > 0)
                        {
                            return false;
                        }
                    }

                    // Insert new user
                    string insertSql = "INSERT INTO Users (Name, Role, Email, Password) VALUES (@Name, @Role, @Email, @Password)";
                    using (SqlCommand command = new SqlCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Role", model.Role);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Password", model.Password);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return (rowsAffected > 0);
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (ex)
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return (false);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                Console.WriteLine($"General Exception: {ex.Message}");
                return (false);
            }
        }       
    }
}

