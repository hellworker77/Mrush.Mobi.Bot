using System.Net;
using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Domain.Implementation.Parsers;
using Newtonsoft.Json;
using Requests.Abstraction;

namespace Requests.Implementation;

public class PingRequest : Request
{
    private readonly Func<string, Parser> _parserFactory;
    public PingRequest(IShowMessage showMessage,
        IWebDriver webDriver,
        Func<string, Parser> parserFactory) : base(showMessage, webDriver)
    {
        RequestAddress = "https://mrush.mobi/welcome";
        _parserFactory = parserFactory;
    }

    protected override string RequestString => "ping";

    protected override bool IsRequestFor(string input)
    {
        return RequestString == input;
    }

    protected override async Task<string> InternalRequestExecute()
    {
        var response = await WebDriver.Client.GetAsync(RequestAddress);

        var parser = _parserFactory("ping");
        parser.Initialize();

        WebDriver.SetLastResponse(response);

        var responseUri = WebDriver.GetResponseUriAsString();
        var parserResult = parser.Parse(responseUri);

        return await Task.FromResult(parserResult);
    }
}