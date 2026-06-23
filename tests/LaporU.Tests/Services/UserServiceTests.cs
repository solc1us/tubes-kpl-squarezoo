using LaporU.Tests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Services;

namespace LaporU.Tests.Services
{
    public class UserServiceTests : IDisposable
    {
        private readonly string _filePath;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _filePath = TestDataFactory.CreateTempJsonPath("users.json");
            _service = new UserService(_filePath);
        }

        public void Dispose()
        {
            TestDataFactory.DeleteTempDirectoryForFile(_filePath);
        }

        [Fact]
        public void Login_WithValidAdminCredentials_ShouldReturnAdminUser()
        {
            var admin = _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");

            var result = _service.Login("081111111111", "admin-password");

            Assert.NotNull(result);
            Assert.Equal(admin.UserId, result.UserId);
            Assert.Equal(UserRole.Admin, result.Role);
        }

        [Fact]
        public void Login_WithValidPimpinanCredentials_ShouldReturnPimpinanUser()
        {
            var pimpinan = _service.CreateUser("Pimpinan Kampus", "082222222222", UserRole.Pimpinan, "pimpinan-password");

            var result = _service.Login("082222222222", "pimpinan-password");

            Assert.NotNull(result);
            Assert.Equal(pimpinan.UserId, result.UserId);
            Assert.Equal(UserRole.Pimpinan, result.Role);
        }

        [Fact]
        public void Login_WithInvalidNoHP_ShouldFail()
        {
            _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");

            var result = _service.Login("089999999999", "admin-password");

            Assert.Null(result);
        }

        [Fact]
        public void Login_WithInvalidPassword_ShouldFail()
        {
            _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");

            var result = _service.Login("081111111111", "wrong-password");

            Assert.Null(result);
        }

        [Fact]
        public void GetAllUsers_WithExistingUsers_ShouldReturnUsers()
        {
            var admin = _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");
            var pimpinan = _service.CreateUser("Pimpinan Kampus", "082222222222", UserRole.Pimpinan, "pimpinan-password");

            var users = _service.GetAll();

            Assert.Contains(admin, users);
            Assert.Contains(pimpinan, users);
        }

        [Fact]
        public void GetUserById_WithExistingUser_ShouldReturnUser()
        {
            var admin = _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");

            var result = _service.GetById(admin.UserId);

            Assert.NotNull(result);
            Assert.Equal(admin.UserId, result.UserId);
        }

        [Fact]
        public void GetUserById_WithMissingUser_ShouldReturnNullOrExpectedFailure()
        {
            var result = _service.GetById(Guid.NewGuid());

            Assert.Null(result);
        }
    }
}
