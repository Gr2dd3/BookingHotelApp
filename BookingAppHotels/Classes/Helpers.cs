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
    internal class Helpers
    {
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
                        View.ShowUserBookings(user, booking);
                        break;
                    case 2:
                        Helpers.BookWichHotel(user, booking);
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
            View.ShowUser(user);
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
                    View.ShowUserBookings(user, booking);
                    break;
                case 5:
                    Helpers.ManageStatistics(user, booking);
                    break;
                case 6:
                    user = null;
                    break;
            }
            return user;
        }

        internal static void ManageStatistics(Person user, Booking booking)
        {
            Console.Clear();
            View.ShowUser(user);
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
        internal static void ManageHotels(Person user)
        {
            View.ShowUser(user);
            using (var db = new HotelContext())
            {
                var hotelsList = db.Hotels;
                int answer = 0;

                string[] menu = ("1. Se vilka hotell som finns|2. Lägg till hotell|3. Ta bort hotell|4. Återgå").Split('|');
                foreach (var choice in menu)
                {
                    Console.WriteLine(choice);
                }
                
                answer = Helpers.ChooseFromMenu(answer, 1, menu.Length);
                switch (answer)
                {
                    case 1:
                        Console.Clear();
                        View.ShowHotels(user);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nTryck [enter] för att återgå");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    case 2:
                            AddMethods.AddHotels(user);
                        break;
                    case 3:
                        RemoveMethods.RemoveHotels(user);
                        break;
                    case 4:
                        break;

                }
            }
        }
        internal static void ManageCities(Person user)
        {
            View.ShowUser(user);
            using (var db = new HotelContext())
            {
                var cityList = db.Cities.ToList();
                int answer = 0;

                string[] menu = ("1. Se vilka städer som finns|2. Lägg till stad|3. Ta bort stad|4. Återgå").Split('|');
                foreach (var choice in menu)
                {
                    Console.WriteLine(choice);
                }
                answer = Helpers.ChooseFromMenu(answer, 1, menu.Length);

                switch (answer)
                {
                    case 1:
                        Console.Clear();
                        View.ShowCities();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nTryck [enter] för att återgå");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    case 2:
                        AddMethods.AddCities(user);
                        break;
                    case 3:
                        RemoveMethods.RemoveCities(user);
                        break;
                    case 4:
                        break;

                }
            }
        }
        internal static void ManageCountries(Person user)
        {
            View.ShowUser(user);
            using (var db = new HotelContext())
            {
                var countryList = db.Countries.ToList();
                int answer = 0;

                string[] menu = ("1. Se vilka länder som finns|2. Lägg till land|3. Ta bort land|4. Återgå").Split('|');
                foreach (var choice in menu)
                {
                    Console.WriteLine(choice);
                }
                answer = Helpers.ChooseFromMenu(answer, 1, menu.Length);

                switch (answer)
                {
                    case 1:
                        Console.Clear();
                        View.ShowCountries();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nTryck [enter] för att återgå");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    case 2:
                        AddMethods.AddCountries(user);
                        break;
                    case 3:
                        RemoveMethods.RemoveCountries(user);
                        break;
                    case 4:
                        break;

                }
            }
        }



        internal static void ChangePriceForRoom(int outputRoom)
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
                    View.ShowUser(user);

                    View.ShowCountries();

                    // Choose City from Country
                    inputCountryId = Helpers.ChooseFromMenu(inputCountryId, 1, highestCountryIdNr.Id);
                    Console.Clear();
                    View.ShowUser(user);
                    outputCity = View.ShowCitiesWithCountryID(inputCountryId);

                    inputCityId = Helpers.ChooseFromMenu(inputCityId, 1, highestCityIdNr.Id);
                    outputCity = cityList[inputCityId - 1].Id;
                    Console.Clear();
                    View.ShowUser(user);

                    // Choose Hotel from City
                    var hotelListWithCityId = db.Hotels.Where(x => x.CityId == outputCity).OrderBy(x => x.Id).ToList();
                    outputHotel = View.ShowHotelsWithCityID(outputCity);
                    inputHotelId = Helpers.ChooseFromMenu(inputHotelId, 1, highestHotelIdNr.Id);
                    outputHotel = hotelListWithCityId[inputHotelId - 1].Id;
                    Console.Clear();

                    // Show Rooms in hotel
                    outputRoom = View.ShowRoomsOfHotelwithHotelID(user, outputHotel, outputRoom);

                    // Select available time of room
                    Helpers.BookCalenderForRoomWithId(user, outputRoom, booking);
                }
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
        internal static void BookDate(DateTime date, Person user, Room oneRoom, List<Booking> roomBooking)
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

        internal static int ChooseFromMenu(int input, int minValue, int maxValue)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nVälj Nr från menyn: ");
            Console.ResetColor();
            input = Helpers.TryNumber(input, minValue, maxValue);
            return input;
        }
        internal static int TryNumber(int number, int minValue, int maxValue)
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


  
    }
}
