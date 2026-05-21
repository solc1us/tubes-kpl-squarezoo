using System;
using System.Collections.Generic;
using System.Linq;

public class User
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string NoHP { get; set; }
    public string Role { get; set; }

    // Table-Driven Permission Dictionary
    private static Dictionary<string, HashSet<string>> PermissionTable =
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

    public User(Guid userId, string name, string noHP, string role)
    {
        UserId = userId;
        Name = name;
        NoHP = noHP;
        Role = role;
    }

    public bool CanPerform(string action)
    {
        if (!PermissionTable.ContainsKey(Role))
            return false;

        return PermissionTable[Role].Contains(action);
    }

    public List<string> GetPermissions()
    {
        if (!PermissionTable.ContainsKey(Role))
            return new List<string>();

        return PermissionTable[Role].ToList();
    }
}