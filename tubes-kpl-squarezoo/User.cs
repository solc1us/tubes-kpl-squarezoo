using System;
using System.Collections.Generic;
using System.Linq;

public class User
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string NoHP { get; set; }

    private Dictionary<string, bool> permissions;

    public User(Guid userId, string name, string noHP)
    {
        UserId = userId;
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