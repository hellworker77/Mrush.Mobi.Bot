using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;

namespace Domain.Implementation.Parsers;

public class UnKnownParser : Parser
{
    public UnKnownParser(IShowMessage showMessage) : base(showMessage)
    {

    }

    public override bool Parse(string responseHtml)
    {
        var result = false;

        return result;
    }

    protected override bool IsParserFor(string parserName)
    {
        return true;
    }

    protected override string ParserString => string.Empty;
}