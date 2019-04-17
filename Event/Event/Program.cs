using Event.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event
{
    class Program
    {
        private static  bool logedIn = false;
        static void Main(string[] args)
        {
            StartMenu();

        }

        private static void LogIn()
        {
            do
            {
                var _context = new EventContext();
                var userName = InputManager.InputString("Ange användarnamn.");
                var passWord = InputManager.InputString("Ange lösenord.");
                var thisUser = _context.User.Where(u => u.Passwd == passWord);
                if (thisUser.Any())
                {
                    logedIn = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Felaktig inloggning, prova igen....");
                }
            } while (!logedIn);
            Console.WriteLine(logedIn);
        }

        private static void ShowEvents()
        {
            var events = new Models.Event();
            var location = new Location();
            var repeat = true;
            while (repeat)
            {
                Console.Clear();
                Console.WriteLine("----- Alla Event-----");
                foreach (var item in events.ListAllEvents())
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }
                var thisEvent = int.Parse(InputManager.InputString("Ange vilket Event du vill se mer av!"));
                var returnedEvent = events.ShowEvent(thisEvent);
                var city = location.GetLocation(returnedEvent.LocationId);
                DateTime startDate = returnedEvent.StartDate.Value;
                DateTime endDate = returnedEvent.EndDate.Value;
                Console.Clear();
                Console.WriteLine(
                    $"{returnedEvent.Name}\n" +
                    $"Eventet startar: {startDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                    $"Eventet slutar: {endDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                    $"Eventet är i: {city.City}"
                    );
                Console.WriteLine("Vill du titta på ett event till? y/n");
                if (Console.ReadLine() != "y")
                    repeat = false;
            }
            Console.Clear();
            StartMenu();
        }

        private static void StartMenu()
        {
            Console.WriteLine("--- Start ---");
            Console.WriteLine("1. Visa Event");
            Console.WriteLine("2. Logga in");
            int selectedStartMenuAlternative = InputManager.InputInt("Välj meny alternativ");
            switch (selectedStartMenuAlternative)
            {
                case 1:
                    ShowEvents();
                    break;
                case 2:
                    LogIn();
                    break;
                default:
                    break;
            }
        }

        private static void AdminMenu()
        {
            Console.WriteLine("--- Admin Menu ---");
            Console.WriteLine("1. Skapa Event");
            Console.WriteLine("2. Avboka Event");
            Console.WriteLine("3. Visa alla Event");
            Console.WriteLine("4. Ta bort Ort");
            Console.WriteLine("5. Ta bort Typ");
            Console.WriteLine("6. Lägg till användare");
            Console.WriteLine("7. Ta bort användare");
        }

        private static void UserMenu()
        {
            Console.WriteLine("--- Användare Menu ---");
            Console.WriteLine("1. Visa alla Event");
            Console.WriteLine("2. Visa mina Event");
            Console.WriteLine("3. Boka av Event");
        }
    }
}
