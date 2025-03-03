using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Zeph1rr.Core.SaveLoad
{
    public class BinarySaveLoadSystem<T, C> : SaveLoadSystem<T, C> where C: ISerializableData<T>
    {
        public BinarySaveLoadSystem(string saveDirectory) : base(saveDirectory)
        {
        }

        public override T Load(string saveFileName, T defaultData)
        {
            string saveFilePath = Path.Combine(SaveDirectory, saveFileName);
            if (!File.Exists(saveFilePath)) {
                Debug.LogWarning("Save file not found! Returning default game data");
                return defaultData;
            }

            using (FileStream fileStream = new(saveFilePath, FileMode.Open))
            {
                BinaryFormatter formatter = new();
                ISerializableData<T> gameData = (ISerializableData<T>)formatter.Deserialize(fileStream);
                return gameData.Deserialize();
            }
        }

        public override void Save(ISerializableData<T> data, string saveFileName)
        {
            CreateSaveDirectory();
            string saveFilePath = Path.Combine(SaveDirectory, $"{saveFileName}.save");

            using (FileStream fileStream = new(saveFilePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new();
                formatter.Serialize(fileStream, data);
            }
            
            Debug.Log($"Saved to {saveFilePath}");
        }
    }

}
