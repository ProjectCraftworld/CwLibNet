using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Shapes;

public class ContactCache: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public Contact[]? Contacts;
    public bool ContactsSorted;
    
    public bool CacheDirtyButRecomputed;

    
    public void Serialize(Serializer serializer)
    {
        Contacts = serializer.Array(Contacts);
        ContactsSorted = serializer.Bool(ContactsSorted);
        if (serializer.GetRevision().GetSubVersion() < 0x46)
            CacheDirtyButRecomputed = serializer.Bool(CacheDirtyButRecomputed);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Contacts == null) return size;
        size += Contacts.Sum(contact => contact.GetAllocatedSize());
        return size;
    }


}