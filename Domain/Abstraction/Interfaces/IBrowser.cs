namespace Domain.Abstraction.Interfaces;

public interface IBrowser
{
    public HttpClient Client { get; }
    public HttpResponseMessage Response { get; }
    public void SetLastResponse(HttpResponseMessage response);
    public string GetResponseMethodAsString();
    public string GetResponseUriAsString();
    public void Initialize();
}