using System.Net;
using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Newtonsoft.Json;
using Requests.Abstraction;

namespace Requests.Implementation;

public class SignInRequest : Request
{
    private readonly IDetourAntiBotSystem _detourAntiBotSystem;
    private readonly Func<string, Parser> _parserFactory;
    public SignInRequest(IShowMessage showMessage, 
        IWebDriver webDriver,
        IDetourAntiBotSystem detourAntiBotSystem,
        Func<string, Parser> parserFactory) : base(showMessage, webDriver)
    {
        RequestAddress = "https://mrush.mobi/login";
        _detourAntiBotSystem = detourAntiBotSystem;
        _parserFactory = parserFactory;
    }

    protected override string RequestString => "signIn";

    protected override bool IsRequestFor(string input)
    {
        return RequestString == input;
    }
    protected override async Task<string> InternalRequestExecute()
    {
        var login = RequestArgs.ElementAt(0);
        var password = RequestArgs.ElementAt(1);

        var lastResponse = WebDriver.Response;
        if (lastResponse == null)
        {
            return string.Empty;
        }

        var content = await lastResponse.Content.ReadAsStringAsync();

        var externalKey = await _detourAntiBotSystem.FindExternalKeyAsync(content);
        var internalKey = await _detourAntiBotSystem.FindInternalKeyAsync(externalKey);

        var formContent = GetFormContent(login, password, internalKey);
        var response = await WebDriver.Client.PostAsync(RequestAddress, formContent);

        WebDriver.SetLastResponse(response);

        var parser = _parserFactory("ping");
        parser.Initialize();

        var responseUri = WebDriver.GetResponseUriAsString();
        var parserResult = parser.Parse(responseUri);
        return await Task.FromResult(parserResult);
    }

    private FormUrlEncodedContent GetFormContent(string login, string password, string internalKey)
    {
        var formVariables = new List<KeyValuePair<string, string>>();

        formVariables.Add(new KeyValuePair<string, string>("name", $"{login}"));
        formVariables.Add(new KeyValuePair<string, string>("password", $"{password}"));
        formVariables.Add(new KeyValuePair<string, string>($"{internalKey}", ""));

        var formContent = new FormUrlEncodedContent(formVariables);

        return formContent;
    }
}