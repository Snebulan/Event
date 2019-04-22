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

        /// <summary>
        /// Creates a new type
        /// </summary>
        /// <param name="name">The name of the type</param>
        /// <returns>Returns a bool if the type is created or not </returns>
        public bool CreateType(string name)
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

        /// <summary>
        /// Removes a type
        /// </summary>
        /// <param name="name">The name of the type</param>
        /// <returns>Returns a bool if the type is removed or not </returns>
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

        /// <summary>
        /// Creates a list of types
        /// </summary>
        /// <returns>A list of types</returns>
        public List<Type> ListAllTypes()
        {
            var _context = new EventContext();
            var types = _context.Type.ToList();
            return types;
        }
    }
}
