using BenchmarkDotNet.Attributes;
using LaporU.PerformanceTests.Helpers;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;

namespace LaporU.PerformanceTests.Services
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class UserServiceBenchmarks
    {
        private UserService _userService = null!;
        private User _admin = null!;
        private User _pimpinan = null!;
        private Guid _existingUserId;
        private string? _filePath;

        [GlobalSetup]
        public void Setup()
        {
            _userService = BenchmarkTestDataFactory.CreateSeededUserService(1000, out _filePath);
            _admin = BenchmarkTestDataFactory.CreateSampleAdminUser();
            _pimpinan = BenchmarkTestDataFactory.CreateSamplePimpinanUser();
            _existingUserId = _userService.GetAll()[500].UserId;
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            BenchmarkTestDataFactory.DeleteTempDirectoryForFile(_filePath);
        }

        [Benchmark]
        public User? LoginValidAdmin() =>
            _userService.Login(_admin.NoHP, _admin.Password!);

        [Benchmark]
        public User? LoginValidPimpinan() =>
            _userService.Login(_pimpinan.NoHP, _pimpinan.Password!);

        [Benchmark]
        public User? LoginInvalidUser() =>
            _userService.Login("089999999999", "wrong-password");

        [Benchmark]
        public List<User> GetAllUsers() =>
            _userService.GetAll();

        [Benchmark]
        public User? GetUserById() =>
            _userService.GetById(_existingUserId);
    }
}
