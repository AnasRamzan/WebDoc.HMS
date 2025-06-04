using System.ComponentModel.DataAnnotations;

namespace WebDoc.HMS.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        [Required(ErrorMessage = "Appointment is required."), Range(1, int.MaxValue, ErrorMessage = "Please select a valid appointment.")]
        public int AppointmentId { get; set; }
        [Required(ErrorMessage = "Medication is required."), StringLength(200, ErrorMessage = "Medication cannot exceed 200 characters.")]
        public string Medication { get; set; }
        [StringLength(100, ErrorMessage = "Dosage cannot exceed 100 characters.")]
        public string Dosage { get; set; }
        [StringLength(500, ErrorMessage = "Instructions cannot exceed 500 characters.")]
        public string Instructions { get; set; }
        [Required(ErrorMessage = "Issue Date is required."), DataType(DataType.Date)]
        public DateTime IssueDate { get; set; } = DateTime.Now;
    }
}
