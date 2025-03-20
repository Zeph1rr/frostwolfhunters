using System;

namespace Zeph1rr.Core.Monos
{
    public abstract class MonoInterlayerLoop<T> : MonoInterlayer<T>
    {
        private Action _loop;
        private Action _fixedLoop;
        public virtual void SetLoop(Action loop, Action fixedLoop)
        {
            this._loop = loop;
            this._fixedLoop = fixedLoop; 
        }

        protected virtual void Update()
        {
            _loop?.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            _fixedLoop?.Invoke();
        }    
    }
}
