using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using tubes_kpl_squarezoo;

namespace UserPerformence
{
    [MemoryDiagnoser]
    public class UserPerformence
    {
        public User user;

        [IterationSetup]
        public void Setup()
        {
            user = new User(
                Guid.NewGuid(),
                "Rafael",
                "08123456789"
            );

            user.AddPermission("CreateReport");
            user.AddPermission("ViewReport");
        }

        [Benchmark]
        public bool CanPerformBenchmark()
        {
            return user.CanPerform("CreateReport");
        }

        [Benchmark]
        public void AddPermissionBenchmark()
        {
            user.AddPermission("NewPermission");
        }

        [Benchmark]
        public List<string> GetPermissionsBenchmark()
        {
            return user.GetPermissions();
        }
    }
}