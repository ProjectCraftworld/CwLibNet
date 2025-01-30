using System.ComponentModel.Design;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using net.torutheredfox.craftworld.util;
using net.torutheredfox.craftworld.gamedata;
using net.torutheredfox.craftworld.serialization;
using net.torutheredfox.graphics.contact_ao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
namespace net.torutheredfox.craftworld.slot
{

    public class SlotID : MonoBehaviour
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;
        public SlotType slotType = SelectionTypes.DEVELOPER;
        public long slotNumber;
        public SlotID() {} //constructs an empty slot ID.
        @param type;
        @param ID;
        public SlotID(SlotType type, long ID)
        {
            if (type == null){
                throw new NullPointerException("SlotType cannot be null!");}
            slotType = type;
            slotNumber = ID;
        }
            /*



                this portion is cut off so serialization can take effect



            */
        public void Serialize(Serializer Serializer)
        {
            slotType = Serializer.Serialize(ref slotType);
            slotNumber = Serializer.Serializer(ref slotNumber);
        }

        public int getAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }

        public bool equals(object o)
        {
            if (o == this) return true;
            if (!(o is SlotID d)) return false;
            return (slotType.equals(d.slotType) && slotNumber == d.slotNumber);
        }
    }
}