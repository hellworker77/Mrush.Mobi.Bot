using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Requests.Implementation;

public class LogoutRequest : Request
{
    private readonly Func<string, Parser> _parserFactory;
    public LogoutRequest(IShowMessage showMessage,
        IWebDriver webDriver,
        Func<string, Parser> parserFactory) : base(showMessage, webDriver)
    {
        RequestAddress = "https://mrush.mobi/logout";
        _parserFactory = parserFactory;
    }
    protected override bool IsRequestFor(string input)
    {
        return RequestString == input;
    }
    protected override string RequestString => "logout";
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