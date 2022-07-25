using DoItFast.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Test.Helpers
{
    public class MockDbContext : IDisposable
    {
        public MockDbContext()
        {
            var root = new InMemoryDatabaseRoot();
            var optionsWrite = new DbContextOptionsBuilder<DbContextWrite>()
                .UseInMemoryDatabase("DoItFastDb", root)
                .Options;
            var optionsRead = new DbContextOptionsBuilder<DbContextRead>()
                .UseInMemoryDatabase("DoItFastDb", root)
                .Options;

            DbContextWrite = new DbContextWrite(optionsWrite);
            DbContextRead = new DbContextRead(optionsRead);
        }

        public DbContextWrite DbContextWrite { get; }
        public DbContextRead DbContextRead { get; }

        public void Dispose()
        {
            DbContextWrite.Dispose();
            DbContextRead.Dispose();
        }
    }
}
