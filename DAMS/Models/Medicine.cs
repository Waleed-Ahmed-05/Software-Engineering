using System.ComponentModel.DataAnnotations;

namespace DAMS.Models
{
    public class Medicine
    {
        [Key]
        public int Medicine_ID { get; set; }
        [Required]
        public string Medicine_Name { get; set; }
        [Required]
        public string Medicine_Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Medicine_Weightage { get; set; }
    }
}
