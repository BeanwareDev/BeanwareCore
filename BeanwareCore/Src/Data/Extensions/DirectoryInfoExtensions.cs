using System.IO;

namespace BeanwareCore.Src.Data.Extensions
{
    public static class DirectoryInfoExtensions
    {
        /// <summary> Deletes all files and directories inside the given directory </summary>
        public static void Empty(this DirectoryInfo directory)
        {
            if (!directory.Exists)
                return;

            foreach (FileInfo file in directory.GetFiles())
                file.Delete();

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                subDirectory.Delete(true);
        }
    }
}
