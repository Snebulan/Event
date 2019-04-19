using System;
using System.Collections.Generic;
using System.Linq;

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


        public bool CreateUser(User newUser)
        {
            var userCreated = false;
            var _context = new EventContext();
            User user = newUser;

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

        public bool RemoveUser(int id)
        {
            var userDeleted = false;
            var _context = new EventContext();
            var user =  _context.User.Find(id);
            _context.User.Remove(user);
            var result = _context.SaveChanges();
            if (result == 1)
            {
                userDeleted = true;
            }
            else
            {
                userDeleted = false;
            }
            return userDeleted;
        }

        public List<User> GetAllUsers()
        {
            var _context = new EventContext();
            var users = _context.User.ToList();
            return users;
        }

        public static implicit operator User(int v)
        {
            throw new NotImplementedException();
        }
    }
}
