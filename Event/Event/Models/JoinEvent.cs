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

        /// <summary>
        /// Signs up a user for an event
        /// </summary>
        /// <param name="user">User to sign up</param>
        /// <param name="chosenEvent">Event to sign up on</param>
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

        /// <summary>
        /// Cancels a sign up for a user on an event
        /// </summary>
        /// <param name="user">User to cancel sign up</param>
        /// <param name="chosenEvent">Event to cancel sign up</param>
        public void SignOff(User user, Event chosenEvent)
        {
            var _context = new EventContext();
            var join = _context.JoinEvent.FirstOrDefault(e => e.UserId == user.Id && e.EventId == chosenEvent.Id);

            _context.JoinEvent.Remove(join);
            _context.SaveChanges();
        }
    }
}
