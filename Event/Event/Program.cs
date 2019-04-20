using Event.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Event
{
    class Program
    {
        private static bool logedIn = false;
        private static string logedinUserName = "";
        private static User logedInUser;

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
                        logedInUser = thisUser;
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

        private static void ShowEventsMenu(int typeOfEventList)
        {
            var events = new Models.Event();
            var location = new Location();
            var repeat = true;
            while (repeat)
            {
                Console.Clear();
                Console.WriteLine("----- Alla Event-----");
                if (typeOfEventList == 0)
                {
                    foreach (var item in events.ListAllEvents())
                    {
                        Console.WriteLine($"{item.Id}. {item.Name}");
                    }
                }
                else if (typeOfEventList == 1)
                {
                    foreach (var item in events.ListAllActiveEvents())
                    {
                        Console.WriteLine($"{item.Id}. {item.Name}");
                    }
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
            RouteUser();
        }

        private static void RouteUser()
        {
            if (logedInUser == null)
            {
                StartMenu();
            }
            var userRole = logedInUser.Role;
            switch (userRole)
            {
                case "admin":
                    AdminMenu();
                    break;
                case "user":
                    UserMenu();
                    break;
                default:

                    break;
            }
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
                city = InputManager.InputInt("Välj ort för eventet eller 0 för att skapa ny.");
                string locationChosen = "";
                if (city == 0)
                {
                    locationChosen = InputManager.InputString("Ange ortens namn: ");
                    var newLocation = location.CreateLocation(locationChosen);

                    var newLocationId = _context.Location.First(l => l.City == locationChosen);

                    city = newLocationId.Id;
                }
                else
                {
                    locationChosen = _context.Location.First(l => l.Id == city).City;
                }
                Console.WriteLine("\n");
                Types = type.ListAllTypes();
                foreach (var item in Types)
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }
                typeOfEvent = InputManager.InputInt("Välj vilken typ av event eller 0 för att skapa ny.");
                string typeChosen = "";
                if (typeOfEvent == 0)
                {
                    typeChosen = InputManager.InputString("Ange typens namn: ");
                    var newType = type.CreateType(typeChosen);

                    var newTypeId = _context.Type.First(x => x.Name == typeChosen);

                    typeOfEvent = newTypeId.Id;

                }
                else
                {
                    typeChosen = _context.Type.First(l => l.Id == typeOfEvent).Name;
                }
                Console.Clear();

                Console.WriteLine($"Eventets namn: {eventNamn}\n" +
                    $"Start datum för Event: {startDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                    $"Slut datum för Event: {endDate.ToString("dddd, dd MMMM yyyy ")}\n" +
                    $"Ort: {locationChosen}\n" +
                    $"Typ: {typeChosen}\n");

                Console.WriteLine("Vill du skapa eventet? y/n");
                if (Console.ReadLine() == "y")
                    repeat = false;
            }
            var eventToSave = new Models.Event();
            var eventCreated = eventToSave.CreateEvent(eventNamn, startDate, endDate, city, typeOfEvent);
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
                    ShowEventsMenu(1);
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
            Console.WriteLine("3. Visa alla aktiva Event");
            Console.WriteLine("4. Visa alla Event");
            Console.WriteLine("5. Ta bort Ort");
            Console.WriteLine("6. Ta bort Typ");
            Console.WriteLine("7. Lägg till användare");
            Console.WriteLine("8. Ta bort användare");
            int selectedStartMenuAlternative = InputManager.InputInt("Välj meny alternativ");
            switch (selectedStartMenuAlternative)
            {
                case 1:
                    CreateEventMenu();
                    break;
                case 2:
                    CancelEventMenu();
                    break;
                case 3:
                    ShowEventsMenu(1);
                    break;
                case 4:
                    ShowEventsMenu(0);
                    break;
                case 5:
                    RemoveLocationMenu();
                    break;
                case 6:
                    RemoveTypeMenu();
                    break;
                case 7:
                    CreateUserMenu();
                    break;
                case 8:
                    RemoveUserMenu();
                    break;
                default:
                    break;
            }

        }

        private static void RemoveUserMenu()
        {
            var _context = new EventContext();
            var users = _context.User.ToList();
            var userToRemove = new User();
            bool repeat = true;
            bool dontRemove = true;

            while (dontRemove)
            {
                foreach (var item in users)
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }
                while (repeat)
                {
                    var userIdToRemove = InputManager.InputInt("Välj användare att ta bort, eller tryck 0 för att avbryta");

                    if (userIdToRemove == 0)
                    {
                        Console.Clear();
                        RouteUser();
                    }
                    userToRemove = _context.User.FirstOrDefault(u => u.Id == userIdToRemove);
                    if (userToRemove.Name == logedInUser.Name)
                    {
                        Console.WriteLine("Du kan inte ta bort den användare du är inloggad med! Välj annan användare att ta bort!");
                    }
                    else
                    {
                        repeat = false;
                    }
                }
                Console.WriteLine($"Är du säker på att du vill ta bort {userToRemove.Name}?");

                Console.WriteLine("Vill du skapa eventet? y/n");
                if (Console.ReadLine() == "y")
                    dontRemove = false;
            }
            _context.User.Remove(userToRemove);
            _context.SaveChanges();
            RouteUser();
        }

        private static void CreateUserMenu()
        {
            bool repeat = true;
            User newUser = new User();
            while (repeat)
            {
                bool userExcists = false;
                var newUserName = "";

                var _context = new EventContext();
                do
                {
                    Console.WriteLine("----- Skapa ny användare -----");
                    newUserName = InputManager.InputString("Ange användarnamn: ");
                    List<string> UserNames = new List<string>();
                    UserNames = _context.User.Select(u => u.Name).ToList();
                    if (UserNames.Contains(newUserName))
                    {
                        Console.WriteLine("Namnet är upptaget, välj ett annat namn!");
                    }
                    else
                    {
                        userExcists = true;
                    }

                } while (!userExcists);
                newUser.Name = newUserName;
                newUser.Passwd = InputManager.InputString("Ange lösenord: ");
                newUser.Mail = InputManager.InputString("Ange mail: ");
                newUser.Role = InputManager.InputString("Ange vilken roll användaren har: admin alt user");


                Console.WriteLine($"Användarens namn: {newUser.Name}\n" +
                    $"Lösenord: {newUser.Passwd}\n" +
                    $"Mail: {newUser.Mail}\n" +
                    $"Roll: {newUser.Role}\n");
                Console.WriteLine("Vill du skapa användaren? y/n");
                if (Console.ReadLine() == "y")
                    repeat = false;
            }
            var createTheNewUser = new User();
            createTheNewUser.CreateUser(newUser);
            RouteUser();
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
            AdminMenu();

        }

        private static void UserMenu()
        {
            Console.WriteLine("--- Användare Menu ---");
            Console.WriteLine("1. Visa alla Event");
            Console.WriteLine("2. Visa mina Event");
            Console.WriteLine("3. Boka Event");
            Console.WriteLine("4. Boka av Event");
            int selectedStartMenuAlternative = InputManager.InputInt("Välj meny alternativ");
            switch (selectedStartMenuAlternative)
            {
                case 1:
                    ShowEventsMenu(1);
                    break;
                case 2:
                    ShowAllEventsForUserMenu();
                    break;
                case 3:
                    JoinEventMenu();
                    break;
                default:
                    break;
            }
        }

        private static void JoinEventMenu()
        {
            var newEvents = new Models.Event();
            
            foreach (var item in newEvents.ListAllActiveEvents())
            {
                Console.WriteLine($"{item.Id}. {item.Name}");
            }

        }

        private static void ShowAllEventsForUserMenu()
        {
            var userEvents = new Models.Event();
            var eventsForUser = userEvents.ShowAllEventsForUser(logedInUser.Id);
            if (!eventsForUser.Any())
            {
                Console.WriteLine("Du är inte anmäld till några Event!");
                Thread.Sleep(3000);
                Console.Clear();
                RouteUser();
            }
            else
            {
                foreach (var item in eventsForUser)
                {
                    Console.WriteLine($"{item.Name}");
                }
                var thisEvent = int.Parse(InputManager.InputString("Ange vilket Event du vill se mer av, tryck 0 för att avbryta!"));
                if (thisEvent == 0)
                {
                    RouteUser();
                }
            }

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

        private static void RemoveLocationMenu()
        {
            Console.Clear();
            var _context = new EventContext();
            var locations = new Location();
            bool locationIsUsed = false;
            var locationId = 0;

            do
            {
                Console.WriteLine("----- Ta bort plats -----");
                foreach (var location in locations.ListAllLocations())
                {
                    Console.WriteLine($"{location.Id}. {location.City}");
                }
                Console.WriteLine("------------------------------------");
                locationId = InputManager.InputInt("Vilken plats vill du ta bort? (välj \"0\" för att avsluta)");
                if (locationId == 0)
                {
                    Console.Clear();
                    RouteUser();
                }
                var events = _context.Event.Where(c => c.LocationId == locationId);

                if (events.Any())
                {
                    Console.WriteLine("-----------------Varning!!!------------------");
                    Console.WriteLine("Platsen du försöker ta bort används, välj ny!");
                    Console.WriteLine("---------------------------------------------");
                    Thread.Sleep(3000);
                    Console.Clear();
                }
                else
                {
                    locationIsUsed = true;
                }

            } while (!locationIsUsed);

            locations.RemoveLocation(locationId);
            Console.Clear();
            RouteUser();
        }
        private static void RemoveTypeMenu()
        {
            Console.Clear();
            var _context = new EventContext();
            var types = new Models.Type();
            bool typeIsUsed = false;
            var typeId = 0;

            do
            {
                Console.WriteLine("----- Ta bort typ -----");
                foreach (var type in types.ListAllTypes())
                {
                    Console.WriteLine($"{type.Id}. {type.Name}");
                }
                Console.WriteLine("------------------------------------");
                typeId = InputManager.InputInt("Vilken typ vill du ta bort? (välj \"0\" för att avsluta)");
                if (typeId == 0)
                {
                    Console.Clear();
                    RouteUser();
                }
                var events = _context.Event.Where(c => c.TypeId == typeId);

                if (events.Any())
                {
                    Console.WriteLine("-----------------Varning!!!------------------");
                    Console.WriteLine("Typen du försöker ta bort används, välj ny!");
                    Console.WriteLine("---------------------------------------------");
                    Thread.Sleep(3000);
                    Console.Clear();
                }
                else
                {
                    typeIsUsed = true;
                }

            } while (!typeIsUsed);

            types.RemoveType(typeId);
            Console.Clear();
            RouteUser();
        }
    }
}
