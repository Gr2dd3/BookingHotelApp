using Microsoft.EntityFrameworkCore;
using BookingAppHotels.RunApp;
using BookingAppHotels.Context_Migrations;
using BookingAppHotels.Models;

public class Program
{
    private static void Main(string[] args)
    {
        bool runProgram = true;
        int input = 0;
        int welcomeCounter = 0;
        Person? user = null;
        Booking booking = new Booking();
        var cityList = new List<City>();
        while (runProgram)
        {
            Console.Clear();
            if (welcomeCounter < 1) { Helpers.Welcome(); welcomeCounter++; }
            // Log in
            if (user == null)
            {
                user = Helpers.LogIn(user);
            }
            if (user != null)
            {
                Helpers.ShowUser(user);
                if (user.Name == "admin")
                {
                    // Admin start page
                   user = Helpers.AdminStartPage(user, booking);
                }
                else
                {
                    // Customer start page
                   user = Helpers.CustomerStartPage(user, input, booking);
                }
            }
        }
    }
}