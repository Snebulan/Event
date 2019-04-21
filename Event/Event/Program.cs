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
        /// <summary>
        /// Logs in user and sets logeInUser
        /// </summary>
        private static void LogIn()
        {
            var userRole = "";
            do
            {
                Console.Clear();
                var _context = new EventContext();
                Console.WriteLine("------- Logga in --------");
                var userName = InputManager.InputString("Ange användarnamn: ");
                var passWord = InputManager.InputString("Ange lösenord: ");
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

        /// <summary>
        /// Shows all events or all active events depending of typeOfEventList
        /// </summary>
        /// <param name="typeOfEventList">If 0 shows all events, if 1 shows all active events</param>
        private static void ShowEventsMenu(int typeOfEventList)
        {
            var events = new Models.Event();
            var location = new Location();
            var repeat = true;
            while (repeat)
            {
                Console.Clear();
                Console.WriteLine("------- Alla Event-------");
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
                Console.WriteLine("-------------------------");
                var thisEvent = int.Parse(InputManager.InputString("Ange vilket Event du vill se mer av!"));
                var returnedEvent = events.ShowEvent(thisEvent);
                var city = location.GetLocation(returnedEvent.LocationId);
                var startDate = returnedEvent.StartDate.ToString("dddd, dd MMMM yyyy ");
                var endDate = returnedEvent.EndDate.ToString("dddd, dd MMMM yyyy ");
                Console.Clear();
                Console.WriteLine(
                    $"{returnedEvent.Name}\n" +
                    $"Eventet startar: {startDate}\n" +
                    $"Eventet slutar: {endDate}\n" +
                    $"Eventet är i: {city.City}"
                    );
                Console.WriteLine("-------------------------");
                Console.WriteLine("Vill du titta på ett event till? y/n");
                if (Console.ReadLine() != "y")
                    repeat = false;
            }
            Console.Clear();
            RouteUser();
        }

        /// <summary>
        /// Checks privilegies for user and routes them to the correct menu
        /// </summary>
        private static void RouteUser()
        {
            Console.Clear();
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

        /// <summary>
        /// Creates event
        /// </summary>
        private static void CreateEventMenu()
        {
            var repeat = true;
            var newEvent = new Models.Event();
            var _context = new EventContext();
            List<Location> Locations = new List<Location>();
            List<Models.Type> Types = new List<Models.Type>();
            int city = 0;
            int typeOfEvent = 0;
            while (repeat)
            {
                var location = new Location();
                var type = new Models.Type();

                Console.Clear();
                Console.WriteLine("------ Skapa Event ------");
                newEvent.Name = InputManager.InputString("Ange namn på Event.");
                newEvent.StartDate = InputManager.InputDate("Ange startdatum.");
                newEvent.EndDate = InputManager.InputDate("Ange slutdatum.");
                Console.WriteLine("\n");
                Console.Clear();
                Console.WriteLine("----- Sparade orter -----");
                Locations = location.ListAllLocations();
                foreach (var item in Locations)
                {
                    Console.WriteLine($"{item.Id}. {item.City}");
                }
                Console.WriteLine("-------------------------");
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
                Console.Clear();
                Console.WriteLine("----- Sparade Typer -----");
                Types = type.ListAllTypes();
                foreach (var item in Types)
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }
                Console.WriteLine("-------------------------");
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
                var startDateString = newEvent.StartDate.ToString("dddd, dd MMMM yyyy ");
                var endDateString = newEvent.EndDate.ToString("dddd, dd MMMM yyyy ");
                Console.WriteLine($"Eventets namn: {newEvent.Name}\n" +
                    $"Start datum för Event: {startDateString}\n" +
                    $"Slut datum för Event: {endDateString}\n" +
                    $"Ort: {locationChosen}\n" +
                    $"Typ: {typeChosen}\n");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Vill du skapa eventet? y/n");
                if (Console.ReadLine() == "y")
                    repeat = false;
            }
            newEvent.LocationId = city;
            newEvent.TypeId = typeOfEvent;
            var eventToSave = new Models.Event();
            var eventCreated = eventToSave.CreateEvent(newEvent);
            Console.Clear();
            Console.WriteLine("----- Sparade Event -----");
            foreach (var item in eventToSave.ListAllEvents())
            {
                Console.WriteLine($"{item.Name}");
            }
            Console.WriteLine("\n");
            Console.WriteLine("-------------------OBS!!---------------------");
            Console.WriteLine("----Du återvänder straxt till Admin menyn ---");
            Console.WriteLine("---------------------------------------------");
            Thread.Sleep(4000);
            RouteUser();
        }

        /// <summary>
        /// shows the initial menu
        /// </summary>
        private static void StartMenu()
        {
            Console.WriteLine("--- Event Application ---");
            Console.WriteLine("1. Visa Event");
            Console.WriteLine("2. Logga in");
            Console.WriteLine("-------------------------");
            int selectedStartMenuAlternative = InputManager.InputInt("Välj meny alternativ:");
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

        /// <summary>
        /// shows admin menu
        /// </summary>
        private static void AdminMenu()
        {
            Console.WriteLine("------ Admin Meny -------");
            Console.WriteLine("1. Skapa Event");
            Console.WriteLine("2. Avboka Event");
            Console.WriteLine("3. Visa alla aktiva Event");
            Console.WriteLine("4. Visa alla Event");
            Console.WriteLine("5. Ta bort Ort");
            Console.WriteLine("6. Ta bort Typ");
            Console.WriteLine("7. Lägg till användare");
            Console.WriteLine("8. Ta bort användare");
            Console.WriteLine("-------------------------");
            int selectedStartMenuAlternative = InputManager.InputInt("Välj meny alternativ:");
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

        /// <summary>
        /// shows user menu
        /// </summary>
        private static void UserMenu()
        {
            Console.WriteLine("---- Användare Menu -----");
            Console.WriteLine("1. Visa alla Event");
            Console.WriteLine("2. Visa mina Event");
            Console.WriteLine("3. Boka Event");
            Console.WriteLine("4. Boka av Event");
            Console.WriteLine("5. Visa alla meddelanden");
            Console.WriteLine("6. Skicka meddelande");
            Console.WriteLine("-------------------------");
            int selectedStartMenuAlternative = InputManager.InputInt("Välj meny alternativ:");
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
                case 4:
                    CancelEventForUserMenu();
                    break;
                case 5:
                    ShowAllMessagesMenu();
                    break;
                case 6:
                    SendMessageMenu();
                    break;
                default:
                    break;
            }
        }

        private static void SendMessageMenu()
        {
            var _context = new EventContext();
            var thisUser = new User();
            var newMessage = new Chatt();
            var users = thisUser.GetAllUsersExceptLogedInUser(logedInUser.Id);
            bool repeat = true;

            while (repeat)
            {
                Console.Clear();
                Console.WriteLine("Skicka nytt meddelande!");
                foreach (var item in users)
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }


                newMessage.RecieverId = InputManager.InputInt("Välj mottagare.");
                newMessage.Message = InputManager.InputString("Skriv in ditt meddelande");
                newMessage.SenderId = logedInUser.Id;
                var reciever = _context.User.FirstOrDefault(u => u.Id == newMessage.RecieverId);
                Console.WriteLine(
                    $"Vill du skicka följande meddelande:\n" +
                    $"Mottagare: {reciever.Name}\n" +
                    $"Meddelande: {newMessage.Message}");
                Console.WriteLine("Vill du skicka meddelande? y/n");
                if (Console.ReadLine() == "y")
                    repeat = false;
            }
            newMessage.sendMessage(newMessage);
            RouteUser();

        }

        private static void ShowAllMessagesMenu()
        {
            var Chatts = new Chatt();
            var recievedMessages = Chatts.ShowAllRecievedMessages(logedInUser.Id);
            var sentMessages = Chatts.ShowAllSentMessages(logedInUser.Id);

            Console.Clear();
            Console.WriteLine("Mottagna Meddelanden:");
            foreach (var item in recievedMessages)
            {
                Console.WriteLine(item.Message);
            }
            Console.WriteLine("\n");
            Console.WriteLine("Skickade Meddelaneden:");
            foreach (var item in sentMessages)
            {
                Console.WriteLine(item.Message);
            }
        }

        /// <summary>
        /// removes user from db
        /// </summary>
        private static void RemoveUserMenu()
        {
            var _context = new EventContext();
            var users = _context.User.ToList();
            var userToRemove = new User();
            bool repeat = true;
            bool dontRemove = true;

            while (dontRemove)
            {
                Console.Clear();
                Console.WriteLine("--- Ta bort användare ---");
                foreach (var item in users)
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }
                while (repeat)
                {
                    Console.WriteLine("-------------------------");
                    var userIdToRemove = InputManager.InputInt("Välj användare att ta bort, eller tryck 0 för att avbryta");

                    if (userIdToRemove == 0)
                    {
                        RouteUser();
                    }
                    userToRemove = _context.User.FirstOrDefault(u => u.Id == userIdToRemove);
                    if (userToRemove.Name == logedInUser.Name)
                    {
                        Console.WriteLine("-------------------------------------- Varning!! --------------------------------------");
                        Console.WriteLine("Du kan inte ta bort den användare du är inloggad med! Välj annan användare att ta bort!");
                        Console.WriteLine("---------------------------------------------------------------------------------------");
                    }
                    else
                    {
                        repeat = false;
                    }
                }
                Console.WriteLine($"Är du säker på att du vill ta bort {userToRemove.Name}? y/n");
                if (Console.ReadLine() == "y")
                    dontRemove = false;
            }
            _context.User.Remove(userToRemove);
            _context.SaveChanges();
            RouteUser();
        }

        /// <summary>
        /// ads user to db
        /// </summary>
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
                    Console.Clear();
                    Console.WriteLine("-- Skapa ny användare ---");
                    newUserName = InputManager.InputString("Ange användarnamn: ");
                    List<string> UserNames = new List<string>();
                    UserNames = _context.User.Select(u => u.Name).ToList();
                    if (UserNames.Contains(newUserName))
                    {
                        Console.WriteLine("--------------- Varning!! --------------");
                        Console.WriteLine("Namnet är upptaget, välj ett annat namn!");
                        Console.WriteLine("----------------------------------------");
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

                Console.Clear();
                Console.WriteLine($"Användarens namn: {newUser.Name}\n" +
                    $"Lösenord: {newUser.Passwd}\n" +
                    $"Mail: {newUser.Mail}\n" +
                    $"Roll: {newUser.Role}\n");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Vill du skapa användaren? y/n");
                if (Console.ReadLine() == "y")
                    repeat = false;
            }
            var createTheNewUser = new User();
            createTheNewUser.CreateUser(newUser);
            RouteUser();
        }

        /// <summary>
        /// sets state of event to not active
        /// </summary>
        private static void CancelEventMenu()
        {
            Console.Clear();
            var _context = new EventContext();
            var Events = new Models.Event();
            var activeEvents = Events.ListAllActiveEvents();
            Console.WriteLine("--- Avaktivera Event ----");
            foreach (var item in activeEvents)
            {
                Console.WriteLine($"{item.Id}. {item.Name}");
            }
            Console.WriteLine("-------------------------");
            var eventToSetUnActive = InputManager.InputInt("Välj event att avaktivera, tryck 0 för att avbryta!");
            if (eventToSetUnActive == 0)
            {
                RouteUser();
            }
            Events.CancellEvent(eventToSetUnActive);
            RouteUser();
        }

        /// <summary>
        /// cancels event for user
        /// </summary>
        private static void CancelEventForUserMenu()
        {
            Console.Clear();
            var thisEvent = new Models.Event();
            var eventsForUser = thisEvent.ShowAllEventsForUser(logedInUser.Id);
            if (!eventsForUser.Any())
            {
                Console.WriteLine("Du är inte anmäld till några Event!");
                Thread.Sleep(3000);
                RouteUser();
            }
            else
            {
                var location = new Location();
                var repeat = true;
                while (repeat)
                {
                    Console.WriteLine("----- Avboka Event ------");
                    foreach (var item in eventsForUser)
                    {
                        Console.WriteLine($"{item.Id}. {item.Name}");
                    }
                    Console.WriteLine("-------------------------");
                    var selectedEventId = int.Parse(InputManager.InputString("Ange vilket Event du avboka, tryck 0 för att avbryta!"));
                    if (selectedEventId == 0)
                    {
                        RouteUser();
                    }
                    thisEvent = thisEvent.ShowEvent(selectedEventId);
                    location = location.GetLocation(thisEvent.LocationId);
                    var startDate = thisEvent.StartDate.ToString("dddd, dd MMMM yyyy ");
                    var endDate = thisEvent.EndDate.ToString("dddd, dd MMMM yyyy ");
                    Console.Clear();
                    Console.WriteLine(
                        $"{thisEvent.Name}\n" +
                        $"Eventet startar: {startDate}\n" +
                        $"Eventet slutar: {endDate}\n" +
                        $"Eventet är i: {location.City}"
                        );
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Vill du avboka det här eventet? y/n");
                    if (Console.ReadLine() == "y")
                        repeat = false;
                }
                var joinEvent = new JoinEvent();
                joinEvent.SignOff(logedInUser, thisEvent);
                RouteUser();
            }
        }

        /// <summary>
        /// signs up user to event
        /// </summary>
        private static void JoinEventMenu()
        {
            var _context = new EventContext();
            var thisEvent = new Models.Event();
            var location = new Location();
            var repeat = true;
            while (repeat)
            {
                Console.Clear();
                Console.WriteLine("-- Anmälan till Event ---");
                foreach (var item in thisEvent.GetEventsAvailableForUSer(logedInUser.Id))
                {
                    Console.WriteLine($"{item.Id}. {item.Name}");
                }
                Console.WriteLine("-------------------------");
                var selectedEventId = InputManager.InputInt("Välj Event som du vill anmäla dig till eller tryck 0 för att avbryta.");
                if (selectedEventId == 0)
                {
                    RouteUser();
                }
                thisEvent = thisEvent.ShowEvent(selectedEventId);
                location = location.GetLocation(thisEvent.LocationId);
                var startDate = thisEvent.StartDate.ToString("dddd, dd MMMM yyyy ");
                var endDate = thisEvent.EndDate.ToString("dddd, dd MMMM yyyy ");
                Console.Clear();
                Console.WriteLine(
                    $"{thisEvent.Name}\n" +
                    $"Eventet startar: {startDate}\n" +
                    $"Eventet slutar: {endDate}\n" +
                    $"Eventet är i: {location.City}"
                    );
                Console.WriteLine("-------------------------");
                Console.WriteLine("Vill du anmäla dig till det här eventet? y/n");
                if (Console.ReadLine() == "y")
                    repeat = false;
            }
            var joinEvent = new JoinEvent();
            joinEvent.SignUp(logedInUser, thisEvent);
            RouteUser();
        }

        /// <summary>
        /// shows all events that user is signup for
        /// </summary>
        private static void ShowAllEventsForUserMenu()
        {
            Console.Clear();
            var userEvents = new Models.Event();
            var eventsForUser = userEvents.ShowAllEventsForUser(logedInUser.Id);
            if (!eventsForUser.Any())
            {
                Console.WriteLine("-------------- OBS! ---------------");
                Console.WriteLine("Du är inte anmäld till några Event!");
                Console.WriteLine("-----------------------------------");
                Thread.Sleep(3000);
                RouteUser();
            }
            else
            {
                var location = new Location();
                var repeat = true;
                while (repeat)
                {
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("----- Anmälda Event -----");
                    foreach (var item in eventsForUser)
                    {
                        Console.WriteLine($"{item.Id}. {item.Name}");
                    }
                    Console.WriteLine("-------------------------");
                    var thisEvent = int.Parse(InputManager.InputString("Ange vilket Event du vill se mer av, tryck 0 för att avbryta!"));
                    if (thisEvent == 0)
                    {
                        RouteUser();
                    }
                    var returnedEvent = userEvents.ShowEvent(thisEvent);
                    var city = location.GetLocation(returnedEvent.LocationId);
                    var startDate = returnedEvent.StartDate.ToString("dddd, dd MMMM yyyy ");
                    var endDate = returnedEvent.EndDate.ToString("dddd, dd MMMM yyyy ");
                    Console.Clear();
                    Console.WriteLine(
                        $"{returnedEvent.Name}\n" +
                        $"Eventet startar: {startDate}\n" +
                        $"Eventet slutar: {endDate}\n" +
                        $"Eventet är i: {city.City}"
                        );
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Vill du titta på ett event till? y/n");
                    if (Console.ReadLine() != "y")
                        repeat = false;
                }
                RouteUser();
            }
        }

        /// <summary>
        /// removes location from db
        /// </summary>
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
                Console.WriteLine("-------------------------");
                locationId = InputManager.InputInt("Vilken plats vill du ta bort?, tryck 0 för att avbryta!");
                if (locationId == 0)
                {
                    RouteUser();
                }
                var events = _context.Event.Where(c => c.LocationId == locationId);

                if (events.Any())
                {
                    Console.WriteLine("\n-----------------Varning!!!------------------");
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
            RouteUser();
        }

        /// <summary>
        /// removes type
        /// </summary>
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
                Console.WriteLine("-----------------------");
                typeId = InputManager.InputInt("Vilken typ vill du ta bort?, tryck 0 för att avbryta!");
                if (typeId == 0)
                {
                    RouteUser();
                }
                var events = _context.Event.Where(c => c.TypeId == typeId);

                if (events.Any())
                {
                    Console.WriteLine("\n-----------------Varning!!!------------------");
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
            RouteUser();
        }
    }
}
