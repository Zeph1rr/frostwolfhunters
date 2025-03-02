namespace Zeph1rr.Core.SaveLoad
{
    public interface ISerializableData<T>
    {
        public abstract T Deserialize();
    }
}
