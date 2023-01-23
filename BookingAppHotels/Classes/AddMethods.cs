using BookingAppHotels.Context_Migrations;
using BookingAppHotels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.RunApp
{
    internal class AddMethods
    {
        internal static void AddCountries(Person user)
        {
            View.ShowUser(user);
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
                answer = Helpers.TryNumber(answer, 1, 2);
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
        internal static void AddCities(Person user)
        {
            View.ShowUser(user);
            bool correctInput = false;
            using (var db = new HotelContext())
            {
                var cityList = db.Cities;
                var countryList = db.Countries.ToList();

                while (!correctInput)
                {

                    int countryInput = 0;
                    View.ShowCountries();
                    Console.Write("\nVälj vilket land staden ska befinna sig i: ");
                    countryInput = Helpers.TryNumber(countryInput, 1, countryList.Count);
                    var existingCountryId = countryList.SingleOrDefault(x => x.Id == countryInput) != null;
                    Console.Clear();
                    View.ShowCities();
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
        internal static void AddHotels(Person user)
        {
            View.ShowUser(user);
            bool correctInput = false;
            using (var db = new HotelContext())
            {
                var hotelsList = db.Hotels;
                var roomList = db.Rooms;
                var countryList = db.Countries.ToList();
                var cityList = db.Cities.ToList();

                while (!correctInput)
                {
                    View.ShowCities();
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
                    floors = Helpers.TryNumber(floors, 1, 10);
                    Console.Write("Ange hur många rum per våning: ");
                    roomsPerFloor = Helpers.TryNumber(roomsPerFloor, 1, 20);
                    if (name == "å")
                    {
                        break;
                    }
                    var newHotelName = hotelsList.SingleOrDefault(x => x.Name == name) == null;
                    Console.Clear();
                    // Select Country
                    View.ShowCountries();
                    Console.WriteLine("\n----------------------");
                    Console.Write("Välj vilket land hotellet befinner sig i: ");
                    countryInput = Helpers.TryNumber(countryInput, 1, countryList.Count);
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
                    cityInput = Helpers.TryNumber(countryInput, 0, citiesInChosenCountry.Count);
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
    }
}
