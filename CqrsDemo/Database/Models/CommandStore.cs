using System;

namespace CqrsDemo.Database.Models
{

    public class CommandStore
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
    }

}
