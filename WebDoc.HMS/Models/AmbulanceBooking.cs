using System.ComponentModel.DataAnnotations;

namespace WebDoc.HMS.Models
{
    public class AmbulanceBooking
    {
        public int AmbulanceBookingId { get; set; }

        [Required(ErrorMessage = "Ambulance Booker Name is required."), StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string BookerName { get; set; }
        [Required(ErrorMessage = "Destination is required."), StringLength(100, ErrorMessage = "Specialty cannot exceed 100 characters.")]
        public string Destination { get; set; }
        [Required(ErrorMessage = "Distance is required."), Range(0, 100, ErrorMessage = "Distance cannot exceed 100 Kilometers.")]
        public int Distance { get; set; } // Distance in kilometers
        [Required(ErrorMessage = "Additional Charges is required."), Range(0, 10000, ErrorMessage = "Fee must be between 0 and 10,000.")]
        public decimal AdditionalCharges { get; set; }
        public decimal Charges { get; set; }
        public decimal FuelConsumed { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Contact is required."),StringLength(20, ErrorMessage = "Contact cannot exceed 20 characters.")]
        public string Contact { get; set; }
        
 
    }
}
