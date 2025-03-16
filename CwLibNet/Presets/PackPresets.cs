using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Presets
{
    public class Presets
    {
        public static Pack Group(string name, string description, string author, ResourceDescriptor icon)
        {
            Pack item = new Pack();
            item.contentsType = ContentsType.GROUP;
            item.mesh = new ResourceDescriptor(0x6a1a, ResourceType.Mesh);
            
            item.slot.name = name;
            item.slot.description = description;

            item.slot.icon = icon;
            item.slot.authorName = author;

            return item;
        }
        public static Pack Level(string name, string description, string author, ResourceDescriptor icon)
        {
            Pack item = new Pack();

            item.contentID = null; // TODO: Set contentID if needed

            item.contentsType = ContentsType.LEVEL;
            item.mesh = new ResourceDescriptor(0x3e86, ResourceType.Mesh);
            
            item.slot.name = name;
            item.slot.description = description;

            item.slot.icon = icon;
            item.slot.authorName = author;

            return item;
        }
    }
}