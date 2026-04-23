
using System.ComponentModel;
using System.Reflection;

namespace Csp.Slide.Tools
{
    /// <summary>
    /// string 扩展
    /// </summary>
    public static class StringExtensions
    {
        public static string GetDescription(this FieldInfo property)
        {
            var attribute = property.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? string.Empty;
        }

        public static bool ArePathsSame(this string path,string path2)
        {
            string fullPath1 = Path.GetFullPath(path);
            string fullPath2 = Path.GetFullPath(path2);
            return string.Equals(fullPath1, fullPath2, StringComparison.OrdinalIgnoreCase);
        }


        public static bool IsFolder(this string path)
        {
            if (Directory.Exists(path))
            {
                //Console.WriteLine("路径是文件夹。");
                return true;
            }
            else if (File.Exists(path))
            {
                //Console.WriteLine("路径是文件。");
                return false;
            }
            else
            {
                //Console.WriteLine("路径不存在。");
                return false;
            }
        }

        public static IEnumerable<FileInfo> GetFiles(this string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return GetFiles(directory);
        }

        private static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                yield return file;
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                foreach (FileInfo file in GetFiles(subDirectory))
                {
                    yield return file;
                }
            }
        }

        public static Tuple<string, string>[] RenameSameNameFiles(this string directoryPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                                            .Where(f => f.ToLower().EndsWith(".sdpc") ||
                                            f.ToLower().EndsWith(".dcmz") ||
                                            f.ToLower().EndsWith(".dcm")).ToArray();

            // 获取所有文件的同名集合
            //string[] fileNamesWithoutExtensions = files.Select(f => Path.GetFileNameWithoutExtension(f)).Distinct().ToArray();
            string[] fileNamesWithoutExtensions = files.Select(f => Path.GetFileNameWithoutExtension(f)).GroupBy(f => f)
                                                       .Where(f => f.Count() > 1).Select(f => f.Key).ToArray();

            // 查找不同名文件列表
            var difFiles = files.Where(f =>
            {
                string nameWithoutExtension = Path.GetFileNameWithoutExtension(f);
                return !fileNamesWithoutExtensions.Contains(nameWithoutExtension);
            }).Select(f => new Tuple<string, string>(f, f));

            // 查找同名文件列表
            var i = 1;
            var sameFiles = files.Where(f =>
            {
                string nameWithoutExtension = Path.GetFileNameWithoutExtension(f);
                return fileNamesWithoutExtensions.Contains(nameWithoutExtension);
            }).Select(f => new Tuple<string, string>(f, Path.Combine(Path.GetDirectoryName(f)??"", $"{Path.GetFileNameWithoutExtension(f)}_{i++}{Path.GetExtension(f)}")));

            return difFiles.Union(sameFiles).ToArray();
        }
    }
}
