using System;

namespace tubes_kpl_squarezoo.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string NoHP { get; set; }

        private Dictionary<string, bool> permissions;

        public User(string name, string noHP)
        {
            UserId = Guid.NewGuid();
            Name = name;
            NoHP = noHP;
            permissions = new Dictionary<string, bool>();
        }


        public void AddPermission(string action)
        {
            if (!permissions.ContainsKey(action))
                permissions.Add(action, true);
        }

        public bool CanPerform(string action)
        {
            return permissions.ContainsKey(action) && permissions[action];
        }

        public List<string> GetPermissions()
        {
            return permissions.Keys.ToList();
        }
    }
}