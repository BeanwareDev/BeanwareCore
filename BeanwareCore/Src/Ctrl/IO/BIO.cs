using BeanwareCore.Src.Data.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BeanwareCore.Src.Ctrl.IO
{
    /// <summary>
    /// Provides static utility methods for io operations
    /// </summary>
    public static class BIO
    {
        // File Methods
        /// <summary>
        /// Generates the sha1 hash value for the given file
        /// </summary>
        /// <param name="file">The absolute path of the file to generate the hash for</param>
        /// <returns>The file hash as a string</returns>
        public static string GenerateFileHash(string file)
        {
            StringBuilder formatted;

            using (FileStream fs = new FileStream(file, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }
                }
            }

            return formatted?.ToString() ?? null;
        }
        /// <summary>
        /// Returns the non-zero-based n-th line of text from a text file
        /// </summary>
        /// <param name="file">The absolute path of the file to read</param>
        /// <param name="line">The non-zero-based number of the line to read</param>
        /// <returns></returns>
        public static string GetNthLineFromFile(string file, int line)
        {
            return File.ReadLines(file).Skip(line - 1).Take(1).First();
        }
        /// <summary>
        /// Returns the given number of lines starting at the given non-zero-based index
        /// </summary>
        /// <param name="file">The absolute path of the file to read</param>
        /// <param name="startIndex">The non-zero-based number of the line to start reading</param>
        /// <param name="lineCount">The amount of lines to read starting at the startIndex</param>
        /// <returns></returns>
        public static string[] GetLinesFromFile(string file, int startIndex, int lineCount)
        {
            if (lineCount < 0)
                throw new BParameterException(nameof(lineCount), nameof(GetLinesFromFile), nameof(BIO), "was lower or equal to 0");

            var lines = File.ReadLines(file).Skip(startIndex - 1);
            lines = lines.Take(lineCount);
            return lines.ToArray();
        }

        // Directory Methods
        /// <summary>
        /// Delets all files and subdirectories from the given directory
        /// </summary>
        /// <param name="folder">The root directory in which to delete all files</param>
        /// <param name="includeRoot">Wether to also delete the root folder or not</param>
        public static void DeleteFolderContents(string folder, bool includeRoot)
        {
            foreach (string file in Directory.EnumerateFiles(folder))
                File.Delete(file);

            foreach (string dir in Directory.EnumerateDirectories(folder))
                DeleteFolderContents(dir, false);

            if (!includeRoot)
                Directory.Delete(folder);
        }
        /// <summary>
        /// Copies the entire content of the given directory to the given other directory
        /// </summary>
        /// <param name="originFolder">The directory to copy</param>
        /// <param name="targetFolder">The target directory</param>
        public static void CopyFolderContents(string originFolder, string targetFolder)
        {
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            foreach (string file in Directory.EnumerateFiles(originFolder))
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(targetFolder, name);
                File.Copy(file, dest);
            }

            foreach (string folder in Directory.EnumerateDirectories(originFolder))
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(targetFolder, name);
                CopyFolderContents(folder, dest);
            }
        }

        // Zip Methods
        /// <summary>
        /// Checks if the given password is correcty for the given encrypted zip file
        /// </summary>
        /// <param name="zipFile">The location of the zip file</param>
        /// <param name="password">The password to check</param>
        /// <returns>true if the password is corrct</returns>
        public static bool CheckZipPassword(string zipFile, string password)
        {
            return Ionic.Zip.ZipFile.CheckZipPassword(zipFile, password);
        }
        /// <summary>
        /// Unpacks the given encrypted zip file to the given directory
        /// </summary>
        /// <param name="zipFile">The location of the zip file</param>
        /// <param name="targetDirectory">The target directory to unpack the zip to</param>
        /// <param name="password">The password</param>
        public static void UnpackEncryptedZipFile(string zipFile, string targetDirectory, string password)
        {
            using (var zip = new Ionic.Zip.ZipFile(zipFile)
            {
                Password = password,
                Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes128
            })
            {
                zip.ExtractAll(targetDirectory, Ionic.Zip.ExtractExistingFileAction.Throw);
            }
        }
        /// <summary>
        /// Unpacks the given zip file to the given directory
        /// </summary>
        /// <param name="zipFile">The location of the zip file</param>
        /// <param name="targetDirectory">The target directory to unpack the zip to</param>
        public static void UnpackZipFile(string zipFile, string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            System.IO.Compression.ZipFile.ExtractToDirectory(zipFile, targetDirectory);
        }
        /// <summary>
        /// Packs the contents of the given directory into an encrypted zip file and optionally deletes the origin directory afterwards
        /// </summary>
        /// <param name="originDirectory">The directory to pack</param>
        /// <param name="targetZipFile">The target location of the packed file</param>
        /// <param name="password">The password to use for encryption</param>
        /// /// <param name="deleteFolderAfterwards">Wether to delete the origin directory afterwards or not</param>
        public static void PackEncryptedZipFile(string originDirectory, string targetZipFile, string password, bool deleteFolderAfterwards)
        {
            using (var zip = new Ionic.Zip.ZipFile(targetZipFile)
            {
                Password = password,
                Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes128
            })
            {
                zip.AddDirectory(originDirectory, "");
                zip.Save();
            }
        }
        /// <summary>
        /// Packs the contents of the given directory into a zip file and optionally deletes the origin directory afterwards
        /// </summary>
        /// <param name="originDirectory">The directory to pack</param>
        /// <param name="targetZipFile">The target location of the packed file</param>
        /// <param name="deleteFolderAfterwards">Wether to delete the origin directory afterwards or not</param>
        public static void PackZipFile(string originDirectory, string targetZipFile, bool deleteFolderAfterwards)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(originDirectory, targetZipFile);
            if (deleteFolderAfterwards)
                DeleteFolderContents(originDirectory, true);
        }

        // Serialization Methods
        /// <summary>
        /// Performs binary serialization of the given file and stores it in the given location
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="path">The absolute path to save the file to</param>
        public static void SerializeBinary(object toSerialize, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, toSerialize);
            }
        }
        /// <summary>
        /// Reads the file at the given location and deserializes it to the given type
        /// </summary>
        /// <typeparam name="T">The type to deserilize the file to</typeparam>
        /// <param name="path">The abolute path of the file to deserialize</param>
        /// <returns>The deserialized object</returns>
        public static T DeserializeBinary<T>(string path) where T : class
        {
            T obj;

            using (var fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(fs) as T;
            }

            return obj;
        }
        
        // Network Methods
        /// <summary>
        /// Downloads the file represented by the given url to the target location with an option for percentage based progress callbacks
        /// </summary>
        /// <param name="downloadUrl">The url of the file to download</param>
        /// <param name="targetFile">The path of the file to store the downloaded content as</param>
        /// <param name="progressCallback">The percentage based progress callback</param>
        /// <returns>true if the download was succesful</returns>
        public static async Task<bool> DownloadFile(string downloadUrl, string targetFile, Action<int> progressCallback = null)
        {
            WebClient client = null;
            var result = true;

            try
            {
                client = new WebClient();
                client.DownloadProgressChanged += (s, e) => progressCallback?.Invoke(e.ProgressPercentage);
                await client.DownloadFileTaskAsync(new Uri(downloadUrl), targetFile);
            }
            catch
            {
                result = false;
            }
            finally
            {
                client?.Dispose();
            }

            return result;
        }
    }
}
