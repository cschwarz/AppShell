using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.ServiceDispatcher
{
    [Service("sampleService")]
    public interface ISampleService
    {
        event EventHandler<DateTime> CurrentTime;

        [ServiceMethod("add")]
        int Add(int value1, int value2);
    }
}
