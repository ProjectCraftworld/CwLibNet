namespace CwLibNet.Enums;

public enum DatabaseType
{
    // NONE(null, null, false, false)
    NONE,
    // FILE_DATABASE("FileDB", "map", true, false)
    FILE_DATABASE,
    // BIGFART("Big Profile", "", false, true)
    BIGFART,
    // SAVE("Profile Data", null, false, true)
    SAVE,
    // MOD("Mod", "mod", true, true)
    MOD 
}

public sealed class DatabaseBody
{
    private readonly bool hasGUIDs;
    private readonly bool containsData;
    private readonly DatabaseType name;
    private readonly DatabaseType extension;

    private DatabaseBody(string name, string extension, bool hasKeys, bool hasData)
    {
        this.name = (DatabaseType)Enum.Parse(typeof(DatabaseType), name);
        this.extension = (DatabaseType)Enum.Parse(typeof(DatabaseType), name);
        hasGUIDs = hasKeys;
        containsData = hasData;
    }

    public string GetName()
    {
        return name.ToString();
    }

    public string GetExtension()
    {
        return extension.ToString();
    }

    public bool HasGUIDs => hasGUIDs;

    public bool ContainsData => containsData;
}