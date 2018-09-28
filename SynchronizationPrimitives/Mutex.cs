using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationPrimitives
{
    public class Mutex
    {
        private readonly AutoResetEvent _releaseSignal = new AutoResetEvent(false);
        private readonly Spinlock _lock = new Spinlock();

        private volatile int _ownerThreadId;
        private volatile int _enterCounter;

        public Mutex()
        {
            _enterCounter = 0;
            _ownerThreadId = -1;
        }

        public bool ReleaseMutex()
        {
            _lock.Enter();

            bool released;

            if (_ownerThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                _enterCounter--;

                if (_enterCounter == 0)
                {
                    _ownerThreadId = -1;

                    _releaseSignal.Set();
                }

                released = true;
            }
            else
            {
                released = false;
            }

            _lock.Exit();
            return released;
        }

        public void WaitOne()
        {
            _lock.Enter();

            if (_ownerThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                _enterCounter++;

                _lock.Exit();
            }
            else
            {
                while (_ownerThreadId != -1)
                {
                    _lock.Exit();

                    _releaseSignal.WaitOne();

                    _lock.Enter();
                }

                _ownerThreadId = Thread.CurrentThread.ManagedThreadId;
                _enterCounter++;

                _lock.Exit();
            }
        }
    }
}
