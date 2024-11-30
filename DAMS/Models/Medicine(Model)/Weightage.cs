using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace DAMS.Models.Medicine_Model_
{
    public class Weightage
    {
        [Key]
        public int Weightage_ID { get; set; }
        [Required]
        public int Medicine_Weightage {  get; set; }
    }
}
