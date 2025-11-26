using System.Reflection.Metadata.Ecma335;

namespace Utils
{
    public class PathVariables
    {
        public static string GetPath(string command)
        {
            string? systemPath = Environment.GetEnvironmentVariable("Path");

            List<string> pathList = new List<string>();
            if (!string.IsNullOrEmpty(systemPath))
            {
                string[] paths = systemPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string path in paths)
                {   
                    string pathLower = path.ToLower();
                    string exe = GetExecutable(pathLower, command);
                    //Console.WriteLine(exe);
                    if (exe != null)
                    {
                        return exe;
                    }
                }
            }
            return null;
        }

        public static string GetExecutable(string fullPath, string executable)
        {
            if (string.IsNullOrEmpty(fullPath)) return null;
            DirectoryInfo? currentDir = new DirectoryInfo(fullPath);

            executable = executable + ".exe";
            
            while (currentDir != null)
            {
                string candidatePath = Path.Combine(currentDir.FullName, executable);
                if (File.Exists(candidatePath))
                {
                    return candidatePath;
                }
                currentDir = currentDir.Parent;
            }
            return null;
        }
    }
}