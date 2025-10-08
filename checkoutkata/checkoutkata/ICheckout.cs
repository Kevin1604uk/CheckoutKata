using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkoutkata
{
    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }
}
