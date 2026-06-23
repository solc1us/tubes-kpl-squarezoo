using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace LaporU.Tests.Models
{
    public class UserTests
    {
        [Fact]
        public void User_WithValidData_ShouldStoreUserProperties()
        {
            var user = new User("Admin Satgas", "081111111111", UserRole.Admin, "password");

            Assert.Equal("Admin Satgas", user.Name);
            Assert.Equal("081111111111", user.NoHP);
            Assert.Equal(UserRole.Admin, user.Role);
        }

        [Fact]
        public void User_WhenCreated_ShouldContainUserId()
        {
            var user = new User("Admin Satgas", "081111111111", UserRole.Admin);

            Assert.NotEqual(Guid.Empty, user.UserId);
        }

        [Fact]
        public void User_RoleAdmin_ShouldStoreCorrectRole()
        {
            var user = new User("Admin Satgas", "081111111111", UserRole.Admin);

            Assert.Equal(UserRole.Admin, user.Role);
        }

        [Fact]
        public void User_RolePimpinan_ShouldStoreCorrectRole()
        {
            var user = new User("Pimpinan Kampus", "082222222222", UserRole.Pimpinan);

            Assert.Equal(UserRole.Pimpinan, user.Role);
        }

        [Fact]
        public void User_ShouldStorePermissions()
        {
            var user = new User("Admin Satgas", "081111111111", UserRole.Admin);

            var permissions = user.GetPermissions();

            Assert.NotNull(permissions);
            Assert.Contains("ViewReports", permissions);
            Assert.Contains("UpdateReportStatus", permissions);
            Assert.Contains("CloseReport", permissions);
        }
    }
}
