namespace ModdingToolkit.Core
{
    public enum ExitCodes
    {
        Normal = 0,

        SteamNotFound = 1,
        SteamInstallPathNotFound = 2,

        MagickaFolderNotFound = 3,
        MagickaExecutableNotFound = 4,

        BuildFailed = 5
    }
}