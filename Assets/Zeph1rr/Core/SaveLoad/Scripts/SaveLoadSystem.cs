using UnityEngine;
using System.IO;

namespace Zeph1rr.Core.SaveLoad
{
    public abstract class SaveLoadSystem<T>
    {
        public string SaveDirectory {get; private set;}

        public SaveLoadSystem(string saveDirectory)
        {
            SaveDirectory = saveDirectory;
        }

        public void CreateSaveDirectory() {
            if (!Directory.Exists(SaveDirectory)) {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        public bool IsSaveExists(string playerName)
        {
            return File.Exists(Path.Combine(SaveDirectory, $"{playerName}.save"));
        }

        public abstract void Save(T data, string saveFileName);

        public abstract T Load(string saveFileName, T defaultData);

        public string[] GetSaveFiles()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Debug.LogWarning("Save directory not found!");
                return new string[0];
            }

            return Directory.GetFiles(SaveDirectory, "*.save");
        }

        public string GetSaveFileLastWriteTime(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.GetLastWriteTime(filePath).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                Debug.LogWarning($"File not found: {filePath}");
                return "File not found";
            }
        }

        public string GetSaveFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }
    }
}

