using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using IWshRuntimeLibrary;

namespace DynamicWin.Utils
{

    public class StartupShortcutManager
    {
        private static string GetStartupFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        }

        private static string GetShortcutPath(string shortcutName)
        {
            return Path.Combine(GetStartupFolderPath(), $"{shortcutName}.lnk");
        }

        public static void CreateShortcut()
        {
            string appPath = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            string shortcutPath = GetShortcutPath(appPath);

            if (System.IO.File.Exists(shortcutPath))
            {
                Console.WriteLine("Shortcut already exists.");
                return;
            }

            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            IShellLink wshShell = (IShellLink)new ShellLink();
            wshShell.SetWorkingDirectory(Path.GetDirectoryName(exePath));
            wshShell.SetPath(exePath);
            wshShell.SetDescription("Launches the app on system startup.");
            IPersistFile file = (IPersistFile)wshShell;
            file.Save(shortcutPath, false);

            Console.WriteLine("Shortcut created successfully.");
        }

        public static bool RemoveShortcut()
        {
            string appPath = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            string shortcutPath = GetShortcutPath(appPath);

            if (System.IO.File.Exists(shortcutPath))
            {
                System.IO.File.Delete(shortcutPath);
                Console.WriteLine("Shortcut removed successfully.");
                return true;
            }

            Console.WriteLine("Shortcut does not exist.");
            return false;
        }
    }
}