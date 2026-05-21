using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace UserPerformence
{
    [MemoryDiagnoser]
    [SimpleJob]
    public class UserPerformence
    {
        private User user;

        [GlobalSetup]
        public void Setup()
        {
            user = new User(
                Guid.NewGuid(),
                "Rafael",
                "08123456789",
                "User"
            );
        }

        [Benchmark]
        public bool CanPerformBenchmark()
        {
            bool result = false;

            for (int i = 0; i < 10000; i++)
            {
                result = user.CanPerform("Report:Create");
            }

            return result;
        }

        [Benchmark]
        public List<string> GetPermissionsBenchmark()
        {
            List<string> permissions = null;

            for (int i = 0; i < 10000; i++)
            {
                permissions = user.GetPermissions();
            }

            return permissions;
        }
    }
}