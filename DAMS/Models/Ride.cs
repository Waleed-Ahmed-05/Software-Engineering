using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DAMS.Models
{
    public class Ride
    {
        [Key]
        public int Ride_ID { get; set; }
        [Required]
        public int Driver_ID { get; set; }
        [Required]
        public int Patient_ID { get; set; }
        [Required]
        public string Pickup_Location { get; set; }
        [Required]
        public string Dropoff_Location { get; set; }
        [Required]
        public string Pickup_Time { get; set; }
        [Required]
        public string Pickup_Date { get; set; }
        [Required]
        public string Ride_Status { get; set; }
    }
}
