using System;
using System.Collections.Generic;
using System.Linq;

namespace tubes_kpl_squarezoo.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string NoHP { get; set; }

        // Table-driven permission
        private static readonly Dictionary<string, HashSet<string>> PermissionTable =
            new Dictionary<string, HashSet<string>>()
        {
            {
                "User",
                new HashSet<string>
                {
                    "Report:Create",
                    "Report:Update",
                    "Evidence:Add"
                }
            },
            {
                "Admin",
                new HashSet<string>
                {
                    "Report:ViewAll",
                    "Report:FilterStatus",
                    "Report:Close"
                }
            }
        };

        // Constructor utama
        public User(Guid userId, string name, string role)
        {
            UserId = userId;
            Name = name;
            Role = role;
            NoHP = "";
        }

        // Constructor kompatibel kode lama
        public User(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
            Role = "User";
            NoHP = "";
        }

        // Constructor untuk UserService
        public User(string name, string phone)
        {
            UserId = Guid.NewGuid();
            Name = name;
            NoHP = phone;
            Role = "User";
        }

        // Check permission
        public bool CanPerform(string action)
        {
            if (!PermissionTable.ContainsKey(Role))
                return false;

            return PermissionTable[Role].Contains(action);
        }

        // Get all permissions
        public List<string> GetPermissions()
        {
            if (!PermissionTable.ContainsKey(Role))
                return new List<string>();

            return PermissionTable[Role].ToList();
        }
    }
}