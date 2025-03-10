namespace Zeph1rr.Core.Monos
{
    public abstract class MonoInterlayer<T>: Mono
    {
        public T ParentObject { get; protected set; }

        public virtual void SetParentSctipt(T parentObject)
        {
            ParentObject = parentObject;
        }
    }
}
