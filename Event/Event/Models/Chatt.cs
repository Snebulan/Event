using System;
using System.Collections.Generic;

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
    }
}
