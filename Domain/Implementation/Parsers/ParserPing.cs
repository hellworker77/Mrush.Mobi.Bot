using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;

namespace Domain.Implementation.Parsers;

public class ParserPing : Parser
{
    public ParserPing(IShowMessage showMessage) : base(showMessage)
    {

    }

    public override bool Parse(string response)
    {
        var result = false;

        result = !response.Contains("welcome");
        
        return result;
    }

    protected override bool IsParserFor(string parserName)
    {
        return parserName == ParserString;
    }

    protected override string ParserString => "ping";
}