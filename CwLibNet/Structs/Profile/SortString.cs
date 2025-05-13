using System;
using System.Collections.Generic;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class SortString : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int lamsKeyID;
        public string myString;
        public int index;

        public SortString() { }

        public SortString(int key, string myString, int index)
        {
            this.lamsKeyID = key;
            this.myString = myString;
            this.index = index;
        }

        public void Serialize(Serializer serializer)
        {
            lamsKeyID = serializer.I32(lamsKeyID);
            myString = serializer.Wstr(myString);
            index = serializer.I32(index);
        }

        public int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (myString != null)
            {
                size += myString.Length * 2;
            }
            return size;
        }

        public bool equals(SortString other)
        {
            if (other == this) return true;
            if (!(other is SortString)) return false;
            return ((SortString) other).myString.Equals(this.myString);
        }

        public int hashCode() 
        {
            return this.myString.GetHashCode();
        }
    }
}