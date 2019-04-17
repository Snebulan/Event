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

        public void SignUp(User user, Event chosenEvent)
        {
            var _context = new EventContext();
            var thisUser = _context.User.Find(user.Id);
            var thisEvent = _context.Event.Find(chosenEvent.Id);

            JoinEvent join = new JoinEvent
            {
                UserId = thisUser.Id,
                EventId = thisEvent.Id,
                Date = DateTime.Now,
                Event = thisEvent,
                User = thisUser,
            };

            _context.JoinEvent.Add(join);
            _context.SaveChanges();
        }

        public void SignOff(User user, Event chosenEvent)
        {
            var _context = new EventContext();
            var thisUser = _context.User.Find(user.Id);
            var thisEvent = _context.Event.Find(chosenEvent.Id);
            var join = _context.JoinEvent.Find(user.Id, chosenEvent.Id);

            _context.JoinEvent.Remove(join);
            _context.SaveChanges();
        }
    }
}
