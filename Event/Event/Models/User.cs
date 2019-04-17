﻿using System;
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


        public bool CreateUser(string name, string eMail, string passWord, string role)
        {
            var userCreated = false;
            var _context = new EventContext();
            User user = new User
            {
                Name = name,
                Passwd = passWord,
                Mail = eMail,
                Role = role
            };

            _context.User.Add(user);
            var result = _context.SaveChanges();

            if (result == 1)
            {
                userCreated = true;
            }
            else
            {
                userCreated = false;
            }
            return userCreated;
        }
    }
}
