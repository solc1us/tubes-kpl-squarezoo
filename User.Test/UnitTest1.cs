using System;
using Xunit;

namespace UserTest
{
    public class UnitTest1
    {
        [Fact]
        public void CanPerform_ReturnsTrue_WhenPermissionExists()
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
    }
}