using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Requests.Implementation;

public class ArenaRequest : Request
{
    private readonly Func<string, Parser> _parserFactory;
    public ArenaRequest(IShowMessage showMessage,
        IWebDriver webDriver,
        Func<string, Parser> parserFactory) : base(showMessage, webDriver)
    {
        RequestAddress = "https://mrush.mobi/arena?id=1&r=112&hash=1097";
        _parserFactory = parserFactory;
    }

    protected override string RequestString => "arena";
    protected override bool IsRequestFor(string input)
    {
        return RequestString == input;
    }
    protected override async Task<string> InternalRequestExecute()
    {
        var response = await WebDriver.Client.GetAsync(RequestAddress);
        var content = await response.Content.ReadAsStringAsync();

        var parser = _parserFactory("arena");
        parser.Initialize();

        WebDriver.SetLastResponse(response);

        var parserResult = parser.Parse(content);

        return await Task.FromResult(parserResult);
    }
}