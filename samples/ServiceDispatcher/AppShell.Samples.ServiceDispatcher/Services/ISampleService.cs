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

    public class CounterEventArgs : EventArgs
    {
        public int Counter { get; private set; }

        public CounterEventArgs(int counter)
        {
            Counter = counter;
        }
    }


    [Service("sampleService")]
    public interface ISampleService : IService
    {
        event EventHandler<DateTimeEventArgs> CurrentTime;
        event EventHandler<CounterEventArgs> CounterIncreased;

        [ServiceMethod("add")]
        int Add(int value1, int value2);
    }
}
