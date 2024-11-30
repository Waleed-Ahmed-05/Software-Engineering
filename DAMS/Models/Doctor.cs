using System.ComponentModel.DataAnnotations;

namespace DAMS.Models
{
    public class Doctor
    {
        [Key]
        public int Doctor_ID { get; set; }
        [Required]
        public int User_ID { get; set; }
        [Required]
        public string Clinic_Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Certification { get; set; }
        [Required]
        public string Study_Field { get; set; }
        [Required]
        public string Availability { get; set; }
        [Required]
        public string Offline_Duration_Start { get; set; }
        [Required]
        public string Offline_Duration_End { get; set; }
        [Required]
        public string Online_Duration_Start { get; set; }
        [Required]
        public string Online_Duration_End { get; set; }
    }
}
