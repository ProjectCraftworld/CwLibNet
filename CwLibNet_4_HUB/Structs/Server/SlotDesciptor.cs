using System.Xml.Linq;
using CwLibNet.Types.Data;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Server;

public class SlotDescriptor 
{
    public int Id;
    public string Name;
    public string Description;
    public string? Root;
    public string? Icon;
    public int X, Y;
    public Sha1[] Resources;
    public string[]? Labels;
    public bool Locked;
    public bool IsSubLevel;
    public bool IsAdventurePlanet;
    public int Shareable;
    public int Background;
    public int Minplayers = 1, Maxplayers = 4;

    public static SlotDescriptor[] Parse(string xml)
    {
        var doc = XDocument.Parse(xml);
        var root = doc.Root;

        if (root == null)
            throw new Exception("Invalid XML format");

        var elements = root.Name.LocalName switch
        {
            "slots" => root.Elements("slot").ToArray(),
            "slot" => [root],
            _ => throw new Exception("Invalid XML format")
        };

        var descriptors = new SlotDescriptor[elements.Length];
        var i = 0;
        foreach (var element in elements)
        {
            var descriptor = new SlotDescriptor
            {
                Id = (int?)element.Element("id") ?? 0,
                Name = (string)element.Element("name"),
                Description = (string)element.Element("description"),
                Root = ((string)element.Element("rootLevel"))?.ToLower(),
                Icon = ((string)element.Element("icon"))?.ToLower(),
                Locked = ((string)element.Element("initiallyLocked"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false,
                IsSubLevel = ((string)element.Element("isSubLevel"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false,
                Background = (int?)element.Element("background") ?? 0,
                Shareable = (int?)element.Element("shareable") ?? 0,
                Labels = ((string)element.Element("authorLabels"))?.Split(','),
                Minplayers = (int?)element.Element("minPlayers") ?? 1,
                Maxplayers = (int?)element.Element("maxPlayers") ?? 4,
                IsAdventurePlanet = ((string)element.Element("isAdventurePlanet"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false
            };

            var location = element.Element("location");
            if (location != null)
            {
                descriptor.X = (int?)location.Element("x") ?? 0;
                descriptor.Y = (int?)location.Element("y") ?? 0;
            }

            descriptors[i++] = descriptor;
        }

        return descriptors;
    }
}