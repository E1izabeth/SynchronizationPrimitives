using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace SynchronizationPrimitives
{

    class Program
    {
        static void Main(string[] args)
        {
            AutoResetEventExample.DoWork();

            /*
            //MonitorExample.DoWork();
            // AutoResetEventExample.DoWork();

            // var obj = new object();
            var ev1 = new System.Threading.AutoResetEvent(false);
            var ev2 = new System.Threading.AutoResetEvent(false);

            var th = new Thread(() => {
                for (int j = 0; ; j++)
                {
                    Console.WriteLine("waiting");
                    ev1.WaitOne();

                    Console.WriteLine("continued");
                    ev2.Set();
                }
            }) {
                IsBackground = true
            };
            th.Start();


            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("setting");
                ev1.Set();
                ev2.WaitOne();
            }

            Console.ReadKey();

    */
        }
    }
}
