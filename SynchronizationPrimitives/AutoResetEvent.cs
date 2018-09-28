using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationPrimitives
{
    class AutoResetEvent
    {
        private Spinlock _signal = new Spinlock();

        public AutoResetEvent(bool signal = true)
        {
            if (signal)
                this.Set();
            else
                this.Reset();
        }

        public void Set()
        {
            _signal.Exit();
        }

        public void Reset()
        {
            _signal.Set();
        }

        public void WaitOne()
        {
            _signal.Enter();
        }
    }
}
