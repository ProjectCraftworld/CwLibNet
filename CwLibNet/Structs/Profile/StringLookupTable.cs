using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class StringLookupTable : ISerializable, IEnumerable<SortString>
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        private bool unsorted;
        private bool sortEnabled = true;
        public int[] rawIndexToSortedIndex;
        public List<SortString> stringList = new List<SortString>();

        public void Serialize(Serializer serializer)
        {
            if (serializer.IsWriting())
            {
                sortEnabled = true;
                unsorted = false;
                rawIndexToSortedIndex = new int[stringList.Count];
                stringList.Sort((a, z) => a.myString.CompareTo(z.myString));
                for (int i = 0; i < stringList.Count; i++)
                    rawIndexToSortedIndex[i] = i;
            }

            unsorted = serializer.Bool(unsorted);
            sortEnabled = serializer.Bool(sortEnabled);
            rawIndexToSortedIndex = serializer.Intvector(rawIndexToSortedIndex);
            stringList = serializer.Arraylist(stringList);
        }

        public int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (stringList != null)
            {
                for (int i = 0; i < stringList.Count; i++)
                {
                    size += stringList[i].GetAllocatedSize();
                }
                size += stringList.Count * sizeof(int);
            }
            if (rawIndexToSortedIndex != null)
                size += rawIndexToSortedIndex.Length * 0x4;
            return size;
        }

        public IEnumerator<SortString> GetEnumerator()
        {
            return stringList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Get(int index)
        {
            foreach (SortString sortString in stringList)
            {
                if (sortString.index == index)
                {
                    return sortString.myString;
                }
            }
            return null;
        }


        /// <summary>
        ///  Finds the index of a string in the list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Find(string key)
        {
            for (int i = 0; i < stringList.Count; i++)
            {
                SortString myString = stringList[i];
                if (myString.myString.Equals(key))
                {
                    return myString.index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Finds the index of a string in the list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Find(int key)
        {
            for (int i = 0; i < stringList.Count; i++)
            {
                SortString myString = stringList[i];
                if (myString.index == key)
                {
                    return myString.index;
                }
            }
            return -1;
        }

        public int Add(string stringValue, int key)
        {
            int index = key != 0 ? Find(key) : Find(stringValue);
            if (index != -1)
            {
                return index;
            }

            index = stringList.Count;
            stringList.Add(new SortString(key, stringValue, index));
            return index;
        }

        public void Clear() 
        {
            stringList.Clear();
            rawIndexToSortedIndex = new int[0];
        }
    }
}