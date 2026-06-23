using BenchmarkDotNet.Attributes;
using LaporU.PerformanceTests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace LaporU.PerformanceTests.Models
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class UserModelBenchmarks
    {
        [Benchmark]
        public User CreateUserObject() =>
            new("User Benchmark", "081111110000", UserRole.Admin, "password");

        [Benchmark]
        public List<string> AssignUserRole()
        {
            var user = new User("User Benchmark", "081111110000", UserRole.Pimpinan, "password");

            return user.GetPermissions();
        }

        [Benchmark]
        public List<User> CreateManyUsers() =>
            BenchmarkTestDataFactory.CreateManyUsers(100);
    }
}
