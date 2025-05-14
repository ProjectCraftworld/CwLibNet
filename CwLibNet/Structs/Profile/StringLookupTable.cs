using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class StringLookupTable : ISerializable, IEnumerable<SortString>
    {
        public const int BaseAllocationSize = 0x10;

        private bool _unsorted;
        private bool _sortEnabled = true;
        public int[]? RawIndexToSortedIndex;
        public List<SortString> StringList = [];

        public void Serialize(Serializer serializer)
        {
            if (serializer.IsWriting())
            {
                _sortEnabled = true;
                _unsorted = false;
                RawIndexToSortedIndex = new int[StringList.Count];
                StringList.Sort((a, z) => string.Compare(a.MyString, z.MyString, StringComparison.Ordinal));
                for (var i = 0; i < StringList.Count; i++)
                    RawIndexToSortedIndex[i] = i;
            }

            _unsorted = serializer.Bool(_unsorted);
            _sortEnabled = serializer.Bool(_sortEnabled);
            RawIndexToSortedIndex = serializer.Intvector(RawIndexToSortedIndex);
            StringList = serializer.Arraylist(StringList);
        }

        public int GetAllocatedSize()
        {
            var size = BaseAllocationSize;
            size += StringList.Sum(t => t.GetAllocatedSize());
            size += StringList.Count * sizeof(int);
            if (RawIndexToSortedIndex != null)
                size += RawIndexToSortedIndex.Length * 0x4;
            return size;
        }

        public IEnumerator<SortString> GetEnumerator()
        {
            return StringList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string? Get(int index)
        {
            return (from sortString in StringList where sortString.Index == index select sortString.MyString).FirstOrDefault();
        }


        /// <summary>
        ///  Finds the index of a string in the list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Find(string? key)
        {
            foreach (var myString in StringList.Where(myString => myString.MyString != null && myString.MyString.Equals(key)))
            {
                return myString.Index;
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
            foreach (var myString in StringList.Where(myString => myString.Index == key))
            {
                return myString.Index;
            }

            return -1;
        }

        public int Add(string? stringValue, int key)
        {
            var index = key != 0 ? Find(key) : Find(stringValue);
            if (index != -1)
            {
                return index;
            }

            index = StringList.Count;
            StringList.Add(new SortString(key, stringValue, index));
            return index;
        }

        public void Clear() 
        {
            StringList.Clear();
            RawIndexToSortedIndex = [];
        }
    }
}