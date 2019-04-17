using Event.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event
{
    class Program
    {
        private static  bool logedIn = false;
        private static string logedinUserName = "";
        static void Main(string[] args)
        {
            StartMenu();

        }

        private static void LogIn()
        {
            var userRole = "";
            do
            {
                var _context = new EventContext();
                var userName = InputManager.InputString("Ange användarnamn.");
                var passWord = InputManager.InputString("Ange lösenord.");
                if (_context.User.Any(c => c.Name == userName))
                {
                    var thisUser = _context.User.FirstOrDefault(u => u.Name == userName);
                    if (thisUser.Name == userName && thisUser.Passwd == passWord)
                    {
                        logedIn = true;
                        logedinUserName = userName;
                        userRole = thisUser.Role;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Felaktig inloggning, prova igen....");
                }
            } while (!logedIn);
            Console.Clear();
            Console.WriteLine($"\nVälkommen {logedinUserName}!\n");
            if (userRole == "admin")
            {
                AdminMenu();

            }
            else
            {
                UserMenu();
            }


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

        private static void CreateEvent()
        {
            var location = new Location();
            var type = new Models.Type();
            var eventNamn = InputManager.InputString("Ange namn på Event.");
            var startDate = InputManager.InputDate("Ange startdatum.");
            var endDate = InputManager.InputDate("Ange slutdatum.");
            Console.WriteLine("\n");
            var locations = location.ListAllLocations();
            foreach (var item in locations)
            {
                Console.WriteLine($"{item.Id}. {item.City}");
            }
            var city = InputManager.InputInt("Välj ort för eventet.");
            Console.WriteLine("\n");
            var types = type.ListAllTypes();
            foreach (var item in types)
            {
                Console.WriteLine($"{item.Id}. {item.Name}");
            }
            var typeOfEvent = InputManager.InputInt("Välj vilken typ av event.");
            Console.Clear();

            Console.WriteLine(
                $"Eventets namn: {eventNamn}\n" +
                $"Start datum för Event: {startDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                $"Slut datum för Event: {endDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                $"Ort: {locations.FirstOrDefault(c => c.Id == city).City}\n" +
                $"Typ: {types.FirstOrDefault(t => t.Id == typeOfEvent).Name}\n"       
                );
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
            int selectedStartMenuAlternative = InputManager.InputInt("Välj meny alternativ");
            switch (selectedStartMenuAlternative)
            {
                case 1:
                    CreateEvent();
                    break;
                case 2:
                    LogIn();
                    break;
                default:
                    break;
            }

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
