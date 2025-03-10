using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeph1rr.Core.Monos
{
    public abstract class MonoInterlayerLoop<T> : MonoInterlayer<T>
    {
        protected Action loop;
        protected Action fixedLoop;
        public virtual void SetLoop(Action loop, Action fixedLoop)
        {
            this.loop = loop;
            this.fixedLoop = fixedLoop; 
        }

        protected virtual void Update()
        {
            loop?.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            fixedLoop?.Invoke();
        }    
    }
}
