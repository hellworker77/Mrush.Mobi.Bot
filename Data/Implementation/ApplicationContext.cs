using Data.Configurations;
using Entities.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementation;

public class ApplicationContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<AccountEntity> Accounts { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> contextOptionsBuilder)
        : base(contextOptionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new AccountConfiguration());

    }
}