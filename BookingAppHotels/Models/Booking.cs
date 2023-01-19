using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PersonId { get; set; }
        public int RoomId { get; set; }
        public virtual Person? Person { get; set; }
        public virtual Room? Room { get; set; }

    }
}
