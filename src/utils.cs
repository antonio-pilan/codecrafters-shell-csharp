using System.Reflection.Metadata.Ecma335;

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
                return fullPath;
            }

            string exePath = fullPath + ".exe";
            if (File.Exists(exePath))
            {
                return exePath;
            }

            return null;
        }
    }
}