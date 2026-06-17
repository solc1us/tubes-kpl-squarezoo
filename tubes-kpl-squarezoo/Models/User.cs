using System;
using tubes_kpl_squarezoo.Enums;

namespace tubes_kpl_squarezoo.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string NoHP { get; set; }
        public UserRole Role { get; set; }
        public string? Password { get; set; }

        public User(string name, string noHP, UserRole role = UserRole.Pelapor, string? password = null)
        {
            UserId = Guid.NewGuid();
            Name = name;
            NoHP = noHP;
            Role = role;
            Password = password;
        }

        public bool CanPerform(string action)
        {
            return GetPermissions().Contains(action);
        }

        public List<string> GetPermissions()
        {
            return Role switch
            {
                UserRole.Admin => new List<string> { "ViewReports", "UpdateReportStatus", "CloseReport" },
                UserRole.Pimpinan => new List<string> { "ViewSummary" },
                _ => new List<string> { "CreateReport", "TrackReport" }
            };
        }
    }
}
