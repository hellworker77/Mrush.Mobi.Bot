using Entities.Abstraction;

namespace Entities.Implementation;

public class UserEntity : BaseEntity
{
    public long TelegramId { get; set; }
    public virtual ICollection<AccountEntity> Accounts { get; set; }

}