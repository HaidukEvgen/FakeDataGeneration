using Bogus;
using FakeDataGeneration.Server.Models;

namespace FakeDataGeneration.Server.Services
{
    public interface IErrorsService
    {
        FakeUser ApplyErrors(Faker faker, FakeUser user, double errorAmount);
    }
}