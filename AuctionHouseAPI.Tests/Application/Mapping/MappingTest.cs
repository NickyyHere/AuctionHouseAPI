using AuctionHouseAPI.Application.MappingProfiles;
using AutoMapper;

namespace AuctionHouseAPI.Tests.Application.Mapping
{
    [TestFixture]
    public class MappingTest
    {
        [TestCase(typeof(AuctionMappingProfile))]
        [TestCase(typeof(CategoryMappingProfile))]
        [TestCase(typeof(BidMappingProfile))]
        [TestCase(typeof(UserMappingProfile))]
        public void AutoMapperProfilesShouldHaveValidConfiguration(Type profileType)
        {
            var configuration = new MapperConfiguration(cfg => 
                cfg.AddProfile((Profile)Activator.CreateInstance(profileType)!));
            configuration.AssertConfigurationIsValid();
        }
    }
}
