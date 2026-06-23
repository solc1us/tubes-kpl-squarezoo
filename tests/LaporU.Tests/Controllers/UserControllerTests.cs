using LaporU.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using tubes_kpl_squarezoo.Controllers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Services;

namespace LaporU.Tests.Controllers
{
    public class UserControllerTests : IDisposable
    {
        private readonly string _filePath;
        private readonly UserService _service;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _filePath = TestDataFactory.CreateTempJsonPath("users.json");
            _service = new UserService(_filePath);
            _controller = new UserController(_service);
        }

        public void Dispose()
        {
            TestDataFactory.DeleteTempDirectoryForFile(_filePath);
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldReturnOkResult()
        {
            _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");
            var request = new LoginUserRequest
            {
                NoHP = "081111111111",
                Password = "admin-password"
            };

            var result = _controller.Login(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<UserResponse>(ok.Value);
            Assert.Equal(UserRole.Admin, response.Role);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldReturnUnauthorizedOrBadRequest()
        {
            _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");
            var request = new LoginUserRequest
            {
                NoHP = "081111111111",
                Password = "wrong-password"
            };

            var result = _controller.Login(request);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_Response_ShouldNotExposePassword()
        {
            _service.CreateUser("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");
            var request = new LoginUserRequest
            {
                NoHP = "081111111111",
                Password = "admin-password"
            };

            var result = _controller.Login(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<UserResponse>(ok.Value);
            Assert.Null(response.GetType().GetProperty("Password"));
        }
    }
}
