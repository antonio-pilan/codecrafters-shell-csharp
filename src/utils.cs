using System.Reflection.Metadata.Ecma335;
using System.IO;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

namespace Utils
{
    public class PathVariables
    {
        public static string? GetPath(string command)
        {
            string? systemPath = Environment.GetEnvironmentVariable("PATH");
            
            if (!string.IsNullOrEmpty(systemPath))
            {
                string expandedPath = Environment.ExpandEnvironmentVariables(systemPath);

                string[] paths = expandedPath.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

                foreach (string path in paths)
                {   
                    string cleanPath = path.Trim();
                    string? exe = GetExecutable(cleanPath, command);
                    
                    if (exe != null)
                    {
                        return exe;
                    }
                }
            }
            return null;
        }

        public static string? GetExecutable(string directory, string filename)
        {
            if (string.IsNullOrEmpty(directory)) return null;

            string fullPath = Path.Combine(directory, filename);
            if (File.Exists(fullPath)) 
            {
                if (IsExecutable(fullPath)) return fullPath;  
            }

            string exePath = fullPath + ".exe";
            if (File.Exists(exePath)) return exePath;
        
            return null;
        }

        private static bool IsExecutable(string path)
        {
            if (OperatingSystem.IsWindows()) return true;

            try 
            {
                UnixFileMode mode = File.GetUnixFileMode(path);
                return (mode & (UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute)) != 0;
            }
            catch 
            {
                return false;
            }
        }
    }

    public class DirectoryManipulation
    {
        public static void FromPath(string newDirectory)
        {
            try
            {
                Directory.SetCurrentDirectory(newDirectory);
            }
            catch
            {
                Console.WriteLine($"cd: {newDirectory}: No such file or directory");
            }
        }

        public static string GetHomeDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }
















    
}