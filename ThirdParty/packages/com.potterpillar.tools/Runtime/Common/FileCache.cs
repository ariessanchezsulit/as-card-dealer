#region Header

// FileCache.cs
// Aries Sanchez Sulit
// 2022-11-17 at 11:38 PM

#endregion

using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace PotterPillar.Tools.Common
{
    public class FileCache
    {
        private string BasePath { get; }

        public FileCache(string name)
        {
            BasePath = Path.Combine(Application.persistentDataPath, name);
        }

        public string WriteToFile<T>(string filePath, T obj)
        {
            var path     = Path.Combine(BasePath, filePath);
            var contents = JsonConvert.SerializeObject(obj); // TODO: parameterize serialization logic
            WriteAllText(path, contents);
            return path;
        }

        protected void EnsureDirectoryExists(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        protected void WriteAllText(string filePath, string contents)
        {
            var directory = Path.GetDirectoryName(filePath);
            EnsureDirectoryExists(directory);
            File.WriteAllText(filePath, contents);
        }
    }
}
