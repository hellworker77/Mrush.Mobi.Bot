using AutoMapper;
using Commands.Abstraction;
using Commands.Implementation;
using Core.Abstraction.Interfaces;
using Core.Implementation;
using Data.Abstraction.Interfaces;
using Data.Implementation;
using Data.Repositories;
using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Domain.Implementation;
using Domain.Implementation.Parsers;
using Host.Abstraction.Interfaces;
using Host.Implementation;
using Mapping.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Requests.Abstraction;
using Requests.Implementation;
using Timer = Domain.Implementation.Timer;

namespace Host;
class Program
{
    static async Task Main(string[] args)
    {
        var services = ConfigureServices();

        var serviceProvider = services.BuildServiceProvider();

        var dbInitializer = serviceProvider.GetService<IDBInitializer>();

        if (dbInitializer != null)
        {
            dbInitializer.Initialize();
        }

        var runtimeService = serviceProvider.GetService<IRuntime>();

        if (runtimeService != null)
        {
            await runtimeService.InitializeAsync();
        }
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IRuntime, Runtime>();
        services.AddScoped<IShowMessage, ShowMessage>();
        services.AddSingleton<ITimer, Timer>();
        services.AddSingleton<IBrowser, Browser>();
        services.AddSingleton<IDBInitializer, DBInitializer>();

        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IUserService, UserService>();

        services.AddTransient<IDetourAntiBotSystem, DetourAntiBotSystem>();

        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=HellworkerBotApplicationDB;Trusted_Connection=True;");
            options.UseLazyLoadingProxies();
        });


        var mapperConfiguration = new MapperConfiguration(options =>
        {
            options.AddProfile(new AccountMapper());
            options.AddProfile(new UserMapper());
        });

        var mapper = mapperConfiguration.CreateMapper();
        services.AddSingleton(mapper);

        services.AddTransient<Command, ShutdownCommand>();
        services.AddTransient<Command, AddAccountCommand>();
        services.AddTransient<Command, AddUserCommand>();
        services.AddTransient<Command, SignInCommand>();
        services.AddTransient<Command, PingCommand>();
        services.AddTransient<Command, UnKnownCommand>();
        services.AddTransient(Command.GetCommand);

        services.AddTransient<Request, SignInRequest>();
        services.AddTransient<Request, PingRequest>();
        services.AddTransient<Request, UnKnownRequest>();
        services.AddTransient(Request.GetRequest);

        services.AddTransient<Parser, ParserPing>();
        services.AddTransient<Parser, UnKnownParser>();
        services.AddTransient(Parser.GetParser);

        return services;
    }
}