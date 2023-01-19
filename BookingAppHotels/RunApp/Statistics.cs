using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BookingAppHotels.Dapper;
using BookingAppHotels.Context_Migrations;

namespace BookingAppHotels.RunApp
{
    public class Statistics
    {
        internal static readonly string _connString = "Server=tcp:nykopingdemo1mattiasg.database.windows.net,1433;Initial Catalog=BookingAppdb;Persist Security Info=False;User ID=mattiasadmin;Password=baT_maN23;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static void ShowAllCities()
        {
            var allCities = AllCities();
            Console.WriteLine("  CityId:\tCityName:\tCountry:");
            Console.WriteLine("  -----------------------------------------------\n");
            foreach (AllCitiesDapper city in allCities)
            {
                Console.WriteLine("  " + city.CityId + "\t\t" + city.CityName + "\t" + city.CountryName);
            }
        }
        // Showing off Dapper skills
        public static List<AllCitiesDapper> AllCities()
        {
            var sql = "SELECT c.Id AS 'CityId', c.Name AS 'CityName', Cl.Name AS 'CountryName' FROM Cities C JOIN Countries Cl ON C.CountryId = Cl.Id";
            var cities = new List<AllCitiesDapper>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                cities = connection.Query<AllCitiesDapper>(sql).ToList();
                connection.Close();
            }
            return cities;
        }

        internal static void MostPopularHotel()
        {
            string sql = " SELECT top 1 \r\n\t\th.Name AS Name, \r\n\t\tcount(b.RoomId) as RoomId\r\nFROM \r\n\tHotels h join Rooms r on h.Id = r.HotelId\r\n\tjoin Bookings b on r.Id = b.RoomId\r\ngroup by \r\n\t\th.Name\r\norder by \r\n\t\tRoomId Desc";

            List<Dapper.MostPopularHotelsDapper> result = new();

            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                result = connection.Query<Dapper.MostPopularHotelsDapper>(sql).ToList();
                connection.Close();
            }
            Console.Clear();
            Console.WriteLine("\n\n\n");
            foreach (var item in result)
            {
                Console.WriteLine("Populäraste hotellet just nu är: " + item.Name);
            }

