using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingAppHotels.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAppHotels.Context_Migrations 
{
    public class HotelContext : DbContext
    {
        // DbSets

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:nykopingdemo1mattiasg.database.windows.net,1433;Initial Catalog=BookingAppdb;Persist Security Info=False;User ID=mattiasadmin;Password=baT_maN23;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
