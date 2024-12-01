using System.ComponentModel.DataAnnotations;

namespace DAMS.Models
{
    public class Purchase
    {
        [Key]
        public int Purchase_ID { get; set; }
        [Required]
        public int Selling_ID { get; set; }
        [Required]
        public int Patient_ID { get; set; }
        [Required]
        public int Requested_Quantity { get; set; }
        [Required]
        public string Request_Status { get; set; }
        [Required]
        public string Delivery_Date { get; set; }
        [Required]
        public string Delivery_Time { get; set; }

    }
}
