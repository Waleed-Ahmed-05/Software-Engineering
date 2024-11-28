using System.ComponentModel.DataAnnotations;

namespace DAMS.Models.Medicine_Model_
{
    public class Price
    {
        [Key]
        public int Price_ID { get; set; }
        [Required]
        public string Medicine_Price {  get; set; }
    }
}
