using System.ComponentModel.DataAnnotations;

namespace DAMS.Models
{
    public class Sell
    {
        [Key]
        public int Selling_ID { get; set; }
        [Required]
        public int Medicine_ID { get; set; }
        [Required]
        public int Seller_ID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float Medicine_Price { get; set; }
    }
}
