using Bogus;
using FakeDataGeneration.Server.Models;

namespace FakeDataGeneration.Server.Services
{

    public class ErrorsService : IErrorsService
    {
        private Faker _faker;
        private const int MinFieldLength = 3;
        private const int MaxFieldLength = 65;
        private const int MaxNumberLength = 15;


        public FakeUser ApplyErrors(Faker faker, FakeUser user, double errorAmount)
        {
            _faker = faker;
            var errorCount = (int)Math.Floor(errorAmount);
            var remainingProbability = errorAmount - errorCount;

            for (int i = 0; i < errorCount; i++)
            {
                PrepareError(user);
            }

            if (_faker.Random.Double() < remainingProbability)
            {
                PrepareError(user);
            }

            return user;
        }

        private void PrepareError(FakeUser user)
        {
            var errorType = GetRandomErrorType();
            var field = GetRandomField();
            ApplyError(user, errorType, field);
        }

        private ErrorType GetRandomErrorType()
        {
            return _faker.Random.Enum<ErrorType>();
        }

        private FieldType GetRandomField()
        {
            return _faker.Random.Enum<FieldType>();
        }

        private FakeUser ApplyError(FakeUser user, ErrorType errorType, FieldType fieldType)
        {
            switch (errorType)
            {
                case ErrorType.DeleteCharacter:
                    ApplyDeleteCharacterError(user, fieldType);
                    break;
                case ErrorType.SwapCharacters:
                    ApplySwapCharactersError(user, fieldType);
                    break;
                case ErrorType.AddCharacter:
                    ApplyAddCharacterError(user, fieldType);
                    break;
            }

            return user;
        }

        private void ApplyDeleteCharacterError(FakeUser user, FieldType fieldType)
        {
            var fieldInfo = typeof(FakeUser).GetProperty(fieldType.ToString());
            var fieldValue = (string)fieldInfo.GetValue(user);
            if (fieldValue.Length <= MinFieldLength)
            {
                return;
            }
            var randomIndex = _faker.Random.Number(fieldValue.Length - 1);
            fieldValue = fieldValue.Remove(randomIndex, 1);
            fieldInfo.SetValue(user, fieldValue);
        }

        private void ApplySwapCharactersError(FakeUser user, FieldType fieldType)
        {
            var fieldInfo = typeof(FakeUser).GetProperty(fieldType.ToString());
            var fieldValue = (string)fieldInfo.GetValue(user);

            var randomIndex = _faker.Random.Number(fieldValue.Length - 2);
            var character1 = fieldValue[randomIndex];
            var character2 = fieldValue[randomIndex + 1];

            var modifiedName = fieldValue.ToCharArray();
            modifiedName[randomIndex] = character2;
            modifiedName[randomIndex + 1] = character1;

            fieldInfo.SetValue(user, new string(modifiedName));
        }

        private void ApplyAddCharacterError(FakeUser user, FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Name:
                    user.Name = AddAlphabeticCharacter(user.Name);
                    break;
                case FieldType.Address:
                    user.Address = AddAlphabeticCharacter(user.Address);
                    break;
                case FieldType.PhoneNumber:
                    user.PhoneNumber = AddNumericCharacter(user.PhoneNumber);
                    break;
            }
        }

        private string AddAlphabeticCharacter(string field)
        {
            if (field.Length >= MaxFieldLength)
            {
                return field;
            }
            var randomWord = _faker.Lorem.Word().ToString();
            var randomIndexForWord = _faker.Random.Number(randomWord.Length - 1);
            var randomCharacter = randomWord[randomIndexForWord];
            var randomIndexForField = _faker.Random.Number(field.Length - 1);
            return field.Insert(randomIndexForField, randomCharacter.ToString());
        }

        private string AddNumericCharacter(string field)
        {
            if (field.Length >= MaxNumberLength)
            {
                return field;
            }
            var randomNumber = _faker.Random.Number(0, 9).ToString();
            var randomIndexForField = _faker.Random.Number(1, field.Length - 1);
            return field.Insert(randomIndexForField, randomNumber);
        }
    }
}
