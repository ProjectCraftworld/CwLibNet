using static CwLibNet.IO.ValueEnum<int>;

namespace CwLibNet.Enums
{
    public enum DatabaseType : int
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

        DatabaseBody(String name, String extension, bool hasKeys, bool hasData)
        {
            this.name = (DatabaseType)Enum.Parse(typeof(DatabaseType), name);
            this.extension = (DatabaseType)Enum.Parse(typeof(DatabaseType), name);
            this.hasGUIDs = hasKeys;
            this.containsData = hasData;
        }

        public String getName()
        {
            return this.name.ToString();
        }

        public String getExtension()
        {
            return this.extension.ToString();
        }

        public DatabaseType HasGUIDs
        {
            get {return this.HasGUIDs;}
        }

        public DatabaseType ContainsData
        {
            get {return this.ContainsData;}
        }
    }
}