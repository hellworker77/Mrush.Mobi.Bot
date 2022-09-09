using Domain.Abstraction.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Abstraction;

public abstract class Parser
{
    protected HtmlDocument? Document;
    protected readonly IShowMessage ShowMessage;
    protected Parser(IShowMessage showMessage)
    {
        ShowMessage = showMessage;
    }
    public virtual void Initialize()
    {
        Document = new HtmlDocument();
    }
    public abstract bool Parse(string response);

    protected virtual bool IsParserFor(string parserName)
    {
        return ParserString.Contains(parserName);
    }

    public static Func<IServiceProvider, Func<string, Parser>> GetParser =>
        provider => input =>
        {
            var command = provider.GetServices<Parser>().First(c => c.IsParserFor(input));

            return command;
        };
    protected abstract string ParserString { get; }
}