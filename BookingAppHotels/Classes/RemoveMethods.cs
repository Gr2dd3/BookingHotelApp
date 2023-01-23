using BookingAppHotels.Context_Migrations;
using BookingAppHotels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAppHotels.RunApp
{
    internal class RemoveMethods
    {
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
        internal static void RemoveCountries(Person user)
        {
            View.ShowUser(user);
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
                        answer = Helpers.TryNumber(answer, 1, countryList.ToList().Count);
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
        internal static void RemoveCities(Person user)
        {
            View.ShowUser(user);
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
                        answer = Helpers.TryNumber(answer, 1, cityList.ToList().Count);
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
        internal static void RemoveHotels(Person user)
        {
            View.ShowUser(user);
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
                        answer = Helpers.TryNumber(answer, 1, hotelList.ToList().Count);
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
    }
}
