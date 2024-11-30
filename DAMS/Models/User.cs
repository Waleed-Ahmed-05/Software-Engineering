using System.ComponentModel.DataAnnotations;

namespace DAMS.Models
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Age { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string CNIC { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
