using CwLibNet.Enums;
using CwLibNet.Util;

namespace CwLibNet.Types.Data;

public class ResourceDescriptor
{
    private readonly ResourceType type;
    private readonly GUID? guid;
    private readonly Sha1? sha1;
    private int flags;

    public ResourceDescriptor(ResourceDescriptor descriptor)
    {
        type = descriptor.type;
        guid = descriptor.guid;
        sha1 = descriptor.sha1;
        flags = descriptor.flags;
    }

    public ResourceDescriptor(string? resource, ResourceType type)
    {
        if (Strings.IsSHA1(resource))
        {
            var sha1 = Strings.GetSHA1(resource)!;
            this.sha1 = sha1.Equals(Sha1.Empty) ? null : sha1;
            guid = null;
        }
        else if (Strings.IsGUID(resource))
        {
            guid = new GUID(Strings.GetLong(resource));
            sha1 = null;
        }
        else
            throw new ArgumentException("Invalid resource reference passed into resource reference!");

        this.type = type;
    }

    public ResourceDescriptor(uint guid, ResourceType type)
    {
        this.guid = new GUID(guid);
        this.type = type;
        sha1 = null;
        flags = ResourceFlags.NONE;
    }

    public ResourceDescriptor(GUID guid, ResourceType type)
    {
        this.guid = guid;
        this.type = type;
        sha1 = null;
        flags = ResourceFlags.NONE;
    }

    public ResourceDescriptor(Sha1 sha1, ResourceType type)
    {
        this.sha1 = sha1.Equals(Sha1.Empty) ? null : sha1;
        this.type = type;
        guid = null;
        flags = ResourceFlags.NONE;
    }

    public ResourceDescriptor(GUID? guid, Sha1? sha1, ResourceType type)
    {
        this.guid = guid;
        this.sha1 = sha1 != null && sha1.Equals(Sha1.Empty) ? null : sha1;
        this.type = type;
        flags = ResourceFlags.NONE;
    }

    public bool IsGUID()
    {
        return guid != null;
    }

    public bool IsHash()
    {
        return sha1 != null;
    }

    public bool IsValid()
    {
        return guid != null || (sha1 != null && !sha1.Equals(Sha1.Empty));
    }

    public GUID? GetGUID()
    {
        return guid;
    }

    public Sha1? GetSHA1()
    {
        return sha1;
    }

    public ResourceType GetResourceType()
    {
        return type;
    }

    public int GetFlags()
    {
        return flags;
    }

    public void SetFlags(int flags)
    {
        this.flags = flags;
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj == this) return true;
        return obj is ResourceDescriptor reference && reference.ToString().Equals(ToString());
    }

    public override string ToString()
    {
        if (sha1 != null && guid != null)
            return $"{sha1} ({guid})";
        if (sha1 != null)
            return "h" + sha1;
        return guid != null ? guid.ToString() : "null";
    }
}