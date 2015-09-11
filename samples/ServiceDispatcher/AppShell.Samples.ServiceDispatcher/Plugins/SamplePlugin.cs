using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.ServiceDispatcher
{
    public class SamplePlugin : ServicePlugin<ISampleService>, ISampleService
    {
        private int counter;

        public SamplePlugin(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            RetrieveCurrentTime();
        }

        private void RetrieveCurrentTime()
        {
            if (CurrentTime != null)
                CurrentTime(this, new DateTimeEventArgs(DateTime.Now));

            if (CounterIncreased != null)
                CounterIncreased(this, new CounterEventArgs(counter++));

            Task.Delay(1000).ContinueWith(t => RetrieveCurrentTime());
        }

        public event EventHandler<DateTimeEventArgs> CurrentTime;
        public event EventHandler<CounterEventArgs> CounterIncreased;

        public int Add(int value1, int value2)
        {
            return value1 + value2;
        }
    }
}
