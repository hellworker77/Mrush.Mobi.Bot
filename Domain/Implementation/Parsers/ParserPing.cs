using System.Text.Json.Nodes;
using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;
using Newtonsoft.Json;

namespace Domain.Implementation.Parsers;

public class ParserPing : Parser
{
    public ParserPing(IShowMessage showMessage) : base(showMessage)
    {

    }

    public override string Parse(string response)
    {
        var result = false;

        result = !response.Contains("welcome");
        
        return JsonConvert.SerializeObject(result);
    }

    protected override bool IsParserFor(string parserName)
    {
        return parserName == ParserString;
    }

    protected override string ParserString => "ping";
}