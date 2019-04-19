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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int LocationId { get; set; }
        public int TypeId { get; set; }
        public bool? Active { get; set; }

        public virtual Location Location { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<EventChatt> EventChatt { get; set; }
        public virtual ICollection<JoinEvent> JoinEvent { get; set; }

        public bool CreateEvent(string name, DateTime startDate, DateTime endDate, int locationId, int typeId)
        {
            var eventCreated = false;
            var _context = new EventContext();
            Event newEvent = new Event
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                LocationId = locationId,
                TypeId = typeId,
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

        public List<Event> ListAllEvents()
        {
            var _context = new EventContext();
            var events = _context.Event.ToList();
            return events;
        }

        public List<Event> ListAllActiveEvents()
        {
            var _context = new EventContext();
            var events = _context.Event.Where(c => c.Active == true).ToList();
            return events;
        }

        public Event ShowEvent(int id)
        {
            var _context = new EventContext();
            var requestedEvent = _context.Event.FirstOrDefault(e => e.Id == id);
            return requestedEvent;
        }

        public List<Event> ShowAllEventsForUser (int Id)
        {
            var _context = new EventContext();
            List<JoinEvent> JoinEvents = _context.JoinEvent.Where(j => j.UserId == Id).ToList();
            List<Models.Event> EventsForUser = new List<Models.Event>();

            foreach (var item in JoinEvents)
            {
                EventsForUser.Add(_context.Event.FirstOrDefault(e => e.Id == item.Id));
            }
            return EventsForUser;
        }
    }
}
