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

        private static void ShowEventsMenu()
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

        private static void CreateEventMenu()
        {
            var repeat = true;
            var eventNamn = "";
            var _context = new EventContext();
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            List<Location> Locations = new List<Location>();
            List<Models.Type> Types = new List<Models.Type>();
            int city = 0;
            int typeOfEvent = 0;
            while (repeat)
            {
                var location = new Location();
                var type = new Models.Type();
                eventNamn = InputManager.InputString("Ange namn på Event.");
                startDate = InputManager.InputDate("Ange startdatum.");
                endDate = InputManager.InputDate("Ange slutdatum.");
                Console.WriteLine("\n");
                Locations = location.ListAllLocations();
                foreach (var item in Locations)
                {
                    Console.WriteLine($"{item.Id}. {item.City}");
                }
                city = InputManager.InputInt("Välj ort för eventet.");
                Console.WriteLine("\n");
                Types = type.ListAllTypes();
                foreach (var item in Types)
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }
                typeOfEvent = InputManager.InputInt("Välj vilken typ av event.");
                Console.Clear();

                Console.WriteLine(
                    $"Eventets namn: {eventNamn}\n" +
                    $"Start datum för Event: {startDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                    $"Slut datum för Event: {endDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                    $"Ort: {Locations.FirstOrDefault(c => c.Id == city).City}\n" +
                    $"Typ: {Types.FirstOrDefault(t => t.Id == typeOfEvent).Name}\n"
                    );

                Console.WriteLine("Vill du skap eventet? y/n");
                if (Console.ReadLine() == "y")
                    repeat = false;
            }
            var eventToSave = new Models.Event();
            eventToSave.CreateEvent(eventNamn, startDate,endDate, city, typeOfEvent);
            foreach (var item in eventToSave.ListAllEvents())
            {
                Console.WriteLine($"{item.Name}");
            }
            Console.WriteLine("\n");
            AdminMenu();
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
                    ShowEventsMenu();
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
                    CreateEventMenu();
                    break;
                case 2:
                    CancelEventMenu();
                    break;
                default:
                    break;
            }

        }

        private static void CancelEventMenu()
        {
            var _context = new EventContext();
            var Events = new Models.Event();
            var activeEvents = Events.ListAllActiveEvents();
            foreach (var item in activeEvents)
            {
                Console.WriteLine($"{item.Id}. {item.Name}");
            }
            var eventToSetUnActive = InputManager.InputInt("Välj event att avaktivera.");
            Events.CancellEvent(eventToSetUnActive);


        }

        private static void UserMenu()
        {
            Console.WriteLine("--- Användare Menu ---");
            Console.WriteLine("1. Visa alla Event");
            Console.WriteLine("2. Visa mina Event");
            Console.WriteLine("3. Boka av Event");
        }

        private static void canceluser()
        {
            var _context = new EventContext();
            var userId = _context.User.FirstOrDefault(c => c.Name == logedinUserName).Id;

            var eventsId = _context.JoinEvent.ToList().Where(c => c.UserId == userId);
            List<Models.Event> Events = new List<Models.Event>();
            foreach (var item in eventsId)
            {
                Console.WriteLine(
                    $"{_context.Event.FirstOrDefault(c => c.Id == item.EventId).Id}. " +
                    $"{_context.Event.FirstOrDefault(c => c.Id == item.EventId).Name}");
            }
            var eventToCancelId = InputManager.InputInt("Vilket event vill du avboka?");
        }
    }
}
