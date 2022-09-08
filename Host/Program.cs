using Commands.Abstraction;
using Commands.Implementation;
using Core.Abstraction.Interfaces;
using Core.Implementation;
using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Domain.Implementation;
using Domain.Implementation.Parsers;
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

        services.AddTransient<Command, PingCommand>();
        services.AddTransient<Command, UnKnownCommand>();
        services.AddTransient(Command.GetCommand);

        services.AddTransient<Request, PingRequest>();
        services.AddTransient<Request, UnKnownRequest>();
        services.AddTransient(Request.GetRequest);

        services.AddTransient<Parser, ParserMainPage>();
        services.AddTransient<Parser, UnKnownParser>();
        services.AddTransient(Parser.GetParser);

        return services;
    }
}