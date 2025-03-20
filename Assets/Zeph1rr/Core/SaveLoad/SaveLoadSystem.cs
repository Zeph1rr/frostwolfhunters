namespace Zeph1rr.Core.SaveLoad
{
    public abstract class SaveLoadSystem
    {
        public abstract void Save<T>(string key, T data);
        public abstract T Load<T>(string key, T defaultData);
    }
}