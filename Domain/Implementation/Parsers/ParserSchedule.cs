using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Domain.Abstraction;
using Domain.Abstraction.Interfaces;
using Domain.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Domain.Implementation.Parsers;

public class ParserSchedule : Parser
{
    public ParserSchedule(IShowMessage showMessage) : base(showMessage)
    {
    }

    public override string Parse(string response)
    {
        var scheduleList = new List<ScheduleItem>();
        if (Document == null)
        {
            return JsonConvert.SerializeObject(scheduleList);
        }

        Document.LoadHtml(response);
        var documentNode = Document.DocumentNode;

        var nodes = documentNode.QuerySelectorAll(".mb10").ToList();

        foreach (var node in nodes)
        {
            var anchor = node.QuerySelector(".mb2");
            var parsedAnchorLink = GetHrefFromAnchor(anchor);

            if (parsedAnchorLink == string.Empty)
            {
                return string.Empty;
            }

            var paragraph = node.QuerySelector(".mt8");
            var timeValues = GetTimeValues(paragraph);

            scheduleList.Add(new ScheduleItem
            {
                Name = parsedAnchorLink,
                Days = timeValues.days,
                Hours = timeValues.hours,
                Minutes = timeValues.minutes
            });
        }

        return JsonConvert.SerializeObject(scheduleList);
    }

    private int GetInt32FromString(string rawString)
    {
        
        var result = 0;
        var resultString = string.Join(string.Empty, Regex.Matches(rawString, @"\d+").
            OfType<Match>().Select(m => m.Value));
        result = int.Parse(resultString);
        return result;
    }
    private string GetHrefFromAnchor(HtmlNode? anchor)
    {
        var href = string.Empty;
        if (anchor == null)
        {
            return href;
        }
        var anchorAttributes = anchor.Attributes;
        foreach (var attribute in anchorAttributes)
        {
            if (attribute.Name == "href")
            {
                href = attribute.Value;
            }
        }
        href = href.Trim('/');
        return href;
    }

    private (int days, int hours, int minutes) GetTimeValues(HtmlNode? paragraph)
    {
        (int days, int hours, int minutes) timeValues = (-1, -1, -1);
        if (paragraph == null)
        {
            return timeValues;
        }
        var paragraphInnerHtml = paragraph.InnerHtml;
        paragraphInnerHtml = paragraphInnerHtml.Trim(new Char[] { ' ', '\n' });
        var parsed = paragraphInnerHtml.Split(' ');

        timeValues = FillTimeValues(parsed);

        return timeValues;
    }

    private (int days, int hours, int minutes) FillTimeValues(string[] parsed)
    {
        var firstValue = GetInt32FromString(parsed[1]);
        var secondValue = GetInt32FromString(parsed[3]);

        (int days, int hours, int minutes) timeValues = (0, firstValue, secondValue);

        if (parsed.Length == 5)
        {
            timeValues.days = firstValue;
            timeValues.hours = secondValue;
            timeValues.minutes = GetInt32FromString(parsed[4]);
        }

        return timeValues;
    }
    protected override string ParserString => "schedule";

}