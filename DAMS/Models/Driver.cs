using System.ComponentModel.DataAnnotations;

namespace DAMS.Models
{
    public class Driver
    {
        [Key]
        public int Driver_ID { get; set; }
        [Required]
        public int User_ID { get; set; }
        [Required]
        public string Vehicle_Category { get; set; }
        [Required]
        public string Vehicle_Name { get; set; }
        [Required]
        public string Number_Plate { get; set; }
        [Required]
        public string Rating { get; set; }
    }
}
