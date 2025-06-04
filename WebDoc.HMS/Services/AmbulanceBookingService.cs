using Microsoft.Data.SqlClient;
using WebDoc.HMS.Models;
using System.Data;

namespace WebDoc.HMS.Services
{
    public class AmbulanceBookingService
    {
        private readonly string _connectionString;

        public AmbulanceBookingService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<AmbulanceBooking>> GetAmbulanceBookingsAsync()
        {
            var bookings = new List<AmbulanceBooking>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM AmbulanceBookings", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    bookings.Add(new AmbulanceBooking
                    {
                        AmbulanceBookingId = reader.GetInt32("AmbulanceBookingId"),
                        BookerName = reader.GetString("BookerName"),
                        Destination = reader.GetString("Destination"),
                        Distance = reader.GetInt32("Distance"),
                        AdditionalCharges = reader.GetDecimal("AdditionalCharges"),
                        Charges = reader.GetDecimal("Charges"),
                        FuelConsumed = reader.GetDecimal("FuelConsumed"),
                        Status = reader.GetString("Status"),
                        Contact = reader.GetString("Contact")
                    });
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error retrieving ambulance bookings from database.", ex);
            }
            return bookings;
        }

        public async Task AddAmbulanceBookingAsync(AmbulanceBooking booking)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand(@"
                    INSERT INTO AmbulanceBookings 
                    (BookerName, Destination, Distance, AdditionalCharges, Charges, FuelConsumed, Status, Contact) 
                    VALUES 
                    (@BookerName, @Destination, @Distance, @AdditionalCharges, @Charges, @FuelConsumed, @Status, @Contact)", connection);

                command.Parameters.AddWithValue("@BookerName", booking.BookerName);
                command.Parameters.AddWithValue("@Destination", booking.Destination);
                command.Parameters.AddWithValue("@Distance", booking.Distance);
                command.Parameters.AddWithValue("@AdditionalCharges", booking.AdditionalCharges);
                command.Parameters.AddWithValue("@Charges", booking.Charges);
                command.Parameters.AddWithValue("@FuelConsumed", booking.FuelConsumed);
                command.Parameters.AddWithValue("@Status", booking.Status);
                command.Parameters.AddWithValue("@Contact", booking.Contact);

                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error adding ambulance booking to database.", ex);
            }
        }

        public async Task UpdateAmbulanceBookingAsync(AmbulanceBooking booking)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand(@"
                    UPDATE AmbulanceBookings 
                    SET BookerName = @BookerName, Destination = @Destination, Distance = @Distance,
                        AdditionalCharges = @AdditionalCharges, Charges = @Charges, FuelConsumed = @FuelConsumed,
                        Status = @Status, Contact = @Contact
                    WHERE AmbulanceBookingId = @AmbulanceBookingId", connection);

                command.Parameters.AddWithValue("@AmbulanceBookingId", booking.AmbulanceBookingId);
                command.Parameters.AddWithValue("@BookerName", booking.BookerName);
                command.Parameters.AddWithValue("@Destination", booking.Destination);
                command.Parameters.AddWithValue("@Distance", booking.Distance);
                command.Parameters.AddWithValue("@AdditionalCharges", booking.AdditionalCharges);
                command.Parameters.AddWithValue("@Charges", booking.Charges);
                command.Parameters.AddWithValue("@FuelConsumed", booking.FuelConsumed);
                command.Parameters.AddWithValue("@Status", booking.Status);
                command.Parameters.AddWithValue("@Contact", booking.Contact);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                    throw new Exception($"Ambulance booking with ID {booking.AmbulanceBookingId} not found.");
            }
            catch (SqlException ex)
            {
                throw new Exception("Error updating ambulance booking in database.", ex);
            }
        }

        public async Task DeleteAmbulanceBookingAsync(int bookingId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM AmbulanceBookings WHERE AmbulanceBookingId = @AmbulanceBookingId", connection);
                command.Parameters.AddWithValue("@AmbulanceBookingId", bookingId);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error deleting ambulance booking from database.", ex);
            }
        }
    }
}
