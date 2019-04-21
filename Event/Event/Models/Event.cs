using System;
using System.Collections.Generic;
using System.Linq;

namespace Event.Models
{
    public partial class Event
    {

        public Event()
        {
            EventChatt = new HashSet<EventChatt>();
            JoinEvent = new HashSet<JoinEvent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LocationId { get; set; }
        public int TypeId { get; set; }
        public bool? Active { get; set; }

        public virtual Location Location { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<EventChatt> EventChatt { get; set; }
        public virtual ICollection<JoinEvent> JoinEvent { get; set; }

        /// <summary>
        /// ads event to db
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns></returns>
        public bool CreateEvent(Event newEvent)
        {
            var eventCreated = false;
            var _context = new EventContext();
            newEvent = new Event
            {
                Name = newEvent.Name,
                StartDate = newEvent.StartDate,
                EndDate = newEvent.EndDate,
                LocationId = newEvent.LocationId,
                TypeId = newEvent.TypeId,
                Active = true
            };

            _context.Event.Add(newEvent);
            var result = _context.SaveChanges();

            if (result == 1)
            {
                eventCreated = true;
            }
            else
            {
                eventCreated = false;
            }
            return eventCreated;
        }

        /// <summary>
        /// sets event state to not active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CancellEvent(int id)
        {
            var cancelledEvent = false;
            var _context = new EventContext();
            var eventToCancell = _context.Event.Find(id);
            eventToCancell.Active = false;
            var result = _context.SaveChanges();
            if (result == 1)
            {
                cancelledEvent = true;
            }
            else
            {
                cancelledEvent = false;
            }
            return cancelledEvent;
        }

        /// <summary>
        /// Gets all events from db
        /// </summary>
        /// <returns>List<event></returns>
        public List<Event> ListAllEvents()
        {
            var _context = new EventContext();
            var events = _context.Event.ToList();
            return events;
        }

        /// <summary>
        /// Gets all active events
        /// </summary>
        /// <returns>List<event></returns>
        public List<Event> ListAllActiveEvents()
        {
            var _context = new EventContext();
            var events = _context.Event.Where(c => c.Active == true).ToList();
            return events;
        }

        /// <summary>
        /// Gets a certain event
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event</returns>
        public Event ShowEvent(int id)
        {
            var _context = new EventContext();
            var requestedEvent = _context.Event.FirstOrDefault(e => e.Id == id);
            return requestedEvent;
        }

        /// <summary>
        /// Returns all event that user is signed up for
        /// </summary>
        /// <param name="Id"></param>
        /// <returnsList<Event>></returns>
        public List<Event> ShowAllEventsForUser (int Id)
        {
            var _context = new EventContext();
            List<JoinEvent> JoinEvents = _context.JoinEvent.Where(j => j.UserId == Id).ToList();
            List<Models.Event> EventsForUser = new List<Models.Event>();

            foreach (var item in JoinEvents)
            {
                EventsForUser.Add(_context.Event.FirstOrDefault(e => e.Id == item.EventId));
            }
            return EventsForUser;
        }

        public List<Event> GetEventsAvailableForUSer(int userId)
        {
            var _context = new EventContext();
            List<Models.Event> activeEvents = ListAllActiveEvents();
            List<JoinEvent> joinEventsForUser = _context.JoinEvent.Where(j => j.UserId == userId).ToList();
            List<Models.Event> eventsAvailableForUser = new List<Event>();
            IEnumerable<Event> result = activeEvents.Where(a => !joinEventsForUser.Any(p1 => p1.EventId == a.Id));
            foreach (var item in result)
            {
                eventsAvailableForUser.Add(item);
            }
            return eventsAvailableForUser;
        }
    }
}
