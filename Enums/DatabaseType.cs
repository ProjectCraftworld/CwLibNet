namespace CwLibNet.Enums
{
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

        // --------------------
        // TODO enum body members
        // /**
        //  * Whether or not this database type has entries
        //  * with GUIDs
        //  */
        // private final boolean hasGUIDs;
        // private final boolean containsData;
        // private final String name;
        // private final String extension;
        // DatabaseType(String name, String extension, boolean hasKeys, boolean hasData) {
        //     this.name = name;
        //     this.extension = extension;
        //     this.hasGUIDs = hasKeys;
        //     this.containsData = hasData;
        // }
        // public String getName() {
        //     return this.name;
        // }
        // public String getExtension() {
        //     return this.extension;
        // }
        // public boolean hasGUIDs() {
        //     return this.hasGUIDs;
        // }
        // public boolean containsData() {
        //     return this.containsData;
        // }
        // --------------------
    }
}