using System.ComponentModel.DataAnnotations;

namespace WebDoc.HMS.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Name is required."), StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Date of Birth is required."), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters.")]
        public string Gender { get; set; }
        [StringLength(20, ErrorMessage = "Contact cannot exceed 20 characters.")]
        public string Contact { get; set; }
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }
    }

}
