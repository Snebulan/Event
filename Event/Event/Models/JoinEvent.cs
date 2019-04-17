using System;
using System.Collections.Generic;

namespace Event.Models
{
    public partial class JoinEvent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime? Date { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
