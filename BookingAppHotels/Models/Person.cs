using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool? WorkTrip { get; set; }
        public int? NumberOfGuests { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
