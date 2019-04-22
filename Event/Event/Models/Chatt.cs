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

        /// <summary>
        /// Returns a list of recieved messages for a user
        /// </summary>
        /// <param name="userId">Recievers ID</param>
        /// <returns>List of messages</returns>
        public List<Chatt> ShowAllRecievedMessages(int userId)
        {
            var _context = new EventContext();
            List<Chatt> Chatts = _context.Chatt.Where( c => c.RecieverId == userId).ToList();
            return Chatts;
        }

        /// <summary>
        /// Returns a list of all sent messages for a user
        /// </summary>
        /// <param name="userId">Senders ID</param>
        /// <returns>List of messages</returns>
        public List<Chatt> ShowAllSentMessages(int userId)
        {
            var _context = new EventContext();
            List<Chatt> Chatts = _context.Chatt.Where(c => c.SenderId == userId).ToList();
            return Chatts;
        }

        /// <summary>
        /// Saves a new message
        /// </summary>
        /// <param name="Message">Message to save</param>
        public void sendMessage(Chatt Message)
        {
            var _context = new EventContext();
            _context.Add(Message);
            _context.SaveChanges();
        }
    }
}
