using Entities.Abstraction;

namespace Entities.Implementation;

public class AccountEntity : BaseEntity
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string GoldString { get; set; }
    public string SilverString { get; set; }
    public string LevelString { get; set; }
    public string ValorString { get; set; }
    public string GiftsString { get; set; }
    public string AchievementsString { get; set; }
    public string EquipString { get; set; }
    public string BagString { get; set; }
    public string TrophiesString { get; set; }
    public string ClanString { get; set; }
    public string TrainingString { get; set; }
    public string SkillsString { get; set; }
    public virtual UserEntity User { get; set; }
    public Guid UserId { get; set; }
}