using Domain.Abstraction.Interfaces;
using System.Net;

namespace Domain.Implementation;

public class Browser : IBrowser
{
    private CookieContainer _cookies = new CookieContainer();
    private HttpClientHandler _handler = new HttpClientHandler();
    private HttpClient _client;
    private readonly IShowMessage _showMessage;
    public Browser(IShowMessage showMessage)
    {
        _showMessage = showMessage;
        Initialize();
    }

    public string GetResponseUriAsString()
    {
        var result = Response?.RequestMessage?.RequestUri?.AbsoluteUri ?? string.Empty;
        return result;
    }
    public string GetResponseMethodAsString()
    {
        var result = Response?.RequestMessage?.Method.ToString() ?? string.Empty;
        return result;
    }
    public void Initialize()
    {
        _cookies = new CookieContainer();
        _handler = new HttpClientHandler();

        _handler.CookieContainer = _cookies;
        _client = new HttpClient();
        _showMessage.ShowInfo("HttpClient initialized...");
    }
    public void SetLastResponse(HttpResponseMessage response)
    {
        Response = response;
    }

    

    public HttpClient Client => _client;

    public HttpResponseMessage Response { get; private set; }

    
}