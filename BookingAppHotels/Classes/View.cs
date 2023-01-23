using BookingAppHotels.Context_Migrations;
using BookingAppHotels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.RunApp
{
    internal class View
    {
        internal static void ShowUserBookings(Person user, Booking booking)
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
                View.ShowUser(user);
                int padValue1 = 15;
                int padValue2 = 20;
                int padValue3 = 89;
                int padValue4 = 98;
                int count = 1;
                Console.WriteLine("\nBokningsID".PadRight(padValue1) + "  Namn".PadRight(padValue2) + "  Hotell".PadRight(padValue2) +
                    " Rum".PadRight(padValue1) + "Incheckning".PadRight(padValue2) + " Pris".PadRight(padValue1));
                Console.WriteLine("--------------------------------------------------------------------------------------------------");
                foreach (var b in resultList)
                {
                    Console.WriteLine(count.ToString().PadRight(padValue1) + b.PersonName.PadRight(padValue2) + b.HotelName.PadRight(padValue2) +
                        b.RoomName.PadRight(padValue1) + b.CheckInDate.ToString("d").PadRight(padValue2) + b.RoomPrice + " SEK".PadRight(padValue1));
                    count++;
                }
                int answer = 0;
                var totalPrice = resultList.Sum(x => x.RoomPrice);

                Console.WriteLine("\n");
                Console.WriteLine("Totalt pris: ".PadLeft(padValue3) + totalPrice + " SEK");
                Console.WriteLine("---------------------".PadLeft(padValue4));
                Console.WriteLine("\n\n\n1. Välj bokning att ta bort\n2. Återgå");
                answer = Helpers.TryNumber(answer, 1, 2);
                switch (answer)
                {
                    case 1:
                        int removeProductId = 0;
                        Console.Write("\n\nAnge vilken bokning du vill ta bort: ");
                        removeProductId = Helpers.TryNumber(removeProductId, 1, count - 1);
                        removeProductId = resultList[removeProductId - 1].BookingID;
                        RemoveMethods.RemoveBooking(user, booking, removeProductId);
                        Console.WriteLine("Bokning nr " + (count - 1) + " har blivit borttagen!");
                        Thread.Sleep(1500);
                        break;
                    case 2:
                        break;
                }
            }
        }
        internal static void ShowRoomsInSelectedHotel(Person user, int inputHotelId)
        {
            using (var db = new HotelContext())
            {

                int outputRoom = 0;
                outputRoom = View.ShowRoomsOfHotelwithHotelID(user, inputHotelId, outputRoom);
                int input = 0;
                Console.WriteLine("\n\n1. Ange nytt pris för rum\n2. Återgå");
                input = Helpers.TryNumber(input, 1, 2);
                switch (input)
                {
                    case 1:
                        Helpers.ChangePriceForRoom(outputRoom);
                        break;
                    case 2:
                        break;
                }

                Console.ReadKey();
            }
        }
        internal static void ShowCities()
        {
            using (var db = new HotelContext())
            {
                var cityList = db.Cities.OrderBy(x => x.Id).ToList();

                for (int i = 0; i < cityList.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + cityList[i].Name);
                }

            }
        }
        internal static int ShowCitiesWithCountryID(int id)
        {
            using (var db = new HotelContext())
            {
                var cityList = db.Cities.Where(x => x.CountryId == id).OrderBy(x => x.Id).ToList();
                int i = 0;
                for (i = 0; i < cityList.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + cityList[i].Name);
                }
                return i;
            }
        }
        internal static void ShowCountries()
        {
            using (var db = new HotelContext())
            {
                var countryList = db.Countries.OrderBy(x => x.Id).ToList();

                for (int i = 0; i < countryList.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + countryList[i].Name);
                }

            }
        }
        internal static void ShowHotels(Person user)
        {
            using (var db = new HotelContext())
            {
                var hotelList = db.Hotels.OrderBy(x => x.Id).ToList();
                var largestHotelId = db.Hotels.OrderBy(x => x.Id).LastOrDefault().Id;
                int i = 0;
                for (i = 0; i < hotelList.Count; i++)
                {
                    Console.WriteLine(hotelList[i].Id + ". " + hotelList[i].Name);
                }


                int answer = 0;
                Console.WriteLine("\n\n\n1. Se vilka rum som finns i hotell\n2. Återgå");
                answer = Helpers.TryNumber(answer, 1, 2);
                int hotelId = 0;

                switch (answer)
                {
                    case 1:
                        hotelId = Helpers.ChooseFromMenu(hotelId, 1, largestHotelId);
                        View.ShowRoomsInSelectedHotel(user, hotelId);
                        break;
                    case 2:
                        break;
                }
            }
        }
        internal static int ShowHotelsWithCityID(int cityId)
        {

            using (var db = new HotelContext())
            {
                var hotelList = db.Hotels.Where(x => x.CityId == cityId).OrderBy(x => x.Id).ToList();
                int i = 0;
                for (i = 0; i < hotelList.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + hotelList[i].Name);
                }
                return i;
            }
        }
        internal static void ShowUser(Person user)
        {
            Console.Clear();
            Console.SetCursorPosition(50, 0);
            Console.Write("Användare: " + user.Name + "\n\n");
        }
        internal static int ShowRoomsOfHotelwithHotelID(Person user, int outputHotelId, int outputRoom)
        {
            using (var db = new HotelContext())
            {
                int inputRoom = 0;
                int padValue = 16;
                int padValue1 = 20;
                int padValue2 = 65;
                int count = 1;
                var roomList = db.Rooms.ToList();
                var highestRoomIdNr = roomList.OrderBy(x => x.Id).LastOrDefault();
                var hotel = (from h in db.Hotels
                             where h.Id == outputHotelId
                             join r in db.Rooms on h.Id equals r.HotelId
                             select new
                             {
                                 HotelId = h.Id,
                                 HotelName = h.Name,
                                 NrOfFloors = h.NrOfFloors,
                                 NrOfRoomsPerFloor = h.NrOfRoomsPerFloor,
                                 RoomId = r.Id,
                                 RoomName = r.Name,
                                 Price = r.Price,
                                 Floor = r.Floor,
                                 Size = r.Size,
                                 View = r.View,
                                 RoomService = r.RoomService,
                                 Breakfast = r.Breakfast,
                                 ParkingSpace = r.ParkingSpace,
                                 FullBoard = r.FullBoard,
                                 Rooms = h.Rooms
                             }).FirstOrDefault();
                if (hotel != null || highestRoomIdNr != null)
                {

                    var roomsInHotel = hotel.Rooms.GroupBy(x => x.Floor).ToList();
                    var hotelRoomsList = hotel.Rooms.ToList();


                    Console.Clear();
                    View.ShowUser(user);

                    // Print Rooms in hotel Per Floor
                    Console.WriteLine(hotel.HotelName.PadLeft(padValue2));
                    Console.WriteLine("     ==============".PadLeft(padValue2) + "\n\n\n");
                    foreach (var floor in roomsInHotel)
                    {
                        Console.WriteLine("Våning ".PadLeft(padValue2) + floor.Key + ": ");
                        Console.WriteLine("\t\t--------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n\nNr:".PadRight(padValue1) + "Rum:".PadRight(padValue1) + "Pris: ".PadRight(padValue1) + "Storlek (kvm):".PadRight(padValue) +
                            " Frukost:".PadRight(padValue) + "Utsikt:".PadRight(padValue) + "RoomService:".PadRight(padValue) + "Parkering ingår:".PadRight(padValue1) + "Helpension:".PadRight(padValue1));
                        Console.WriteLine("────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
                        foreach (var room in floor)
                        {
                            Console.WriteLine(count.ToString().PadRight(padValue) + " \"" + room.Name + "\"".PadRight(padValue) + room.Price + " SEK".PadRight(padValue) +
                                room.Size.ToString().PadRight(padValue1) + (room.Breakfast == true ? "Ja" : "Nej").PadRight(padValue) + (room.View == true ? "Ja" : "Nej").PadRight(padValue) +
                                (room.RoomService == true ? "Ja" : "Nej").PadRight(padValue) + (room.ParkingSpace == true ? "Ja" : "Nej").PadRight(padValue1) + (room.FullBoard == true ? "Ja" : "Nej").PadRight(padValue1));
                            count++;
                        }
                        Console.WriteLine("\n\n");
                    }

                    inputRoom = Helpers.ChooseFromMenu(inputRoom, 1, highestRoomIdNr.Id);
                    outputRoom = hotelRoomsList[inputRoom - 1].Id;
                }
                else
                {
                    Console.WriteLine("FEL");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                return outputRoom;
            }

        }
    }
}
