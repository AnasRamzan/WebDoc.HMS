using Microsoft.Data.SqlClient; // Or Microsoft.Data.SqlClient
using WebDoc.HMS.Models; // Assuming your SignupModel is in this namespace
using WebDoc.HMS.Interfaces;
using System.Text;

namespace WebDoc.HMS.Services
{
    public class LoginService : ILoginService
    {
        private readonly string _connectionString;

        // Constructor to inject IConfiguration to get the connection string
        public LoginService(IConfiguration configuration)
        {
            // Assumes your connection string is named "DefaultConnection" in appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");
        }

        public async Task<User> LoginUserAsync(LoginModel loginmodel)
        {
            if (loginmodel == null)
            {
                return null;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    User user = new User();

                    // Insert new user
                    string selectSql = "SELECT * FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", loginmodel.Email);
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            user.Name = reader.GetString(1);
                            user.Role = reader.GetString(2);
                            user.Email = reader.GetString(3);
                            user.Password = reader.GetString(4);    

                        }
                        Console.WriteLine($"Name : {user.Name}, Role: {user.Role}, Email: {user.Email}, Password: {user.Password}");
                        return user;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (ex)
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return (null);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                Console.WriteLine($"General Exception: {ex.Message}");
                return (null);
            }
        }

    }
}
