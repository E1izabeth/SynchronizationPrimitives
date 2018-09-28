using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationPrimitives
{
    class MonitorExample
    {

        public static void DoWork()
        {
            IMonitor sync = new MyMonitor();
            var data = new Data();
            Thread t = new Thread(new WaitingThread(sync, data).run);
            t.Start();
            try
            {
                Console.WriteLine("main::Sleeping");
                Thread.Sleep(5000);
            }
            catch (ThreadInterruptedException ex)
            {
                Console.WriteLine("main::Interrupted: " + ex.Message);
            }
            sync.Capture();
            Console.WriteLine("main::setting value to 1");
            data.value = 1;
            Console.WriteLine("main::notifying thread");
            sync.Notify();
            Console.WriteLine("main::Thread notified");
            sync.Release();

            t.Join();
            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        class Data
        {
            public int value = 0;
        }

        class WaitingThread 
        {
            private IMonitor sync;
            private Data data;

            public WaitingThread(IMonitor sync, Data data)
            {
                this.sync = sync;
                this.data = data;
            }

            public void run()
            {
                Console.WriteLine("own:: Thread started");
                sync.Capture();
                if (data.value == 0)
                {
                    try
                    {
                        Console.WriteLine("own:: Waiting");
                        sync.Wait();
                        Console.WriteLine("own:: Running again");
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Console.WriteLine("own:: Interrupted: " + ex.Message);
                    }
                }
                Console.WriteLine("own:: data.value = " + data.value);
                sync.Release();
            }
        }
    }
}
