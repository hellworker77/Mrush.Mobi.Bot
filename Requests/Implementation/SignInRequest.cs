using System.Net;
using Domain.Abstraction.Interfaces;
using Requests.Abstraction;

namespace Requests.Implementation;

public class SignInRequest : Request
{
    private readonly IDetourAntiBotSystem _detourAntiBotSystem;
    public SignInRequest(IShowMessage showMessage, 
        IBrowser browser,
        IDetourAntiBotSystem detourAntiBotSystem) : base(showMessage, browser)
    {
        RequestAddress = "https://mrush.mobi/login";
        _detourAntiBotSystem = detourAntiBotSystem;
    }

    protected override string RequestString => "signIn";

    protected override bool IsRequestFor(string input)
    {
        return RequestString == input;
    }
    protected override async Task<HttpStatusCode> InternalRequest()
    {
        var formVariables = new List<KeyValuePair<string, string>>();

        var login = RequestArgs.ElementAt(0);
        var password = RequestArgs.ElementAt(1);

        var lastResponse = Browser.Response;
        if (lastResponse == null)
        {
            ShowMessage.ShowError("Ping before signIn");
            return HttpStatusCode.BadRequest;
        }
        var content = await lastResponse.Content.ReadAsStringAsync();

        var dynamicKey = await _detourAntiBotSystem.FindDynamicKeyAsync(content);
        var staticKey = await _detourAntiBotSystem.FindStaticKeyAsync(dynamicKey);


        formVariables.Add(new KeyValuePair<string, string>("name", $"{login}"));
        formVariables.Add(new KeyValuePair<string, string>("password", $"{password}"));
        formVariables.Add(new KeyValuePair<string, string>($"{staticKey}", ""));

        var formContent = new FormUrlEncodedContent(formVariables);

        var response = await Browser.Client.PostAsync(RequestAddress, formContent);

        Browser.SetLastResponse(response);

        return response.StatusCode;
    }
}