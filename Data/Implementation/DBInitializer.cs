using Data.Abstraction.Interfaces;
using Domain.Abstraction.Interfaces;

namespace Data.Implementation;

public class DBInitializer : IDBInitializer
{
    private ApplicationContext _context;
    private readonly IShowMessage _showMessage;
    public DBInitializer(ApplicationContext context, 
        IShowMessage showMessage)
    {
        _context = context;
        _showMessage = showMessage;
    }
    public void Initialize()
    { 
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();


        _context.Accounts.AddRange(FakeData.Accounts);
        _context.SaveChanges();

        _context.Users.AddRange(FakeData.Users);
        _context.SaveChanges();

        _showMessage.ShowSuccessful("Database initialized");
    }
}