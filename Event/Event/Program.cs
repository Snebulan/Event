using Event.Models;
using System;

namespace Event
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var db = new EventContext();
            foreach (var item in db.User)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
