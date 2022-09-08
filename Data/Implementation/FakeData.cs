using Domain.Models;
using Entities.Implementation;

namespace Data.Implementation;

public static class FakeData
{
    public static ICollection<AccountEntity> Accounts = new List<AccountEntity>
    {
        new AccountEntity
        {
            Login = "name",
            Password = "password",
            User = Users.First(),
        }
    };
    public static IEnumerable<UserEntity> Users => new List<UserEntity>
    {
        new UserEntity()
        {
            TelegramId = 885433065,
            Accounts = Accounts,
        }
    };
}