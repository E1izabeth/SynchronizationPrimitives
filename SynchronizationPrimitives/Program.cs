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
            MonitorExample.DoWork();
        }
    }
}
