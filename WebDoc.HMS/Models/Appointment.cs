using System.ComponentModel.DataAnnotations;

namespace WebDoc.HMS.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        [Required(ErrorMessage = "Doctor is required."), Range(1, int.MaxValue, ErrorMessage = "Please select a valid doctor.")]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "Patient is required."), Range(1, int.MaxValue, ErrorMessage = "Please select a valid patient.")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Appointment Date is required."), DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;
        [StringLength(200, ErrorMessage = "Reason cannot exceed 200 characters.")]
        public string Reason { get; set; }
        [Required(ErrorMessage = "Status is required."), StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; }
    }
}
