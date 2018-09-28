using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationPrimitives
{
    public struct SpinLockInstance
    {
        public int isLocked;
    }

    public class Spinlock
    {
        private SpinLockInstance _isLocked;

        public Spinlock()
        {
            _isLocked.isLocked = 0;
        }

        public void Enter()
        {
            bool locked = false;
            Enter(ref _isLocked, ref locked);
        }

        public void Enter(ref bool locked)
        {
            Enter(ref _isLocked, ref locked);
        }

        public void Exit()
        {
            Exit(ref _isLocked);
        }

        public void Set()
        {
            SetBusy(ref _isLocked);
        }

        private static void SetBusy(ref SpinLockInstance lockVar)
        {
            Interlocked.Exchange(ref lockVar.isLocked, 1);
        }

        private static void Enter(ref SpinLockInstance lockVar, ref bool locked)
        {
            locked = false;

            while (1 == Interlocked.Exchange(ref lockVar.isLocked, 1))
            {
            }

            locked = true;
        }

        private static void Exit(ref SpinLockInstance lockVar)
        {
            if (lockVar.isLocked == 1)
            {
                Interlocked.Exchange(ref lockVar.isLocked, 0);
            }
        }
    }
}
