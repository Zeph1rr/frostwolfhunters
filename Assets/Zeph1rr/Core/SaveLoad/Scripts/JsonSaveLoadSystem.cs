using System.IO;
using UnityEngine;

namespace Zeph1rr.Core.SaveLoad
{
    public class JsonSaveLoadSystem<T, C> : SaveLoadSystem<T, C> where C: ISerializableData<T>
    {
        public JsonSaveLoadSystem(string saveDirectory) : base(saveDirectory)
        {
        }

        public override T Load(string saveFileName, T defaultData)
        {
            string saveFilePath = Path.Combine(SaveDirectory, $"{saveFileName}.json");
            if (!File.Exists(saveFilePath)) {
                Debug.LogWarning("Save file not found! Returning default data");
                return defaultData;
            }
            string json = File.ReadAllText(saveFilePath);
            C data = JsonUtility.FromJson<C>(json);
            return data.Deserialize();
        }

        public override void Save(ISerializableData<T> data, string saveFileName)
        {
            CreateSaveDirectory();
            string saveFilePath = Path.Combine(SaveDirectory, $"{saveFileName}.json");

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            
            Debug.Log($"Saved to {saveFilePath}");
        }
    }
}

