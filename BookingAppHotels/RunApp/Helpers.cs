using BookingAppHotels.Context_Migrations;
using BookingAppHotels.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Dapper;

namespace BookingAppHotels.RunApp
{
    public class Helpers
    {
        public static void AddCountries(Person user)
        {
            ShowUser(user);
            using (var db = new HotelContext())
            {
                var countryList = db.Countries;
                int count = 0;
                foreach (var country in countryList)
                {
                    count++;
                    Console.WriteLine(count + ". " + country.Name);
                }
                Console.WriteLine("-------------------");

                int answer = 0;
                Console.WriteLine("\n\n1. Lägg till ett nytt land\n2. Återgå");
                answer = TryNumber(answer, 1, 2);
                switch (answer)
                {
                    case 1:
                        Console.Write("\n\n\nSkriv namnet på landet: ");
                        string countryName = Console.ReadLine();
                        var newCountry = new Country
                        {
                            Name = countryName
                        };
                        db.Countries.Add(newCountry);
                        db.SaveChanges();
                        break;
                    case 2:
                        break;
                }

            }
        }
        public static void ManageHotels(Person user)
        {
            ShowUser(user);
            using (var db = new HotelContext())
            {
                var hotelsList = db.Hotels;
                int answer = 0;

                string[] menu = ("1. Se vilka hotell som finns|2. Lägg till hotell|3. Ta bort hotell|4. Återgå").Split('|');
                foreach (var choice in menu)
                {
                    Console.WriteLine(choice);
                }
                Console.Write("\nAnge vad du vill göra: ");
                answer = TryNumber(answer, 1, menu.Length);
                switch (answer)
                {
                    case 1:
                        Console.Clear();
                        ShowHotels(user);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nTryck [enter] för att återgå");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    case 2:
                        AddHotels(user);
                        break;
                    case 3:
                        RemoveHotels(user);
                        break;
                    case 4:
                        break;

                }
            }
        }
        public static void ManageCities(Person user)
        {
            ShowUser(user);
            using (var db = new HotelContext())
            {
                var cityList = db.Cities.ToList();
                int answer = 0;

                string[] menu = ("1. Se vilka städer som finns|2. Lägg till stad|3. Ta bort stad|4. Återgå").Split('|');
                foreach (var choice in menu)
                {
                    Console.WriteLine(choice);
                }
                Console.Write("\nAnge vad du vill göra: ");
                answer = TryNumber(answer, 1, menu.Length);

                switch (answer)
                {
                    case 1:
                        Console.Clear();
                        ShowCities();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nTryck [enter] för att återgå");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    case 2:
                        AddCities(user);
                        break;
                    case 3:
                        RemoveCities(user);
                        break;
                    case 4:
                        break;

                }
            }
        }
        public static void RemoveCountries(Person user)
        {
            ShowUser(user);
            using (var db = new HotelContext())
            {
                var countryList = db.Countries;
                bool correctInput = false;
                int answer = 0;
                int index = 1;
                while (!correctInput)
                {
                    foreach (var country in countryList)
                    {
                        Console.WriteLine(index + ". " + country.Name);
                        index++;
                    }
                    Console.WriteLine("\n----------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tryck [å] för att återgå");
                    Console.ResetColor();
                    Console.Write("Ange ID för landet du vill ta bort: ");
                    if (Console.ReadLine() == "å")
                    {
                        break;
                    }
                    else
                    {
                        answer = TryNumber(answer, 1, countryList.ToList().Count);
                    }

                    var deleteCountry = (from c in countryList
                                         where c.Id == answer
                                         select c).SingleOrDefault();
                    if (deleteCountry != null)
                    {
                        countryList.Remove((Country)deleteCountry);
                        db.SaveChanges();
                        correctInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Id finns inte. Testa igen eller tryck [å] för att återgå");
                    }
                }
            }
        }
        public static void RemoveCities(Person user)
        {
            ShowUser(user);
            using (var db = new HotelContext())
            {
                var cityList = db.Cities;
                bool correctInput = false;
                int answer = 0;
                int index = 1;
                while (!correctInput)
                {
                    foreach (var city in cityList)
                    {
                        Console.WriteLine(index + ". " + city.Name);
                        index++;
                    }
                    Console.WriteLine("\n----------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tryck [å] för att återgå");
                    Console.ResetColor();
                    Console.Write("Ange ID för staden du vill ta bort: ");
                    if (Console.ReadLine() == "å")
                    {
                        break;
                    }
                    else
                    {
                        answer = TryNumber(answer, 1, cityList.ToList().Count);
                    }

                    var deleteCity = (from c in cityList
                                      where c.Id == answer
                                      select c).SingleOrDefault();
                    if (deleteCity != null)
                    {
                        cityList.Remove((City)deleteCity);
                        db.SaveChanges();
                        correctInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Id finns inte. Testa igen eller tryck [å] för att återgå");
                    }
                }
            }
        }
        public static void RemoveHotels(Person user)
        {
            ShowUser(user);
            using (var db = new HotelContext())
            {
                var hotelList = db.Hotels;
                bool correctInput = false;
                int answer = 0;
                int index = 1;
                while (!correctInput)
                {
                    foreach (var hotel in hotelList)
                    {
                        Console.WriteLine(index + ". " + hotel.Name);
                        index++;
                    }
                    Console.WriteLine("\n----------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tryck [å] för att återgå");
                    Console.ResetColor();
                    Console.Write("Ange ID för staden du vill ta bort: ");
                    if (Console.ReadLine() == "å")
                    {
                        break;
                    }
                    else
                    {
                        answer = TryNumber(answer, 1, hotelList.ToList().Count);
                    }

                    var deleteHotel = (from h in hotelList
                                       where h.Id == answer
                                       select h).SingleOrDefault();
                    if (deleteHotel != null)
                    {
                        hotelList.Remove((Hotel)deleteHotel);
                        db.SaveChanges();
                        correctInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Id finns inte. Testa igen eller tryck [å] för att återgå");
                    }
                }
            }
        }
        public static void AddCities(Person user)
        {
            ShowUser(user);
            bool correctInput = false;
            using (var db = new HotelContext())
            {
                var cityList = db.Cities;
                var countryList = db.Countries.ToList();

                while (!correctInput)
                {

                    int countryInput = 0;
                    ShowCountries();
                    Console.Write("\nVälj vilket land staden ska befinna sig i: ");
                    countryInput = TryNumber(countryInput, 1, countryList.Count);
                    var existingCountryId = countryList.SingleOrDefault(x => x.Id == countryInput) != null;
                    Console.Clear();
                    ShowCities();
                    Console.WriteLine("\n----------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nTryck [å] för att återgå");
                    Console.ResetColor();
                    Console.Write("\nSkriv namn på nya staden: ");
                    string answer = Console.ReadLine();
                    if (answer == "å")
                    {
                        break;
                    }
                    var newCityName = cityList.SingleOrDefault(x => x.Name == answer) == null;
                    Console.Clear();
                    if (answer == "å")
                    {
                        correctInput = true;
                    }
                    else if (newCityName && existingCountryId)
                    {
                        var newCity = new City
                        {
                            Name = answer,
                            CountryId = countryInput
                        };

                        cityList.Add(newCity);
                        db.SaveChanges();
                        Console.WriteLine("Stad tillagd!");
                        Thread.Sleep(800);
                        correctInput = true;
                    }
                    else if (newCityName == false)
                    {
                        Console.WriteLine("Staden finns redan. Försök igen.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else if (existingCountryId == false)
                    {
                        Console.WriteLine("Du har skrivit in fel landId. Försök igen.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Något blev fel. Försök igen.");
                        Console.ReadLine();
                        Console.Clear();
                    }


                }
            }
        }
        public static void AddHotels(Person user)
        {
            ShowUser(user);
            bool correctInput = false;
            using (var db = new HotelContext())
            {
                var hotelsList = db.Hotels;
                var roomList = db.Rooms;
                var countryList = db.Countries.ToList();
                var cityList = db.Cities.ToList();

                while (!correctInput)
                {
                    ShowCities();
                    int countryInput = 0;
                    int cityInput = 0;
                    int floors = 0;
                    int roomsPerFloor = 0;
                    Console.WriteLine("\n----------------------\n\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tryck [å] för att återgå");
                    Console.ResetColor();
                    Console.Clear();
                    Console.Write("\nSkriv namn på nya hotellet: ");
                    string? name = Console.ReadLine();
                    Console.Write("Ange hur många våningar det är i hotellet: ");
                    floors = TryNumber(floors, 1, 10);
                    Console.Write("Ange hur många rum per våning: ");
                    roomsPerFloor = TryNumber(roomsPerFloor, 1, 20);
                    if (name == "å")
                    {
                        break;
                    }
                    var newHotelName = hotelsList.SingleOrDefault(x => x.Name == name) == null;
                    Console.Clear();
                    // Select Country
                    ShowCountries();
                    Console.WriteLine("\n----------------------");
                    Console.Write("Välj vilket land hotellet befinner sig i: ");
                    countryInput = TryNumber(countryInput, 1, countryList.Count);
                    var existingCountryId = countryList.SingleOrDefault(x => x.Id == countryInput) != null;
                    Console.Clear();
                    // Select City
                    var citiesInChosenCountry = cityList.Where(x => x.CountryId == countryInput).ToList();
                    int count = 1;
                    foreach (var city in citiesInChosenCountry)
                    {
                        Console.WriteLine(count + ". " + city.Name);
                        count++;
                    }
                    Console.WriteLine("\n----------------------");
                    Console.WriteLine("\nTryck [0] för att avbryta");
                    Console.Write("\nVälj vilken stad hotellet befinner sig i: ");
                    cityInput = TryNumber(countryInput, 0, citiesInChosenCountry.Count);
                    var existingCityId = cityList.SingleOrDefault(x => x.Id == cityInput) != null;
                    if (newHotelName && existingCountryId && existingCityId || cityInput != 0)
                    {
                        var newHotel = new Hotel
                        {
                            Name = name,
                            CityId = cityInput,
                            NrOfFloors = floors,
                            NrOfRoomsPerFloor = roomsPerFloor
                        };
                        hotelsList.Add(newHotel);
                        db.SaveChanges();
                        // Adds Rooms to Hotel in background with different qualities
                        var nrOfRoomsInNewHotel = (newHotel.NrOfRoomsPerFloor * newHotel.NrOfFloors);
                        int roomNamecount = 1;
                        int makeBoolTrueCount = 0;
                        int price = 1299;
                        int size = 8;
                        int floor = 1;
                        bool roomService = false;
                        bool breakFast = true;
                        bool parkingSpace = true;
                        bool fullBoard = false;
                        bool view = false;
                        string roomName = "";
                        for (int i = 0; i < nrOfRoomsInNewHotel; i++)
                        {
                            if (roomNamecount == (newHotel.NrOfRoomsPerFloor + 1) && floor < newHotel.NrOfFloors)
                            {
                                floor += 1;
                                roomNamecount = 1;
                                makeBoolTrueCount++;
                                price += 500;
                                size += 3;
                                if (makeBoolTrueCount == 1)
                                {
                                    roomService = true;
                                }
                                else if (makeBoolTrueCount == 2)
                                {
                                    view = true;
                                    fullBoard = true;
                                }
                            }
                            roomName = "Rum " + roomNamecount;
                            var newRoom = new Room
                            {
                                Name = roomName,
                                Price = price,
                                Floor = floor,
                                Size = size,
                                View = view,
                                RoomService = roomService,
                                Breakfast = breakFast,
                                ParkingSpace = parkingSpace,
                                FullBoard = fullBoard,
                                HotelId = newHotel.Id
                            };
                            roomNamecount++;
                            //if (roomNamecount == (newHotel.NrOfRoomsPerFloor + 1))
                            //{
                            //    floor += 1;
                            //}
                            roomList.Add(newRoom);
                        }

                        db.SaveChanges();
                        Console.WriteLine("Hotell tillagt!");
                        Thread.Sleep(800);
                        correctInput = true;
                    }
                    else if (cityInput == 0)
                    {
                        break;
                    }
                    else if (newHotelName == false)
                    {
                        Console.WriteLine("Hotellet finns redan. Försök igen.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else if (existingCountryId == false)
                    {
                        Console.WriteLine("Du har skrivit in fel landId. Försök igen.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else if (existingCityId == false)
                    {
                        Console.WriteLine("Du har skrivit in fel stadId. Försök igen.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Något blev fel. Försök igen.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
        }
        public static void ShowCities()
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
        public static int ShowCitiesWithCountryID(int id)
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
        public static void ShowCountries()
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
        public static void ShowHotels(Person user)
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
                answer = TryNumber(answer, 1, 2);
                int hotelId = 0;

                switch (answer)
                {
                    case 1:
                        Console.Write("\n\nVälj ett hotell: ");
                        hotelId = TryNumber(hotelId, 1, largestHotelId);
                        ShowRoomsInSelectedHotel(user, hotelId);
                        break;
                    case 2:
                        break;
                }
            }
        }
        public static int ShowHotelsWithCityID(int cityId)
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
        public static void ShowUser(Person user)
        {
            Console.Clear();
            Console.SetCursorPosition(50, 0);
            Console.Write("Användare: " + user.Name + "\n\n");
        }
        public static int TryNumber(int number, int minValue, int maxValue)
        {
            bool correctInput = false;

            while (!correctInput)
            {
                if (!int.TryParse(Console.ReadLine(), out number) || number > maxValue || number < minValue)
                {
                    Console.Write("Fel inmatning. Försök igen: ");
                }
                else
                {
                    correctInput = true;
                }
            }
            return number;
        }

        public static int ChooseFromMenu(int input, int minValue, int maxValue)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nVälj från menyn: ");
            Console.ResetColor();
            input = Helpers.TryNumber(input, minValue, maxValue);
            return input;
        }
        public static void ShowRoomsInSelectedHotel(Person user, int inputHotelId)
        {
            using (var db = new HotelContext())
            {

                int outputRoom = 0;
                outputRoom = ShowRoomsOfHotelwithHotelID(user, inputHotelId, outputRoom);
                int input = 0;
                Console.WriteLine("\n\n1. Ange nytt pris för rum\n2. Återgå");
                input = TryNumber(input, 1, 2);
                switch (input)
                {
                    case 1:
                        ChangePriceForRoom(outputRoom);
                        break;
                    case 2:
                        break;
                }

                Console.ReadKey();
            }
        }

        private static void ChangePriceForRoom(int outputRoom)
        {
            using (var db = new HotelContext())
            {

                int price = 0;
                Console.Write("\n\nAnge det nya priset: ");
                price = TryNumber(price, 1, 9999);
                var oneRoom = db.Rooms.Where(x => x.Id == outputRoom).FirstOrDefault();
                oneRoom.Price = price;
                db.SaveChanges();
                Console.WriteLine("Priset för rum " + oneRoom.Name + " har blivit " + oneRoom.Price + "!");
            }
        }

        public static int ShowRoomsOfHotelwithHotelID(Person user, int outputHotelId, int outputRoom)
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
                    ShowUser(user);

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

        internal static void BookCalenderForRoomWithId(Person user, int roomID, Booking booking)
        {
            using (var db = new HotelContext())
            {
                Console.Clear();
                bool keepSelecting = true;
                bool makeABreak = false;
                var startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                var endDate = new DateTime(2030, 12, 31);
                DateTime date = startDate;
                Person? personInRoomBooking = null;
                List<Booking>? roomBooking = new();

                // DateTime calender
                while (keepSelecting)
                {
                    for (date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        date = startDate;
                        var bookingsList = db.Bookings;
                        var personList = db.Persons;

                        Room? oneRoom = db.Rooms.FirstOrDefault(x => x.Id == roomID);
                        Console.WriteLine("\t    " + oneRoom.Name);
                        Console.WriteLine("\t----------\n\n");
                        Console.WriteLine("\tn - Nästa vecka\n\tp - Förra veckan\n\tb - Boka rum\n\ta - Avsluta\n\n");
                        Console.WriteLine();

                        for (int j = 0; j < 7; j++)
                        {
                            Console.Write((j + 1) + ". " + date.ToString("d") + " " + date.ToString("dddd") + ":\t");

                            roomBooking = bookingsList.Where(x => x.RoomId == oneRoom.Id && x.Date == date).ToList();
                            if (roomBooking != null && roomBooking.Count > 0)
                            {
                                var personIdOfRoom = roomBooking[0].PersonId;
                                personInRoomBooking = personList.Where(x => x.Id == personIdOfRoom).FirstOrDefault();
                            }

                            if (roomBooking.Count > 0)
                            {
                                if (roomBooking[0].Date != date)
                                {
                                    Console.WriteLine(" - ");
                                }
                                else
                                {
                                    Console.WriteLine(personInRoomBooking.Name == null ? " - " : personInRoomBooking.Name);
                                }
                            }
                            else
                            {
                                Console.WriteLine(" - ");
                            }
                            date = date.AddDays(1);
                        }
                        date = date.AddDays(-1);


                        ConsoleKeyInfo key = Console.ReadKey(true);
                        switch (key.KeyChar)
                        {
                            case 'n':
                                startDate = startDate.AddDays(7);
                                break;
                            case 'p':
                                startDate = startDate.AddDays(-7);
                                break;
                            case 'b':
                                BookDate(date, user, oneRoom, roomBooking);
                                break;
                            case 'a':
                                keepSelecting = false;
                                makeABreak = true;
                                break;
                        }
                        Console.Clear();
                        if (makeABreak == true)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private static void BookDate(DateTime date, Person user, Room oneRoom, List<Booking> roomBooking)
        {
            int bookingDate = 0;

            Console.WriteLine("\n\nVilken dag vill du komma?\n");
            bookingDate = TryNumber(bookingDate, 1, 7);
            DateTime alreadyBooked = new();


            switch (bookingDate)
            {
                case 1:
                    date = date.AddDays(-6);
                    Console.WriteLine(date.ToString("d") + " " + date.ToString("dddd"));
                    break;
                case 2:
                    date = date.AddDays(-5);
                    Console.WriteLine(date.ToString("d") + " " + date.ToString("dddd"));
                    break;
                case 3:
                    date = date.AddDays(-4);
                    Console.WriteLine(date.ToString("d") + " " + date.ToString("dddd"));
                    break;
                case 4:
                    date = date.AddDays(-3);
                    Console.WriteLine(date.ToString("d") + " " + date.ToString("dddd"));
                    break;
                case 5:
                    date = date.AddDays(-2);
                    Console.WriteLine(date.ToString("d") + " " + date.ToString("dddd"));
                    break;
                case 6:
                    date = date.AddDays(-1);
                    Console.WriteLine(date.ToString("d") + " " + date.ToString("dddd"));
                    break;
                case 7:
                    Console.WriteLine(date.ToString("d") + " " + date.ToString("dddd"));
                    break;
            }
            using (var db = new HotelContext())
            {
                bool occupiedDate = false;
                if (oneRoom.Bookings != null)
                {
                    foreach (var booking in oneRoom.Bookings)
                    {
                        if (booking.Date == date)
                        {
                            Console.WriteLine("Det finns redan en bokning denna dag.");
                            occupiedDate = true;
                        }
                    }
                    if (!occupiedDate)
                    {
                        Booking newBooking = new Booking()
                        {
                            Date = date,
                            PersonId = user.Id,
                            RoomId = oneRoom.Id
                        };
                        db.Bookings.Add(newBooking);
                        db.SaveChanges();
                        Console.WriteLine("Ny bokning gjord!");
                    }
                }
                else
                {
                    Booking newBooking = new Booking()
                    {
                        Date = date,
                        PersonId = user.Id,
                        RoomId = oneRoom.Id
                    };
                    db.Bookings.Add(newBooking);
                    db.SaveChanges();
                    Console.WriteLine("Ny bokning gjord!");
                }
            }
            Console.ReadKey();
        }

        internal static void Welcome()
        {
            string welcome = @"                                                                                                                        
,--.   ,--.,---.  ,--.   ,--. ,--. ,-----. ,--.   ,--.,--.   ,--.,------.,--.  ,--.                                     
 \  `.'  //  O  \ |  |   |  .'   /'  .-.  '|   `.'   ||   `.'   ||  .---'|  ,'.|  |                                     
  \     /|  .-.  ||  |   |  .   ' |  | |  ||  |'.'|  ||  |'.'|  ||  `--, |  |' '  |                                     
   \   / |  | |  ||  '--.|  |\   \'  '-'  '|  |   |  ||  |   |  ||  `---.|  | `   |                                     
    `-'  `--' `--'`-----'`--' '--' `-----' `--'   `--'`--'   `--'`------'`--'  `--'                                     
                        ,--------.,--.,--.   ,--.                                                                       
                        '--.  .--'|  ||  |   |  |                                                                       
                           |  |   |  ||  |   |  |                                                                       
                           |  |   |  ||  '--.|  '--.                                                                    
                           `--'   `--'`-----'`-----'                                                              ,---. 
,-----.   ,-----. ,--. ,--.,--.  ,--.,--.,--.  ,--. ,----.    ,---.    ,---.  ,------. ,------. ,------.,--.  ,--.|   | 
|  |) /_ '  .-.  '|  .'   /|  ,'.|  ||  ||  ,'.|  |'  .-./   '   .-'  /  O  \ |  .--. '|  .--. '|  .---'|  ,'.|  ||  .' 
|  .-.  \|  | |  ||  .   ' |  |' '  ||  ||  |' '  ||  | .---.`.  `-. |  .-.  ||  '--' ||  '--' ||  `--, |  |' '  ||  |  
|  '--' /'  '-'  '|  |\   \|  | `   ||  ||  | `   |'  '--'  |.-'    ||  | |  ||  | --' |  | --' |  `---.|  | `   |`--'  
`------'  `-----' `--' '--'`--'  `--'`--'`--'  `--' `------' `-----' `--' `--'`--'     `--'     `------'`--'  `--'.--.  
                                                                                                                  '--'  ";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(welcome);
            Thread.Sleep(3000);
            Console.ResetColor();
            Console.Clear();
        }
        internal static Person CustomerStartPage(Person user, int input, Booking booking)
        {

            string[] menu = ("1. Se och ändra i mina bokningar|2. Boka hotell|3. Logga ut").Split('|');
            foreach (string item in menu)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nVälj från menyn: ");
            input = Helpers.TryNumber(input, 1, menu.Length);
            if (user.Name != null && user.Name != "admin")
            {
                switch (input)
                {
                    case 1:
                        Menu.ViewUserBookings(user, booking);
                        break;
                    case 2:
                        Menu.BookWichHotel(user, booking);
                        break;
                    case 3:
                        user = null;
                        break;
                }
            }
            return user;
        }
        internal static Person AdminStartPage(Person user, Booking booking)
        {
            Helpers.ShowUser(user);
            int input = 0;
            string[] menu = ("1. Hantera länder|2. Hantera städer|3. Hantera hotell|4. Hantera bokningar|5. Se statistik|6. Logga ut").Split('|');

            foreach (var item in menu)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nVälj från menyn: ");
            input = Helpers.TryNumber(input, 0, menu.Length);


            switch (input)
            {
                case 1:
                    Helpers.ManageCountries(user);
                    break;
                case 2:
                    Helpers.ManageCities(user);
                    break;
                case 3:
                    Helpers.ManageHotels(user);
                    break;
                case 4:
                    Menu.ViewUserBookings(user, booking);
                    break;
                case 5:
                    Menu.ManageStatistics(user, booking);
                    break;
                case 6:
                    user = null;
                    break;
            }
            return user;
        }

        private static void ManageCountries(Person user)
        {
            ShowUser(user);
            using (var db = new HotelContext())
            {
                var countryList = db.Countries.ToList();
                int answer = 0;

                string[] menu = ("1. Se vilka länder som finns|2. Lägg till land|3. Ta bort land|4. Återgå").Split('|');
                foreach (var choice in menu)
                {
                    Console.WriteLine(choice);
                }
                Console.Write("\nAnge vad du vill göra: ");
                answer = TryNumber(answer, 1, menu.Length);

                switch (answer)
                {
                    case 1:
                        Console.Clear();
                        ShowCountries();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nTryck [enter] för att återgå");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    case 2:
                        AddCountries(user);
                        break;
                    case 3:
                        RemoveCountries(user);
                        break;
                    case 4:
                        break;

                }
            }
        }

        internal static Person LogIn(Person user)
        {
            while (user == null)
            {
                using (var db = new HotelContext())
                {
                    var personList = db.Persons;
                    Console.Clear();
                    Console.WriteLine("Inloggning/Skapa användare:");
                    Console.WriteLine("--------------------------\n");
                    Console.Write("Ange namn: ");
                    string? nameInput = Console.ReadLine();
                    Console.Write("Ange lösenord: ");
                    string? passwordInput = Console.ReadLine();

                    var existingNamePerson = personList.SingleOrDefault(x => x.Name.ToLower().Trim() == nameInput.ToLower().Trim());
                    var existingUser = personList.SingleOrDefault(a => a.Name.ToLower().Trim() == nameInput.ToLower().Trim() && a.Password == passwordInput);

                    if (existingUser != null)
                    {
                        user = existingUser;
                    }
                    else if (existingNamePerson != null && existingUser == null)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\n\nFel lösenord. Försök igen genom att trycka på enter.");
                        Console.ReadKey();
                        Console.ResetColor();
                    }
                    // Create new user
                    else if (existingNamePerson == null && existingUser == null)
                    {
                        Console.WriteLine("Användaren finns inte.");
                        Console.Write("Vill du skapa en användare nu? [ja/nej]: ");
                        string? answer = Console.ReadLine();
                        if (answer.Trim().ToLower() == "ja")
                        {
                            var newPerson = new Person
                            {
                                Name = nameInput,
                                Password = passwordInput,
                                WorkTrip = false,
                                NumberOfGuests = 1
                            };
                            personList.Add(newPerson);
                            db.SaveChanges();
                            user = newPerson;
                        }
                        else
                        {
                            Console.WriteLine("Testa att logga in igen genom att trycka på enter");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
            }
            return user;
        }

        internal static void RemoveBooking(Person user, Booking booking, int removeProductId)
        {
            using (var db = new HotelContext())
            {
                var bookingToDelete = db.Bookings.Where(x => x.Id == removeProductId).SingleOrDefault();
                if (bookingToDelete != null)
                {
                    db.Bookings.Remove((Booking)bookingToDelete);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("\nDet gick inte att ta bort denna bokning!");
                }
            }
        }
    }
}
