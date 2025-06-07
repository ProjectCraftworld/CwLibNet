using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Shapes;

public class ContactCache: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public Contact[]? Contacts;
    public bool ContactsSorted;
    
    public bool CacheDirtyButRecomputed;

    
    public void Serialize()
    {
        Contacts = Serializer.Serialize(ref Contacts);
        Serializer.Serialize(ref ContactsSorted);
        if (Serializer.GetRevision().GetSubVersion() < 0x46)
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