using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkoutkata
{
    public interface ILogger
    {
        event EventHandler<string>? LogWritten;
        void LogError(string message);
    }
}
