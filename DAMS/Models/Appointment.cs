using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace DAMS.Models
{
    public class Appointment
    {
        [Key]
        public int Appointment_ID { get; set; }
        [Required]
        public int Patient_ID { get; set; }
        [Required]
        public int Doctor_ID { get; set; }
        [Required]
        public string Appointment_Date { get; set; }
        [Required]
        public string Appointment_Time { get; set; }
        [Required]
        public string Appointment_Status { get;set; }
    }
}
