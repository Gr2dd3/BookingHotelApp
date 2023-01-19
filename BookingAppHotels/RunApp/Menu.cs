using BookingAppHotels.Context_Migrations;
using BookingAppHotels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.RunApp
{
    internal class Menu
    {
        internal static void BookWichHotel(Person user, Booking booking)
        {
            using (var db = new HotelContext())
            {
                int inputCountryId = 0;
                int inputCityId = 0;
                int outputCity = 0;
                int inputHotelId = 0;
                int outputHotel = 0;
                int outputRoom = 0;
                var countryList = db.Countries.OrderBy(x => x.Id).ToList();
                var cityList = db.Cities.OrderBy(x => x.Id).ToList();
                var hotelList = db.Hotels.OrderBy(x => x.Id).ToList();
                var roomList = db.Rooms.OrderBy(x => x.Id).ToList();
                var highestCountryIdNr = countryList.OrderBy(x => x.Id).LastOrDefault();
                var highestCityIdNr = cityList.OrderBy(x => x.Id).LastOrDefault();
                var highestHotelIdNr = hotelList.OrderBy(x => x.Id).LastOrDefault();
                if (highestCityIdNr == null || highestCountryIdNr == null || highestHotelIdNr == null)
                {
                    Console.WriteLine("Det finns tyvärr inte tillräckligt med poster än. Tryck för att återvända");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Helpers.ShowUser(user);

                    Helpers.ShowCountries();

                    // Choose City from Country
                    inputCountryId = Helpers.ChooseFromMenu(inputCountryId, 1, highestCountryIdNr.Id);
                    Console.Clear();
                    Helpers.ShowUser(user);
                    outputCity = Helpers.ShowCitiesWithCountryID(inputCountryId);

                    inputCityId = Helpers.ChooseFromMenu(inputCityId, 1, highestCityIdNr.Id);
                    outputCity = cityList[inputCityId - 1].Id;
                    Console.Clear();
                    Helpers.ShowUser(user);

                    // Choose Hotel from City
                    var hotelListWithCityId = db.Hotels.Where(x => x.CityId == outputCity).OrderBy(x => x.Id).ToList();
                    outputHotel = Helpers.ShowHotelsWithCityID(outputCity);
                    inputHotelId = Helpers.ChooseFromMenu(inputHotelId, 1, highestHotelIdNr.Id);
                    outputHotel = hotelListWithCityId[inputHotelId - 1].Id;
                    Console.Clear();

                    // Show Rooms in hotel
                    outputRoom = Helpers.ShowRoomsOfHotelwithHotelID(user, outputHotel, outputRoom);

                    // Select available time of room
                    Helpers.BookCalenderForRoomWithId(user, outputRoom, booking);
                }
            }
        }

        internal static void ManageStatistics(Person user, Booking booking)
        {
            Console.Clear();
            Helpers.ShowUser(user);
            string[] menu =
            {
                "1. Populäraste hotellet",
                "2. Hur mycket pengar appen har dragit in",
                "3. Hur mycket pengar som tjänats på ett hotell",
                "4. Hur mycket pengar som tjänats i ett land",
                "5. Hur mycket pengar som tjänats i en stad",
                "6. Hur många personer som är inbokade just nu",
                "7. Återgå"
            };
            foreach (var item in menu)
            {
                Console.WriteLine(item);
            }
            int choise = 0;
            choise = Helpers.TryNumber(choise, 1, menu.Length);
            switch (choise)
            {
                case 1:
                    Statistics.MostPopularHotel();
                    break;
                case 2:
                    Statistics.MoneyForAllHotels();
                    break;
                case 3:
                    Statistics.MoneyForOneHotel();
                    break;
                case 4:
                    Statistics.MoneyForOneCountry();
                    break;
                case 5:
                    Statistics.MoneyForOneCity();
                    break;
                case 6:
                    Statistics.HowManyPeopleHaveBooked();
                    break;
                case 7:
                    break;
            }

        }

        internal static void ViewUserBookings(Person user, Booking booking)
        {

            using (var db = new HotelContext())
            {
                var roomsList = db.Rooms;
                var resultList = (from b in db.Bookings
                                  join p in db.Persons on b.PersonId equals p.Id
                                  join r in db.Rooms on b.RoomId equals r.Id
                                  join h in db.Hotels on r.HotelId equals h.Id
                                  where b.PersonId == user.Id
                                  select new
                                  {
                                      BookingID = b.Id,
                                      PersonName = p.Name,
                                      HotelName = h.Name,
                                      RoomName = r.Name,
                                      CheckInDate = b.Date,
                                      RoomPrice = r.Price
                                  }).ToList();

                if (user.Name == "admin")
                {
                    resultList = (from b in db.Bookings
                                  join r in db.Rooms on b.RoomId equals r.Id
                                  join p in db.Persons on b.PersonId equals p.Id
                                  join h in db.Hotels on r.HotelId equals h.Id
                                  select new
                                  {
                                      BookingID = b.Id,
                                      PersonName = p.Name,
                                      HotelName = h.Name,
                                      RoomName = r.Name,
                                      CheckInDate = b.Date,
                                      RoomPrice = r.Price
                                  }).ToList();
                }

                Console.Clear();
                Helpers.ShowUser(user);
                int padValue1 = 15;
                int padValue2 = 20;

                Console.WriteLine("\nBokningsID".PadRight(padValue1) + "  Namn".PadRight(padValue2) + "  Hotell".PadRight(padValue2) +
                    " Rum".PadRight(padValue1) + "Incheckning".PadRight(padValue2) + " Pris".PadRight(padValue1));
                Console.WriteLine("--------------------------------------------------------------------------------------------------");
                foreach (var b in resultList)
                {
                    Console.WriteLine(b.BookingID.ToString().PadRight(padValue1) + b.PersonName.PadRight(padValue2) + b.HotelName.PadRight(padValue2) +
                        b.RoomName.PadRight(padValue1) + b.CheckInDate.ToString("d").PadRight(padValue2) + b.RoomPrice + " SEK".PadRight(padValue1));
                }

                int answer = 0;
                Console.WriteLine("\n\n\n1. Välj bokning att ta bort\n2. Återgå");
                answer = Helpers.TryNumber(answer, 1, 2);

                switch (answer)
                {
                    case 1:
                        int removeProductId = 0;
                        Console.Write("\n\nAnge vilken bokning du vill ta bort: ");
                        removeProductId = Helpers.TryNumber(removeProductId, 1, resultList[resultList.Count - 1].BookingID);
                        Helpers.RemoveBooking(user, booking, removeProductId);
                        Console.WriteLine("Bokning med Id " + removeProductId + " har blivit borttagen!");
                        Thread.Sleep(1500);
                        break;
                    case 2:
                        break;
                }
            }
        }
    }
}
