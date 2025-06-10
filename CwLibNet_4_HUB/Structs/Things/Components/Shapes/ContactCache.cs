using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.Shapes;

public class ContactCache: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public Contact[]? Contacts;
    public bool ContactsSorted;
    
    public bool CacheDirtyButRecomputed;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Contacts);
        Serializer.Serialize(ref ContactsSorted);
        if (Serializer.GetCurrentSerializer().GetRevision().GetSubVersion() < 0x46)
            Serializer.Serialize(ref CacheDirtyButRecomputed);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Contacts == null) return size;
        size += Contacts.Sum(contact => contact.GetAllocatedSize());
        return size;
    }


}