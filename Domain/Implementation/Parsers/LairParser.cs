using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;

namespace Domain.Implementation.Parsers;

public class LairParser : Parser
{
    public LairParser(IShowMessage showMessage) : base(showMessage)
    {
    }

    public override string Parse(string response)
    {
        Document.LoadHtml(response);
        var documentNode = Document.DocumentNode;

        var node = documentNode.QuerySelector(".ur");

        return node?.InnerHtml ?? string.Empty;
    }

    protected override string ParserString => "lair";
}