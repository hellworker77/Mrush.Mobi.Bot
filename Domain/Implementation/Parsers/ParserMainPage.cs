using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;

namespace Domain.Implementation.Parsers;

public class ParserMainPage : Parser
{
    public ParserMainPage(IShowMessage showMessage) : base(showMessage)
    {

    }

    public override bool Parse(string responseHtml)
    {
        var result = false;

        if (Document != null)
        {
            Document.LoadHtml(responseHtml);
            var document = Document.DocumentNode;
            var cssSelector = ".mbtn";
            var lairNameNode = document.QuerySelector(cssSelector);
            if (lairNameNode != null)
            {
                result = true;
            }
        }
        

        return result;
    }

    protected override bool IsParserFor(string parserName)
    {
        return parserName == ParserString;
    }

    protected override string ParserString => "ping";
}