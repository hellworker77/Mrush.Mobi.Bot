using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Requests.Implementation;

public class LairRequest : Request
{
    private readonly Func<string, Parser> _parserFactory;
    public LairRequest(IShowMessage showMessage,
        IWebDriver webDriver,
        Func<string, Parser> parserFactory) : base(showMessage, webDriver)
    {
        RequestAddress = "https://mrush.mobi/lair?action=attack&r=342&hash=0";
        _parserFactory = parserFactory;
    }

    protected override string RequestString => "lair";
    protected override bool IsRequestFor(string input)
    {
        return RequestString == input;
    }
    protected override async Task<string> InternalRequestExecute()
    {
        var response = await WebDriver.Client.GetAsync(RequestAddress);
        var content = await response.Content.ReadAsStringAsync();

        var parser = _parserFactory("lair");
        parser.Initialize();

        WebDriver.SetLastResponse(response);

        var parserResult = parser.Parse(content);


        return await Task.FromResult(parserResult);
    }
}