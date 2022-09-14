using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;
using Newtonsoft.Json;

namespace Domain.Implementation.Parsers;

public class UnKnownParser : Parser
{
    public UnKnownParser(IShowMessage showMessage) : base(showMessage)
    {

    }

    public override string Parse(string response)
    {
        return JsonConvert.SerializeObject(false);
    }

    protected override bool IsParserFor(string parserName)
    {
        return true;
    }

    protected override string ParserString => string.Empty;
}