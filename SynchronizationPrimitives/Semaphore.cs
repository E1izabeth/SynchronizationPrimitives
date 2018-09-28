using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationPrimitives
{
    public class Semaphore
    {
        private int _max;
        private int _min;
        private int _counter;
        private AutoResetEvent _guard;
        private Spinlock _spinlock;


        public Semaphore(int max, int min = 0)
        {
            _counter = max;
            _max = max;
            _min = min;
            _guard = new AutoResetEvent();
            _spinlock = new Spinlock();
        }

        public void WaitOne()
        {
            _spinlock.Enter();
        
            if (_counter < _max)
            {
                _counter++;

                _spinlock.Exit();
            }
            else
            {

                _spinlock.Exit();
                _guard.WaitOne();
                while (_counter == _max)
                {

                }
                _counter++;
            }
            
        }

        public int Release()
        {
            _spinlock.Enter();
            var counter = _counter;
            if (_counter < _min)
            {
                throw new InvalidOperationException("Semaphore full");
            }
            --_counter;

            _spinlock.Exit();
            _guard.Set();
            return counter;

        }

        public int Release(int count)
        {
            var ret = _counter;

            for (int i = 0; i < count; i++)
            {
                this.Release();
            }

            return ret; 
        }
    }
}
