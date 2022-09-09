using System.Net;
using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Domain.Implementation.Parsers;
using Requests.Abstraction;

namespace Requests.Implementation;

public class PingRequest : Request
{
    private readonly Func<string, Parser> _parserFactory;
    public PingRequest(IShowMessage showMessage,
        IBrowser browser,
        Func<string, Parser> parserFactory) : base(showMessage, browser)
    {
        RequestAddress = "https://mrush.mobi/welcome";
        _parserFactory = parserFactory;
    }

    protected override string RequestString => "ping";

    protected override bool IsRequestFor(string input)
    {
        return RequestString == input;
    }

    protected override async Task<bool> InternalRequest()
    {
        var response = await Browser.Client.GetAsync(RequestAddress);
        var content = await response.Content.ReadAsStringAsync();

        var parser = _parserFactory("ping");
        parser.Initialize();

        Browser.SetLastResponse(response);

        var responseUri = Browser.GetResponseUriAsString();
        var parserResult = parser.Parse(responseUri);

        return await Task.FromResult(parserResult);
    }
}