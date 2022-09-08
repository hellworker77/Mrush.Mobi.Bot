using Domain.Abstraction.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Domain.Implementation;

public class DetourAntiBotSystem : IDetourAntiBotSystem
{
    private readonly HtmlDocument _document;
    private readonly IShowMessage _showMessage;
    public DetourAntiBotSystem(IShowMessage showMessage)
    {
        _showMessage = showMessage;
        _document = new HtmlDocument();
    }

    public async Task<string> FindDynamicKeyAsync(string response)
    {
        _document.LoadHtml(response);

        var shorterNode = string.Empty;

        var documentNode = _document.DocumentNode;
        documentNode = documentNode.QuerySelectorAll(".bdr").Last();
        var nodes = documentNode.QuerySelectorAll("style").ToList();

        nodes.ForEach(node =>
        {
            var nodeInfo = node.InnerHtml.Split('.')[1];

            if (shorterNode == string.Empty || (shorterNode.Length > nodeInfo.Length && !nodeInfo.Contains("overflow: hidden;")))
            {
                shorterNode = nodeInfo;
            }
        });

        var dynamicKey = shorterNode.Split('{')[0];

        return await Task.FromResult(dynamicKey);
    }

    public async Task<string> FindStaticKeyAsync(string dynamicKey)
    {
        var staticKey = string.Empty;
        try
        {
            var document = _document.DocumentNode;

            var nodes = document.QuerySelectorAll($".{dynamicKey}");

            nodes = nodes.ToList().FirstOrDefault().QuerySelectorAll("label input");

            foreach (var node in nodes)
            {
                var foundKey = TryToFindKey(node.Attributes);
                if (foundKey != string.Empty)
                {
                    staticKey = foundKey;
                    _showMessage.ShowSuccessful("Found secret key ");
                }
            }
        }
        catch (FormatException ex)
        {
            _showMessage.ShowError($"{ex.Message}");
            _showMessage.ShowError("Not hacked login page(...");
        }

        return await Task.FromResult(staticKey);
    }
    private string TryToFindKey(HtmlAttributeCollection attributes)
    {
        var foundKey = string.Empty;

        foreach (var attribute in attributes)
        {
            var attrValue = attribute.Value;
            if (attrValue != string.Empty)
            {
                foundKey = attrValue;
            }
        }

        return foundKey;
    }

}