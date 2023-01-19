using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public int NrOfFloors { get; set; }
        public int NrOfRoomsPerFloor { get; set; }
        public virtual City City { get; set; }
        public ICollection<Room> Rooms { get; set; }

    }
}
