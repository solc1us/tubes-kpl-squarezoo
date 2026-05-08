using System;

namespace tubes_kpl_squarezoo.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public User(string name, string role)
        {
            UserId = Guid.NewGuid();
            Name = name;
            Role = role;
        }
    }
}