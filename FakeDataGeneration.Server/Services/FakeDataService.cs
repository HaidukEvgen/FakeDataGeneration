using FakeDataGeneration.Server.Models;
using Bogus;
using Bogus.DataSets;

namespace FakeDataGeneration.Server.Services
{

    public class FakeDataService : IFakeDataService
    {
        private Faker _faker;
        private const int MinApartmentNumber = 1;
        private const int MaxApartmentNumber = 999;
        private readonly IErrorsService _errorsService;

        public FakeDataService(IErrorsService errorsService)
        {
            _errorsService = errorsService;
        }

        public List<FakeUser> GetFakeUsers(FakeDataRequest request)
        {
            Randomizer.Seed = new Random(request.Seed + request.PageNumber);

            var locale = GetLocaleFromRegion(request.Region);
           
            _faker = new Faker(locale);
            _faker.Lorem.Locale = locale;

            var users = new List<FakeUser>(request.PerPage);

            for (int i = 0; i < request.PerPage; i++)
            {
                var user = GenerateFakeUser(request, i);

                user = _errorsService.ApplyErrors(_faker, user, request.ErrorsAmount);

                users.Add(user);
            }

            return users;
        }

        private FakeUser GenerateFakeUser(FakeDataRequest request, int currentIndex)
        {
            return new FakeUser()
            {
                Index = (request.PageNumber * request.PerPage) + currentIndex + 1,
                Id = GenerateFakeUserId(),
                Name = GenerateFakeUserName(),
                Address = GenerateFakeUserAddress(),
                PhoneNumber = GenerateFakeUserPhoneNumber()
            };
        }

        private string GenerateFakeUserId()
        {
            return _faker.Random.Guid().ToString();
        }

        private string GenerateFakeUserName()
        {
            return _faker.Name.FullName();
        }

        private string GenerateFakeUserAddress()
        {
            var addressFormats = new List<Func<string>>()
            {
                () => $"{_faker.Address.StreetName()}, " +
                    $"{_faker.Address.BuildingNumber()} - " +
                    $"{_faker.Random.Int(MinApartmentNumber, MaxApartmentNumber)}",

                () => $"{_faker.Address.City()}, " +
                    $"{_faker.Address.StreetName()}, " +
                    $"{_faker.Address.BuildingNumber()} - " +
                    $"{_faker.Random.Int(MinApartmentNumber, MaxApartmentNumber)}",

                () => $"{_faker.Address.State()}, " +
                    $"{_faker.Address.City()}, " +
                    $"{_faker.Address.StreetName()}, " +
                    $"{_faker.Address.BuildingNumber()}"
            };

            var randomIndex = _faker.Random.Number(addressFormats.Count - 1);
            return addressFormats[randomIndex]();
        }

        private string GenerateFakeUserPhoneNumber()
        {
            var phoneFormats = new List<Func<string>>()
            {
                () => _faker.Phone.PhoneNumber(),
                () => _faker.Phone.PhoneNumberFormat()
            };

            var randomIndex = _faker.Random.Number(phoneFormats.Count - 1);
            return phoneFormats[randomIndex]();
        }

        private string GetLocaleFromRegion(Region region)
        {
            return region switch
            {
                Region.Russia => "ru",
                Region.America => "en",
                Region.Poland => "pl",
                _ => "en",
            };
        }
    }
}
