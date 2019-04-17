using System;
using System.Collections.Generic;

namespace Event.Models
{
    public partial class User
    {
        public User()
        {
            ChattReciever = new HashSet<Chatt>();
            ChattSender = new HashSet<Chatt>();
            EventChatMessage = new HashSet<EventChatMessage>();
            JoinEvent = new HashSet<JoinEvent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Passwd { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Chatt> ChattReciever { get; set; }
        public virtual ICollection<Chatt> ChattSender { get; set; }
        public virtual ICollection<EventChatMessage> EventChatMessage { get; set; }
        public virtual ICollection<JoinEvent> JoinEvent { get; set; }
    }
}
