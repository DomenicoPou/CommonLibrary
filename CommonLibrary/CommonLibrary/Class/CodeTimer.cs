using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Class
{
    public class CodeTimer : IDisposable
    {
        private readonly Stopwatch stopWatch;
        private readonly Action<TimeSpan> action;

        public CodeTimer(Action<TimeSpan> _action)
        {
            this.action = _action;
            stopWatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            Console.WriteLine("Ahh");
            stopWatch.Stop();
            action(stopWatch.Elapsed);
        }
    }
}
