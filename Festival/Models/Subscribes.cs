using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Festival.Models
{
    public class Subscribes
    {
        [Required,Key]
        public int Subscribe_ID { get; set; }
        [ForeignKey("User_ID")]
        public int User_ID { get; set; }
        [ForeignKey("Event_ID")]
        public int Event_ID { get; set; }

        //public Events Events { get; set; }
        //public Users Users { get; set; }

    }
}
