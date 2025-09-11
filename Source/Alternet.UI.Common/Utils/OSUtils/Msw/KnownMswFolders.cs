using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Alternet.UI;

/// <summary>
/// Provides GUIDs for known Windows folders.
/// </summary>
public static class KnownMswFolders
{
    /// <summary>
    /// Provides a mapping between shell folder identifiers and their corresponding GUIDs.
    /// </summary>
    /// <remarks>This dictionary maps well-known shell folder paths, represented as strings, to functions that
    /// return their corresponding <see cref="Guid"/> values. The keys are case-insensitive and use the format
    /// "shell:{FolderName}", where {FolderName} corresponds to a specific shell folder (e.g., "shell:Documents").
    /// The GUIDs returned by the functions correspond to the identifiers of the respective shell folders
    /// as defined in the <see cref="KnownMswFolders"/> class.
    /// This mapping can be used to resolve shell folder paths to their GUIDs for
    /// various system operations.</remarks>
    public static readonly Dictionary<string, Func<Guid>> ShellToGuid = new(StringComparer.OrdinalIgnoreCase)
    {
        { "shell:3D Objects", () => KnownMswFolders.Objects3D },
        { "shell:AccountPictures", () => KnownMswFolders.AccountPictures },
        { "shell:AppData", () => KnownMswFolders.RoamingAppData },
        { "shell:Common AppData", () => KnownMswFolders.ProgramData },
        { "shell:Common Desktop", () => KnownMswFolders.PublicDesktop },
        { "shell:Common Documents", () => KnownMswFolders.PublicDocuments },
        { "shell:CommonDownloads", () => KnownMswFolders.PublicDownloads },
        { "shell:CommonMusic", () => KnownMswFolders.PublicMusic },
        { "shell:CommonPictures", () => KnownMswFolders.PublicPictures },
        { "shell:CommonVideo", () => KnownMswFolders.PublicVideos },
        { "shell:Contacts", () => KnownMswFolders.Contacts },
        { "shell:Cookies", () => KnownMswFolders.Cookies },
        { "shell:Desktop", () => KnownMswFolders.Desktop },
        { "shell:Documents", () => KnownMswFolders.Documents },
        { "shell:Downloads", () => KnownMswFolders.Downloads },
        { "shell:Favorites", () => KnownMswFolders.Favorites },
        { "shell:Fonts", () => KnownMswFolders.Fonts },
        { "shell:GameTasks", () => KnownMswFolders.GameTasks },
        { "shell:History", () => KnownMswFolders.History },
        { "shell:Links", () => KnownMswFolders.Links },
        { "shell:Local AppData", () => KnownMswFolders.LocalAppData },
        { "shell:LocalAppDataLow", () => KnownMswFolders.LocalAppDataLow },
        { "shell:Music", () => KnownMswFolders.Music },
        { "shell:Pictures", () => KnownMswFolders.Pictures },
        { "shell:ProgramFiles", () => KnownMswFolders.ProgramFiles },
        { "shell:ProgramFilesCommon", () => KnownMswFolders.ProgramFilesCommon },
        { "shell:ProgramFilesCommonX86", () => KnownMswFolders.ProgramFilesCommonX86 },
        { "shell:ProgramFilesX86", () => KnownMswFolders.ProgramFilesX86 },
        { "shell:Programs", () => KnownMswFolders.Programs },
        { "shell:Public", () => KnownMswFolders.Profile }, // fallback
        { "shell:SavedGames", () => KnownMswFolders.SavedGames },
        { "shell:Searches", () => KnownMswFolders.Searches },
        { "shell:Start Menu", () => KnownMswFolders.StartMenu },
        { "shell:Startup", () => KnownMswFolders.Startup },
        { "shell:System", () => KnownMswFolders.System },
        { "shell:SystemX86", () => KnownMswFolders.SystemX86 },
        { "shell:Templates", () => KnownMswFolders.Templates },
        { "shell:Videos", () => KnownMswFolders.Videos },
        { "shell:Windows", () => KnownMswFolders.Windows },
    };

    // User Profile

    /// <summary>
    /// Gets the GUID for the user's profile folder.
    /// </summary>
    public static Guid Profile => new("5E6C858F-0E22-4760-9AFE-EA3317B67173");

    /// <summary>
    /// Gets the GUID for the user's desktop folder.
    /// </summary>
    public static Guid Desktop => new("B4BFCC3A-DB2C-424C-B029-7FE99A87C641");

    /// <summary>
    /// Gets the GUID for the user's documents folder.
    /// </summary>
    public static Guid Documents => new("FDD39AD0-238F-46AF-ADB4-6C85480369C7");

    /// <summary>
    /// Gets the GUID for the user's downloads folder.
    /// </summary>
    public static Guid Downloads => new("374DE290-123F-4565-9164-39C4925E467B");

