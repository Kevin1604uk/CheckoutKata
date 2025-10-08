using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkoutkata
{
    static public class ConsoleLogger
    {
        public static void LogError(string message) => Console.Error.WriteLine($"[ERROR] {message}");

    }
}
