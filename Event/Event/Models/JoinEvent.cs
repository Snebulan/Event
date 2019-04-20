using System;
using System.Collections.Generic;
using System.Linq;

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

        public void SignUp(User user, Event chosenEvent)
        {
            var _context = new EventContext();
            JoinEvent join = new JoinEvent
            {
            UserId = user.Id,
            EventId = chosenEvent.Id,
            Date = DateTime.Now,
            };

            _context.JoinEvent.Add(join);
            _context.SaveChanges();
        }

        public void SignOff(User user, Event chosenEvent)
        {
            var _context = new EventContext();
            var join = _context.JoinEvent.FirstOrDefault(e => e.UserId == user.Id && e.EventId == chosenEvent.Id);

            _context.JoinEvent.Remove(join);
            _context.SaveChanges();
        }
    }
}
