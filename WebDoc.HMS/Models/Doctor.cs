using System.ComponentModel.DataAnnotations;

namespace WebDoc.HMS.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "Name is required."), StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Specialty is required."), StringLength(100, ErrorMessage = "Specialty cannot exceed 100 characters.")]
        public string Specialty { get; set; }
        [Required(ErrorMessage = "Fee is required."), Range(0, 10000, ErrorMessage = "Fee must be between 0 and 10,000.")]
        public decimal Fee { get; set; }
        [Required(ErrorMessage = "Timing is required."), StringLength(50, ErrorMessage = "Timing cannot exceed 50 characters.")]
        public string Timing { get; set; }
        [StringLength(20, ErrorMessage = "Contact cannot exceed 20 characters.")]
        public string Contact { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address."), StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }
    }
}
