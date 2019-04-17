using System;
using System.Collections.Generic;

namespace Event.Models
{
    public partial class EventChatMessage
    {
        public int Id { get; set; }
        public int EventChattId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime? DateTime { get; set; }

        public virtual EventChatt EventChatt { get; set; }
        public virtual User User { get; set; }
    }
}
