using Entities.Abstraction;

namespace Entities.Implementation;

public class AccountEntity : BaseEntity
{
    public string Login { get; set; }
    public string Password { get; set; }
    public virtual UserEntity User { get; set; }
    public Guid UserId { get; set; }
}