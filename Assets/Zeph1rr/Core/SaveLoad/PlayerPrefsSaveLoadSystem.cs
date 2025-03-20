using UnityEngine;

namespace Zeph1rr.Core.SaveLoad
{
    public class PlayerPrefsSaveLoadSystem : SaveLoadSystem
    {
        public override void Save<T>(string key, T data)
        {
            string json = JsonUtility.ToJson(data, true);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public override T Load<T>(string key, T defaultData)
        {
            string json = PlayerPrefs.GetString(key, string.Empty);
            if (string.IsNullOrEmpty(json)) 
                return defaultData;
            return JsonUtility.FromJson<T>(json);
        }
    }
}