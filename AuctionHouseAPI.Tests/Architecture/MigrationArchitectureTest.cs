using AuctionHouseAPI.Migrations;
using NetArchTest.Rules;

namespace AuctionHouseAPI.Tests.Architecture
{
    [TestFixture]
    public class MigrationArchitectureTest
    {
        [Test]
        public void MigrationShouldDependOnSharedLayer()
        {
            var types = Types.InAssembly(typeof(MigrationHostedService).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Shared")
                .GetTypes();

            Assert.IsTrue(result.Any());
        }
        [Test]
        public void MigrationShouldNotDependOnApplicationLayer()
        {
            var types = Types.InAssembly(typeof(MigrationHostedService).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Application")
                .GetTypes();

            Assert.IsFalse(result.Any());
        }
        [Test]
        public void MigrationShouldNotDependOnDomainLayer()
        {
            var types = Types.InAssembly(typeof(MigrationHostedService).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Domain")
                .GetTypes();

            Assert.IsFalse(result.Any());
        }
        [Test]
        public void MigrationShouldNotDependOnPresentationLayer()
        {
            var types = Types.InAssembly(typeof(MigrationHostedService).Assembly);
            var result = types
                .That()
                .HaveDependencyOn("AuctionHouseAPI.Presentation")
                .GetTypes();

            Assert.IsFalse(result.Any());
        }
    }
}