    /// <summary>
    /// Gets the GUID for the user's pictures folder.
    /// </summary>
    public static Guid Pictures => new("33E28130-4E1E-4676-835A-98395C3BC3BB");

    /// <summary>
    /// Gets the GUID for the user's music folder.
    /// </summary>
    public static Guid Music => new("4BD8D571-6D19-48D3-BE97-422220080E43");

    /// <summary>
    /// Gets the GUID for the user's videos folder.
    /// </summary>
    public static Guid Videos => new("18989B1D-99B5-455B-841C-AB7C74E4DDFC");

    /// <summary>
    /// Gets the GUID for the user's favorites folder.
    /// </summary>
    public static Guid Favorites => new("1777F761-68AD-4D8A-87BD-30B759FA33DD");

    /// <summary>
    /// Gets the GUID for the user's contacts folder.
    /// </summary>
    public static Guid Contacts => new("56784854-C6CB-462B-8169-88E350ACB882");

    /// <summary>
    /// Gets the GUID for the user's saved games folder.
    /// </summary>
    public static Guid SavedGames => new("4C5C32FF-BB9D-43B0-BF75-5310B4285A02");

    /// <summary>
    /// Gets the GUID for the user's searches folder.
    /// </summary>
    public static Guid Searches => new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA");

    // App Data

    /// <summary>
    /// Gets the GUID for the roaming application data folder.
    /// </summary>
    public static Guid RoamingAppData => new("3EB685DB-65F9-4CF6-A03A-E3EF65729F3D");

    /// <summary>
    /// Gets the GUID for the local application data folder.
    /// </summary>
    public static Guid LocalAppData => new("F1B32785-6FBA-4FCF-9D55-7B8E7F157091");

    /// <summary>
    /// Gets the GUID for the local low application data folder.
    /// </summary>
    public static Guid LocalAppDataLow => new("A520A1A4-1780-4FF6-BD18-167343C5AF16");

    /// <summary>
    /// Gets the GUID for the program data folder.
    /// </summary>
    public static Guid ProgramData => new("62AB5D82-FDC1-4DC3-A9DD-070D1D495D97");

    /// <summary>
    /// Gets the GUID for the internet cache folder.
    /// </summary>
    public static Guid InternetCache => new("352481E8-33BE-4251-BA85-6007CAEDCF9D");

    /// <summary>
    /// Gets the GUID for the cookies folder.
    /// </summary>
    public static Guid Cookies => new("2B0F765D-C0E9-4171-908E-08A611B84FF6");

    /// <summary>
    /// Gets the GUID for the history folder.
    /// </summary>
    public static Guid History => new("D9DC8A3B-B784-432E-A781-5A1130A75963");

    // Public Folders

    /// <summary>
    /// Gets the GUID for the public desktop folder.
    /// </summary>
    public static Guid PublicDesktop => new("C4AA340D-F20F-4863-AFEF-F87EF2E6BA25");

    /// <summary>
    /// Gets the GUID for the public documents folder.
    /// </summary>
    public static Guid PublicDocuments => new("ED4824AF-DCE4-45A8-81E2-FC7965083634");

    /// <summary>
    /// Gets the GUID for the public downloads folder.
    /// </summary>
    public static Guid PublicDownloads => new("3D644C9B-1FB8-4F30-9B45-F670235F79C0");

    /// <summary>
    /// Gets the GUID for the public pictures folder.
    /// </summary>
    public static Guid PublicPictures => new("B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5");

    /// <summary>
    /// Gets the GUID for the public music folder.
    /// </summary>
    public static Guid PublicMusic => new("3214FAB5-9757-4298-BB61-92A9DEAA44FF");

    /// <summary>
    /// Gets the GUID for the public videos folder.
    /// </summary>
    public static Guid PublicVideos => new("2400183A-6185-49FB-A2D8-4A392A602BA3");

    // System

    /// <summary>
    /// Gets the GUID for the Windows folder.
    /// </summary>
    public static Guid Windows => new("F38BF404-1D43-42F2-9305-67DE0B28FC23");

    /// <summary>
    /// Gets the GUID for the system folder.
    /// </summary>
    public static Guid System => new("1AC14E77-02E7-4E5D-B744-2EB1AE5198B7");

    /// <summary>
    /// Gets the GUID for the system x86 folder.
    /// </summary>
    public static Guid SystemX86 => new("D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27");

    /// <summary>
    /// Gets the GUID for the Program Files folder.
    /// </summary>
    public static Guid ProgramFiles => new("905E63B6-C1BF-494E-B29C-65B732D3D21A");

    /// <summary>
    /// Gets the GUID for the Program Files x86 folder.
    /// </summary>
    public static Guid ProgramFilesX86 => new("6D809377-6AF0-444B-8957-A3773F02200E");

