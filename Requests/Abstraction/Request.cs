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
    }

    protected abstract string RequestString { get; }
    protected virtual bool IsRequestFor(string input)
    {
        return RequestString.Contains(input.ToLower());
    }
    public async Task<HttpStatusCode> SendRequest()
    {
        return await InternalRequest();
    }

    protected abstract Task<HttpStatusCode> InternalRequest();

    public static Func<IServiceProvider, Func<string, Request>> GetRequest =>
        provider => input =>
        {
            var command = provider.GetServices<Request>().First(c => c.IsRequestFor(input));

            return command;
        };
}