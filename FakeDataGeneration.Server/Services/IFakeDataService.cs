using FakeDataGeneration.Server.Models;

namespace FakeDataGeneration.Server.Services
{
    public interface IFakeDataService
    {
        public List<FakeUser> GetFakeUsers(FakeDataRequest request);
    }
}