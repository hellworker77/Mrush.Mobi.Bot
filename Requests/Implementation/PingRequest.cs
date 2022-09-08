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

    protected override string RequestString => string.Empty;

    protected override bool IsRequestFor(string input)
    {
        return true;
    }

    protected override async Task<HttpStatusCode> InternalRequest()
    {
        var response = await Browser.Client.GetAsync(RequestAddress);
        var content = await response.Content.ReadAsStringAsync();

        var parser = _parserFactory("ping");
        parser.Initialize();
        var parserResult = parser.Parse(content);
        if (parserResult == true)
        {
            ShowMessage.ShowInfo("You are authorized");
        }
        else
        {
            ShowMessage.ShowInfo("You are not authorized");
        }
        

        return await Task.FromResult(response.StatusCode);
    }
}