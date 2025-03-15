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
        public int id;
        public string name;
        public string description;
        public string root;
        public string icon;
        public int x, y;
        public SHA1[] resources;
        public string[] labels;
        public bool locked;
        public bool subLevel;
        public int shareable;
        public int background;
        public int minplayers = 1, maxplayers = 4;

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

                descriptor.id = (int?)element.Element("id") ?? 0;
                descriptor.name = (string)element.Element("name");
                descriptor.description = (string)element.Element("description");
                descriptor.root = ((string)element.Element("rootLevel"))?.ToLower();
                descriptor.icon = ((string)element.Element("icon"))?.ToLower();
                descriptor.locked = ((string)element.Element("initiallyLocked"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
                descriptor.subLevel = ((string)element.Element("isSubLevel"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
                descriptor.background = (int?)element.Element("background") ?? 0;
                descriptor.shareable = (int?)element.Element("shareable") ?? 0;
                descriptor.labels = ((string)element.Element("authorLabels"))?.Split(',');
                descriptor.minplayers = (int?)element.Element("minPlayers") ?? 1;
                descriptor.maxplayers = (int?)element.Element("maxPlayers") ?? 4;
                descriptor.isAdventurePlanet = ((string)element.Element("isAdventurePlanet"))?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

                var location = element.Element("location");
                if (location != null)
                {
                    descriptor.x = (int?)location.Element("x") ?? 0;
                    descriptor.y = (int?)location.Element("y") ?? 0;
                }

                descriptors[i++] = descriptor;
            }

            return descriptors;
        }
    }
}