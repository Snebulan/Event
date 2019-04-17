using System;
using System.Collections.Generic;

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
    }
}
