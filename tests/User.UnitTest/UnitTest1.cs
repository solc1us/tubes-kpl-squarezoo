using Xunit;
using System;
using tubes_kpl_squarezoo;

namespace UserTest
{
    public class Test1
    {
        [Fact]
        public void TestAddPermission()
        {
            User user = new User(
                Guid.NewGuid(),
                "Rafael",
                "08123456789"
            );

            user.AddPermission("CreateReport");

            Assert.True(user.CanPerform("CreateReport"));
        }

        [Fact]
        public void TestCanPerformTrue()
        {
            User user = new User(
                Guid.NewGuid(),
                "Rafael",
                "08123456789"
            );

            user.AddPermission("CreateReport");

            bool result = user.CanPerform("CreateReport");

            Assert.True(result);
        }

        [Fact]
        public void TestCanPerformFalse()
        {
            User user = new User(
                Guid.NewGuid(),
                "Rafael",
                "08123456789"
            );

            bool result = user.CanPerform("DeleteReport");

            Assert.False(result);
        }

        [Fact]
        public void TestGetPermissions()
        {
            User user = new User(
                Guid.NewGuid(),
                "Rafael",
                "08123456789"
            );

            user.AddPermission("CreateReport");
            user.AddPermission("ViewReport");

            var permissions = user.GetPermissions();

            Assert.Equal(2, permissions.Count);
        }
    }
}