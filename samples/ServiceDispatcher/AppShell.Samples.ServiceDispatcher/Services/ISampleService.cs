using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.ServiceDispatcher
{
    public class DateTimeEventArgs : EventArgs
    {
        public DateTime DateTime { get; private set; }

        public DateTimeEventArgs(DateTime dateTime)
        {
            DateTime = dateTime;
        }
    }

    [Service("sampleService")]
    public interface ISampleService
    {
        event EventHandler<DateTimeEventArgs> CurrentTime;

        [ServiceMethod("add")]
        int Add(int value1, int value2);
    }
}
