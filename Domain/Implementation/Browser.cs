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

    public void Initialize()
    {
        _cookies = new CookieContainer();
        _handler = new HttpClientHandler();

        _handler.CookieContainer = _cookies;
        _client = new HttpClient();
        _showMessage.ShowInfo("HttpClient initialized...");
    }

    public HttpClient Client => _client;

    public HttpResponseMessage Response { get; private set; }

    public void SetLastResponse(HttpResponseMessage response)
    {
        Response = response;
    }
}