using System;
using System.Collections.Generic;

namespace Event.Models
{
    public partial class Type
    {
        public Type()
        {
            Event = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Event> Event { get; set; }
    }
}
