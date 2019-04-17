using System;
using System.Collections.Generic;

namespace Event.Models
{
    public partial class EventChatt
    {
        public EventChatt()
        {
            EventChatMessage = new HashSet<EventChatMessage>();
        }

        public int Id { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual ICollection<EventChatMessage> EventChatMessage { get; set; }
    }
}
