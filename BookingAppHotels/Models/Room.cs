using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Floor { get; set; }
        public int? Size { get; set; }
        public bool? View { get; set; }
        public bool? RoomService { get; set; }
        public bool? Breakfast { get; set; }
        public bool? ParkingSpace { get; set; }
        public bool? FullBoard { get; set; }
        public int HotelId { get; set; }
        public virtual Hotel? Hotel { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
