using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Newtonsoft.Json;
using Requests.Abstraction;

namespace Requests.Implementation;

public class ScheduleRequest : Request
{
    private readonly Func<string, Parser> _parserFactory;
    public ScheduleRequest(IShowMessage showMessage, 
        IWebDriver webDriver,
        Func<string, Parser> parserFactory) : base(showMessage, webDriver)
    {
        _parserFactory = parserFactory;
        RequestAddress = "https://mrush.mobi/schedule";
    }

    protected override string RequestString => "schedule";
    protected override async Task<string> InternalRequestExecute()
    {
        var response = await WebDriver.Client.GetAsync(RequestAddress);
        var content = await response.Content.ReadAsStringAsync();

        var parser = _parserFactory("schedule");
        parser.Initialize();

        WebDriver.SetLastResponse(response);

        var parserResult = parser.Parse(content);
        
        return await Task.FromResult(parserResult);
    }
}