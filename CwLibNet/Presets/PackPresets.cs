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
            item.ContentsType = ContentsType.GROUP;
            item.Mesh = new ResourceDescriptor(0x6a1a, ResourceType.Mesh);
            
            item.Slot.name = name;
            item.Slot.description = description;

            item.Slot.icon = icon;
            item.Slot.authorName = author;

            return item;
        }
        public static Pack Level(string name, string description, string author, ResourceDescriptor icon)
        {
            Pack item = new Pack();

            item.ContentId = null; // TODO: Set contentID if needed

            item.ContentsType = ContentsType.LEVEL;
            item.Mesh = new ResourceDescriptor(0x3e86, ResourceType.Mesh);
            
            item.Slot.name = name;
            item.Slot.description = description;

            item.Slot.icon = icon;
            item.Slot.authorName = author;

            return item;
        }
    }
}