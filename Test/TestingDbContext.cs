using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.Helpers;
using Test.Setup;

namespace Test
{
    public class TestingDbContext
    {
        private readonly DbContextWrite _dbContextWrite;
        private readonly DbContextRead _dbContextRead;
        private CancellationToken _cancellationToken;

        public TestingDbContext()
        {
            var mockDbContext = new MockDbContext();

            _dbContextWrite = mockDbContext.DbContextWrite;
            _dbContextRead = mockDbContext.DbContextRead;
            _cancellationToken = new CancellationToken();
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task AddValues_ReadValues()
        {
            var gateways = new List<Gateway>();

            for (int i = 10; i < 100; i++)
            {
                var gateway = new Gateway($"SN{i}", $"RN{i}", $"1.1.1.{i}");
                gateway.AddPeripheralDevice(MockSequentialGuid.NewGuid(), $"V{i}", PeripheralDeviceStatusValues.Online);
                gateway.AddPeripheralDevice(MockSequentialGuid.NewGuid(), $"V{i + 1}", PeripheralDeviceStatusValues.Offline);
                gateways.Add(gateway);
            }
            await _dbContextWrite.AddGatewayAsync(gateways.ToArray(), _cancellationToken);

            var any = await _dbContextRead.Set<Gateway>().AnyAsync(_cancellationToken);
            var total = await _dbContextRead.Set<Gateway>().CountAsync(_cancellationToken);
            Assert.IsTrue(any);
            Assert.IsTrue(total == gateways.Count);
        }
    }
}