using System.Net;
using Domain.Abstraction.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Requests.Abstraction;

public abstract class Request
{
    protected bool IsTerminatingCommand;
    protected readonly IShowMessage ShowMessage;
    protected readonly IBrowser Browser;
    protected string RequestAddress;

    protected Request(IShowMessage showMessage,
        IBrowser browser)
    {
        IsTerminatingCommand = false;
        ShowMessage = showMessage;
        Browser = browser;
        RequestAddress = string.Empty;
        RequestArgs = new List<string>();
    }

    protected abstract string RequestString { get; }
    protected IEnumerable<string> RequestArgs { get; set; }
    protected virtual bool IsRequestFor(string input)
    {
        return RequestString.Contains(input.ToLower());
    }
    public async Task<bool> SendRequestAsync()
    {
        return await InternalRequest();
    }
    public virtual void ImportRequestArgs(string request, char separator)
    {
        var args = request.Split(separator).ToList();

        RequestArgs = args;
    }
    protected abstract Task<bool> InternalRequest();

    public static Func<IServiceProvider, Func<string, Request>> GetRequest =>
        provider => input =>
        {
            var command = provider.GetServices<Request>().First(c => c.IsRequestFor(input));

            return command;
        };
}