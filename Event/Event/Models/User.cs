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

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="newUser">User user</param>
        /// <returns>bool created</returns>
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

        /// <summary>
        /// removes a user
        /// </summary>
        /// <param name="id">Id of user to be removed</param>
        /// <returns>bool userDeleted</returns>
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

        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns>List <User></User></returns>
        public List<User> GetAllUsers()
        {
            var _context = new EventContext();
            var users = _context.User.ToList();
            return users;
        }

        /// <summary>
        /// Gets al users except the one thats loggedin
        /// </summary>
        /// <param name="userId">id og loggedInUser</param>
        /// <returns>List<user></user></returns>
        public List<User> GetAllUsersExceptLogedInUser(int userId)
        {
            var _context = new EventContext();
            List<User> users = _context.User.Where(u => u.Id != userId && u.Role != "admin").ToList();
            return users;
        }
    }
}
