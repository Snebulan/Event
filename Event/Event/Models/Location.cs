using System;
using System.Collections.Generic;
using System.Linq;

namespace Event.Models
{
    public partial class Location
    {
        public Location()
        {
            Event = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string City { get; set; }

        public virtual ICollection<Event> Event { get; set; }

        public bool CreateLocation(string name)
        {
            var LocationCreated = false;
            var _context = new EventContext();
            Location location = new Location
            {
                City = name,
            };

            _context.Location.Add(location);
            var result = _context.SaveChanges();

            if (result == 1)
            {
                LocationCreated = true;
            }
            else
            {
                LocationCreated = false;
            }
            return LocationCreated;
        }

        public bool RemoveLocation(int id)
        {
            var LocationDeleted = false;
            var _context = new EventContext();
            var location = _context.Location.Find(id);
            _context.Location.Remove(location);
            var result = _context.SaveChanges();
            if (result == 1)
            {
                LocationDeleted = true;
            }
            else
            {
                LocationDeleted = false;
            }
            return LocationDeleted;
        }

        public List<Location> ListAllLocations()
        {
            var _context = new EventContext();
            var locations = _context.Location.ToList();
            return locations;
        }

        public Location GetLocation(int Id)
        {
            var _context = new EventContext();
            return _context.Location.FirstOrDefault(c => c.Id == Id);
        }
    }
}