            Console.ReadKey();

        }

        internal static void MoneyForAllHotels()
        {
            using (var db = new HotelContext())
            {

                var resultList = (from b in db.Bookings
                                  join r in db.Rooms on b.RoomId equals r.Id
                                  join p in db.Persons on b.PersonId equals p.Id
                                  join h in db.Hotels on r.HotelId equals h.Id
                                  where b.Id > 0
                                  select new
                                  {
                                      RoomPrice = r.Price
                                  }).ToList();
                Console.Clear();
                Console.WriteLine("\n\n\n");
                int totalEarnings = 0;
                foreach (var x in resultList)
                {
                    totalEarnings += x.RoomPrice;
                }

                Console.WriteLine("Totalt intjänade pengar just nu: " + totalEarnings);

                Console.ReadKey();
            }
        }

        internal static void MoneyForOneHotel()
        {
            using (var db = new HotelContext())
            {
                Console.Clear();
                var hotelList = db.Hotels.OrderBy(x => x.Id).ToList();
                var largestHotelId = db.Hotels.OrderBy(x => x.Id).LastOrDefault().Id;
                int i = 0;
                for (i = 0; i < hotelList.Count; i++)
                {
                    Console.WriteLine(hotelList[i].Id + ". " + hotelList[i].Name);
                }
                int answer = 0;
                Console.WriteLine("-----------------");
                Console.Write("\n\nVälj hotell: ");
                answer = Helpers.TryNumber(answer, 1, largestHotelId);

                var resultList = (from b in db.Bookings
                                  join r in db.Rooms on b.RoomId equals r.Id
                                  join p in db.Persons on b.PersonId equals p.Id
                                  join h in db.Hotels on r.HotelId equals h.Id
                                  where h.Id == answer && b.Id > 0
                                  select new
                                  {
                                      HotelName = h.Name,
                                      RoomPrice = r.Price
                                  }).ToList();

                Console.Clear();
                Console.WriteLine("\n\n\n");
                int totalEarnings = 0;
                string name = "";
                foreach (var x in resultList)
                {
                    totalEarnings += x.RoomPrice;
                    name = x.HotelName;
                }
                Console.WriteLine("Totalt intjänade pengar för hotell " + name + " just nu: " + totalEarnings);

                Console.ReadKey();
            }
        }

        internal static void MoneyForOneCountry()
        {
            using (var db = new HotelContext())
            {
                Console.Clear();
                var CountryList = db.Countries.OrderBy(x => x.Id).ToList();
                var largestCountryId = db.Countries.OrderBy(x => x.Id).LastOrDefault().Id;
                int i = 0;
                for (i = 0; i < CountryList.Count; i++)
                {
                    Console.WriteLine(CountryList[i].Id + ". " + CountryList[i].Name);
                }
                int answer = 0;
                Console.WriteLine("-----------------");
                Console.Write("\n\nVälj land: ");
                answer = Helpers.TryNumber(answer, 1, largestCountryId);

                var resultList = (from b in db.Bookings
                                  join r in db.Rooms on b.RoomId equals r.Id
                                  join p in db.Persons on b.PersonId equals p.Id
                                  join h in db.Hotels on r.HotelId equals h.Id
                                  join ci in db.Cities on h.CityId equals ci.Id
                                  join c in db.Countries on ci.CountryId equals c.Id
                                  where h.Id == answer && b.Id > 0
                                  select new
                                  {
                                      CountryName = c.Name,
                                      RoomPrice = r.Price
                                  }).ToList();

                Console.Clear();
                Console.WriteLine("\n\n\n");
                int totalEarnings = 0;
                string name = "";
                foreach (var x in resultList)
                {
                    totalEarnings += x.RoomPrice;
                    name = x.CountryName;
                }
                Console.WriteLine("Totalt intjänade pengar för hotell " + name + " just nu: " + totalEarnings);

                Console.ReadKey();
            }
        }
        internal static void MoneyForOneCity()
        {
            using (var db = new HotelContext())
            {
                Console.Clear();
                var CityList = db.Cities.OrderBy(x => x.Id).ToList();
                var largestCityId = db.Cities.OrderBy(x => x.Id).LastOrDefault().Id;
                int i = 0;
                for (i = 0; i < CityList.Count; i++)
                {
                    Console.WriteLine(CityList[i].Id + ". " + CityList[i].Name);
                }
                int answer = 0;
                Console.WriteLine("-----------------");
                Console.Write("\n\nVälj stad: ");
                answer = Helpers.TryNumber(answer, 1, largestCityId);

                var resultList = (from b in db.Bookings
                                  join r in db.Rooms on b.RoomId equals r.Id
                                  join p in db.Persons on b.PersonId equals p.Id
                                  join h in db.Hotels on r.HotelId equals h.Id
                                  join ci in db.Cities on h.CityId equals ci.Id
                                  where h.Id == answer && b.Id > 0
                                  select new
                                  {
                                      CityName = ci.Name,
                                      RoomPrice = r.Price
                                  }).ToList();

                Console.Clear();
                Console.WriteLine("\n\n\n");
                int totalEarnings = 0;
                string name = "";
                foreach (var x in resultList)
                {
                    totalEarnings += x.RoomPrice;
                    name = x.CityName;
                }
                Console.WriteLine("Totalt intjänade pengar för hotell " + name + " just nu: " + totalEarnings);

                Console.ReadKey();
            }
        }

        internal static void HowManyPeopleHaveBooked()
        {
            using (var db = new HotelContext())
            {

                var resultList = (from b in db.Bookings
                                  join p in db.Persons on b.PersonId equals p.Id
                                  where b.Id > 0
                                  select new
                                  {
                                      PersonId = p.Id,

                                  }).ToList();
                Console.Clear();
                Console.WriteLine("\n\n\n");
                var totalNrOfPeopleBooked = resultList.GroupBy(x => x.PersonId).Count();
                Console.WriteLine("Totalt antal inbokade personer just nu: " + totalNrOfPeopleBooked);
            }


            Console.ReadKey();
        }
    }
}

