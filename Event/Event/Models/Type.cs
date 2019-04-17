using System;
using System.Collections.Generic;
using System.Linq;

namespace Event.Models
{
    public partial class Type
    {
        public Type()
        {
            Event = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Event> Event { get; set; }

        public bool CreateType(string name, string eMail, string passWord, string role)
        {
            var typeCreated = false;
            var _context = new EventContext();
            Type type = new Type
            {
                Name = name,
            };

            _context.Type.Add(type);
            var result = _context.SaveChanges();

            if (result == 1)
            {
                typeCreated = true;
            }
            else
            {
                typeCreated = false;
            }
            return typeCreated;
        }

        public bool RemoveType(int id)
        {
            var typeDeleted = false;
            var _context = new EventContext();
            var type = _context.Type.Find(id);
            _context.Type.Remove(type);
            var result = _context.SaveChanges();
            if (result == 1)
            {
                typeDeleted = true;
            }
            else
            {
                typeDeleted = false;
            }
            return typeDeleted;
        }

        public List<Type> GetAllTypes()
        {
            var _context = new EventContext();
            var types = _context.Type.ToList();
            return types;
        }
    }
}