    /// <summary>
    /// Gets the GUID for the common Program Files folder.
    /// </summary>
    public static Guid ProgramFilesCommon => new("F7F1ED05-9F6D-47A2-AAAE-29D317C6F066");

    /// <summary>
    /// Gets the GUID for the common Program Files x86 folder.
    /// </summary>
    public static Guid ProgramFilesCommonX86 => new("DE974D24-D9C6-4D3E-BF91-F4455120B917");

    // Start Menu & Templates

    /// <summary>
    /// Gets the GUID for the Start Menu folder.
    /// </summary>
    public static Guid StartMenu => new("625B53C3-AB48-4EC1-BA1F-A1EF4146FC19");

    /// <summary>
    /// Gets the GUID for the Programs folder.
    /// </summary>
    public static Guid Programs => new("A77F5D77-2E2B-44C3-A6A2-ABA601054A51");

    /// <summary>
    /// Gets the GUID for the Startup folder.
    /// </summary>
    public static Guid Startup => new("B97D20BB-F46A-4C97-BA10-5E3608430854");

    /// <summary>
    /// Gets the GUID for the Templates folder.
    /// </summary>
    public static Guid Templates => new("A63293E8-664E-48DB-A079-DF759E0509F7");

    // Account Pictures

    /// <summary>
    /// Gets the GUID for the account pictures folder.
    /// </summary>
    public static Guid AccountPictures => new("008CA0B1-55B4-4C56-B8A8-4DE4B299D3BE");

    // Virtual Folders

    /// <summary>
    /// Gets the GUID for the Computer virtual folder.
    /// </summary>
    public static Guid Computer => new("0AC0837C-BBF8-452A-850D-79D08E667CA7");

    /// <summary>
    /// Gets the GUID for the Network virtual folder.
    /// </summary>
    public static Guid Network => new("D20BEEC4-5CA8-4905-AE3B-BF251EA09B53");

    /// <summary>
    /// Gets the GUID for the Control Panel virtual folder.
    /// </summary>
    public static Guid ControlPanel => new("82A74AEB-AEB4-465C-A014-D097EE346D63");

    /// <summary>
    /// Gets the GUID for the Printers virtual folder.
    /// </summary>
    public static Guid Printers => new("76FC4E2D-D6AD-4519-A663-37BD56068185");

    // Misc

    /// <summary>
    /// Gets the GUID for the Fonts folder.
    /// </summary>
    public static Guid Fonts => new("FD228CB7-AE11-4AE3-864C-16F3910AB8FE");

    /// <summary>
    /// Gets the GUID for the Links folder.
    /// </summary>
    public static Guid Links => new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968");

    /// <summary>
    /// Gets the GUID for the Games folder.
    /// </summary>
    public static Guid Games => new("CAC52C1A-B53D-4EDC-92D7-6B2E8AC19434");

    /// <summary>
    /// Gets the GUID for the Game Tasks folder.
    /// </summary>
    public static Guid GameTasks => new("054FAE61-4DD8-4787-80B6-090220C4B700");

    /// <summary>
    /// Gets the GUID for the 3D Objects folder.
    /// </summary>
    public static Guid Objects3D => new("31C0DD25-9439-4F12-BF41-7FF4EDA38722");

    /// <summary>
    /// Gets the GUID for the Original Images folder.
    /// </summary>
    public static Guid OriginalImages => new("2C36C0AA-5812-4B87-BFD0-4CD0DFB19B39");

    /// <summary>
    /// Gets the GUID for the Photo Albums folder.
    /// </summary>
    public static Guid PhotoAlbums => new("69D2CF90-FC33-4FB7-9A0C-EBB0F0FCB43C");

    /// <summary>
    /// Attempts to resolve the file system path associated with the specified known folder identifier.
    /// </summary>
    /// <remarks>This method is only supported on Windows operating systems.
    /// If the application is not running
    /// on Windows, the method will return <see langword="false"/>.</remarks>
    /// <param name="folderId">The <see cref="Guid"/> representing the known folder identifier to resolve.</param>
    /// <param name="path">When this method returns, contains the resolved file system path
    /// if the operation succeeds; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the path was successfully resolved;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryResolvePath(Guid folderId, out string? path)
    {
        path = null;

        try
        {
            if (!App.IsWindowsOS || !MswUtils.IsWindowsVistaOrLater())
                return false;

            int hr = MswUtils.NativeMethods.SHGetKnownFolderPath(folderId, 0, IntPtr.Zero, out IntPtr outPath);
            if (hr != 0)
            {
                Debug.WriteLine($"KnownMswFolders.TryResolvePath Error: {Marshal.GetExceptionForHR(hr)}");
                return false;
            }

            path = Marshal.PtrToStringUni(outPath);
            Marshal.FreeCoTaskMem(outPath);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"KnownMswFolders.TryResolvePath Error: {ex}");
            return false;
        }
    }
}
