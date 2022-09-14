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
using Services.Abstraction;
using Services.Implementation;
using Services.Implementation.Domain;
using Services.Implementation.Domain.Keyboards;
using Services.Intefraces;
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

        var clientService = serviceProvider.GetService<IClient>();

        if (clientService != null)
        {
            clientService.Initialize();
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
        services.AddSingleton<IWebDriver, WebDriver>();
        services.AddSingleton<IDBInitializer, DBInitializer>();
        services.AddSingleton<IClient, Client>();
        services.AddScoped<ITelegramDataParser, TelegramDataParser>();


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

        AddCommandsToServices(services);

        AddRequestsToServices(services);

        AddParsersToServices(services);

        AddKeyboardsToServices(services);

        return services;
    }

    private static void AddCommandsToServices(ServiceCollection services)
    {
        services.AddTransient<Command, ArenaCommand>();
        services.AddTransient<Command, GetUserCommand>();
        services.AddTransient<Command, LairCommand>();
        services.AddTransient<Command, ClearCommand>();
        services.AddTransient<Command, LogoutCommand>();
        services.AddTransient<Command, ScheduleCommand>();
        services.AddTransient<Command, ShutdownCommand>();
        services.AddTransient<Command, AddAccountCommand>();
        services.AddTransient<Command, AddUserCommand>();
        services.AddTransient<Command, SignInCommand>();
        services.AddTransient<Command, PingCommand>();
        services.AddTransient<Command, UnKnownCommand>();
        services.AddTransient(Command.GetCommand);
    }

    private static void AddRequestsToServices(ServiceCollection services)
    {
        services.AddTransient<Request, ArenaRequest>();
        services.AddTransient<Request, PingRequest>();
        services.AddTransient<Request, LairRequest>();
        services.AddTransient<Request, LogoutRequest>();
        services.AddTransient<Request, ScheduleRequest>();
        services.AddTransient<Request, SignInRequest>();
        services.AddTransient<Request, UnKnownRequest>();
        services.AddTransient(Request.GetRequest);
    }
    private static void AddParsersToServices(ServiceCollection services)
    {
        services.AddTransient<Parser, ArenaParser>();
        services.AddTransient<Parser, LairParser>();
        services.AddTransient<Parser, ParserSchedule>();
        services.AddTransient<Parser, ParserPing>();
        services.AddTransient<Parser, UnKnownParser>();
        services.AddTransient(Parser.GetParser);
    }

    private static void AddKeyboardsToServices(ServiceCollection services)
    {
        services.AddTransient<KeyboardMarkup, KeyboardMarkupOptions>();
        services.AddTransient<KeyboardMarkup, KeyboardMarkupSignIn>();
        services.AddTransient(KeyboardMarkup.GetKeyboardMarkup);
    }
}