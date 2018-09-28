using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationPrimitives
{

    class ManualResetEvent
    {
        private Spinlock _lock = new Spinlock();
        private Spinlock _signal = new Spinlock();
        private volatile bool _isSet;

        public ManualResetEvent(bool signal)
        {
            if (signal)
                this.Set();
            else
                this.Reset();
        }

        public void Set()
        {
            _lock.Enter();

            _isSet = true;
            _signal.Exit();

            _lock.Exit();
        }

        public void Reset()
        {
            _lock.Enter();

            _signal.Set();
            _isSet = false;

            _lock.Exit();
        }

        public void WaitOne()
        {
            _signal.Enter();

            _lock.Enter();

            if (_isSet)
                _signal.Exit();

            _lock.Exit();
        }
    }

    //class ManualResetEvent
    //{
    //    private volatile bool _signal;
    //    //private Spinlock _spinlock;
    //    //private bool _splocked;

    //    public ManualResetEvent(bool signal = false)
    //    {
    //        _signal = signal;
    //        //_splocked = false;
    //        //_spinlock = new Spinlock();
    //    }

    //    public void Set()
    //    {
    //        _signal = true;
    //    }

    //    public void Reset()
    //    {
    //        _signal = false;
    //    }

    //    public void WaitOne()
    //    {
    //        // _spinlock.Enter(ref this._splocked);
    //        while (!this._signal)
    //        {
    //            //do nothing
    //        }
    //        // _spinlock.Exit();
    //    }
    //}
}
