using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Server
{
    public class SlotDescriptor 
    {
        public int Id;
        public string Name;
        public string Description;
        public string? Root;
        public string? Icon;
        public int X, Y;
        public SHA1[] Resources;
        public string[]? Labels;
        public bool Locked;
        public bool IsSubLevel;
        public bool IsAdventurePlanet;
        public int Shareable;
        public int Background;
        public int Minplayers = 1, Maxplayers = 4;

        public static SlotDescriptor[] Parse(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            XElement root = doc.Root;

            if (root == null)
                throw new Exception("Invalid XML format");

            var elements = root.Name.LocalName switch
            {
                "slots" => root.Elements("slot").ToArray(),
                "slot" => new[] { root },
                _ => throw new Exception("Invalid XML format")
            };

            SlotDescriptor[] descriptors = new SlotDescriptor[elements.Length];
            int i = 0;
            foreach (var element in elements)
            {
                SlotDescriptor descriptor = new SlotDescriptor();

                descriptor.Id = (int?)element.Element("id") ?? 0;
                descriptor.Name = (string)element.Element("name");
                descriptor.Description = (string)element.Element("description");
                descriptor.Root = ((string)element.Element("rootLevel"))?.ToLower();
                descriptor.Icon = ((string)element.Element("icon"))?.ToLower();
                descriptor.Locked = ((string)element.Element("initiallyLocked"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
                descriptor.IsSubLevel = ((string)element.Element("isSubLevel"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
                descriptor.Background = (int?)element.Element("background") ?? 0;
                descriptor.Shareable = (int?)element.Element("shareable") ?? 0;
                descriptor.Labels = ((string)element.Element("authorLabels"))?.Split(',');
                descriptor.Minplayers = (int?)element.Element("minPlayers") ?? 1;
                descriptor.Maxplayers = (int?)element.Element("maxPlayers") ?? 4;
                descriptor.IsAdventurePlanet = ((string)element.Element("isAdventurePlanet"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

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
}