using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationPrimitives
{
    public interface IMonitor
    {
        void Capture();
        void Release();

        void Notify();
        void NotifyAll();

        bool Wait();
    }

    public class BuiltinMonitor : IMonitor
    {
        public BuiltinMonitor()
        {

        }

        public void Capture()
        {
            Monitor.Enter(this);
        }

        public void Release()
        {
            Monitor.Exit(this);
        }

        public bool Wait()
        {
            return Monitor.Wait(this);
        }

        public void Notify()
        {
            Monitor.Pulse(this);
        }

        public void NotifyAll()
        {
            Monitor.PulseAll(this);
        }
    }

    public class MyMonitor : IMonitor
    {
        private readonly ManualResetEvent _releaseSignal;
        private readonly ManualResetEvent _pulseSignal;

        private readonly Spinlock _lock;

        private volatile int _ownerThreadId;
        private volatile int _enterCounter;

        private volatile bool _isPulseHandled = false;
        private volatile bool _isMultiPulse = false;

        public MyMonitor()
        {
            _enterCounter = 0;
            _ownerThreadId = -1;
            _releaseSignal = new ManualResetEvent(false);
            _pulseSignal = new ManualResetEvent(false);
            _lock = new Spinlock();
        }

        public bool Wait()
        {
            _lock.Enter();
            try
            {
                if (_ownerThreadId == Thread.CurrentThread.ManagedThreadId)
                {
                    do
                    {
                        this.ReleaseImpl();
                        _lock.Exit();

                        _pulseSignal.WaitOne();

                        _lock.Enter();
                    }
                    while (_isMultiPulse || (_isPulseHandled && !_isMultiPulse));

                    _isPulseHandled = true;
                    this.CaptureImpl();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            finally
            {
                _lock.Exit();
            }
            return true;
        }

        public void Release()
        {
            _lock.Enter();
            this.ReleaseImpl();
            _lock.Exit();
        }

        private void ReleaseImpl()
        {
            if (_ownerThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                _enterCounter--;

                if (_enterCounter == 0)
                {
                    _ownerThreadId = -1;

                    _releaseSignal.Set();
                }

            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Capture()
        {
            _lock.Enter();
            this.CaptureImpl();
            _lock.Exit();
        }

        private void CaptureImpl()
        {
            if (_ownerThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                _enterCounter++;
            }
            else if (_ownerThreadId != -1)
            {
                while (_ownerThreadId != -1)
                {
                    _lock.Exit();

                    _releaseSignal.WaitOne();

                    _lock.Enter();
                }

                _ownerThreadId = Thread.CurrentThread.ManagedThreadId;
                _enterCounter++;
            }
            else
            {
                _ownerThreadId = Thread.CurrentThread.ManagedThreadId;
                _enterCounter++;
            }
        }

        public void Notify()
        {
            _lock.Enter();

            _isMultiPulse = false;
            _isPulseHandled = false;
            _pulseSignal.Set();

            _lock.Exit();
        }

        public void NotifyAll()
        {
            _lock.Enter();

            _isMultiPulse = true;
            _isPulseHandled = false;
            _pulseSignal.Set();

            _lock.Exit();
        }
    }
}
