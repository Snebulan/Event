using System;
using System.Collections.Generic;

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
    }
}
