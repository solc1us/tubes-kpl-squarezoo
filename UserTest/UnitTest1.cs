using System;
using Xunit;

public class UnitTest1
{
    [Fact]
    public void TestCanPerform_User()
    {
        User user = new User(
            Guid.NewGuid(),
            "Rafael",
            "08123456789",
            "User"
        );

        bool result = user.CanPerform("Report:Create");

        Assert.True(result);
    }

    [Fact]
    public void TestCanPerform_Admin()
    {
        User admin = new User(
            Guid.NewGuid(),
            "Admin",
            "08111111111",
            "Admin"
        );

        bool result = admin.CanPerform("Report:Close");

        Assert.True(result);
    }

    [Fact]
    public void TestGetPermissions()
    {
        User user = new User(
            Guid.NewGuid(),
            "Rafael",
            "08123456789",
            "User"
        );

        var permissions = user.GetPermissions();

        Assert.NotNull(permissions);
        Assert.True(permissions.Count > 0);
    }
}