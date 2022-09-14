using System.Net;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Requests.Implementation;

public class UnKnownRequest : Request
{
    public UnKnownRequest(IShowMessage showMessage, IWebDriver webDriver) : base(showMessage, webDriver)
    {
    }

    protected override string RequestString => string.Empty;

    protected override bool IsRequestFor(string input)
    {
        return true;
    }

    protected override async Task<string> InternalRequestExecute()
    {
        var result = string.Empty;

        ShowMessage.ShowError("Not found request");

        return await Task.FromResult(result);
    }
}