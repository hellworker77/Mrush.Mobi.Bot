using System.Net;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Requests.Implementation;

public class UnKnownRequest : Request
{
    public UnKnownRequest(IShowMessage showMessage, IBrowser browser) : base(showMessage, browser)
    {
    }

    protected override string RequestString => string.Empty;

    protected override bool IsRequestFor(string input)
    {
        return true;
    }

    protected override async Task<HttpStatusCode> InternalRequest()
    {
        var result = HttpStatusCode.NotImplemented;

        ShowMessage.ShowError("Not found request");

        return await Task.FromResult(result);
    }
}