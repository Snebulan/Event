using System;
using System.Collections.Generic;
using System.Linq;

namespace Event.Models
{
    public partial class Chatt
    {
        public int Id { get; set; }
        public int RecieverId { get; set; }
        public int SenderId { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }

        public virtual User Reciever { get; set; }
        public virtual User Sender { get; set; }

        public List<Chatt> ShowAllRecievedMessages(int userId)
        {
            var _context = new EventContext();
            List<Chatt> Chatts = _context.Chatt.Where( c => c.RecieverId == userId).ToList();
            return Chatts;
        }

        public List<Chatt> ShowAllSentMessages(int userId)
        {
            var _context = new EventContext();
            List<Chatt> Chatts = _context.Chatt.Where(c => c.SenderId == userId).ToList();
            return Chatts;
        }
    }
}
