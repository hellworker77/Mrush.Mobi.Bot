using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;

namespace Domain.Implementation.Parsers;

public class ArenaParser : Parser
{
    public ArenaParser(IShowMessage showMessage) : base(showMessage)
    {
    }

    public override string Parse(string response)
    {
        Document.LoadHtml(response);
        var documentNode = Document.DocumentNode;

        var node = documentNode.QuerySelector(".ur");
        var innerText = node?.InnerText ?? string.Empty;


        return innerText;
    }

    protected override string ParserString => "arena";
}