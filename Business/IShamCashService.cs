using Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
     public interface IShamCashService
    {
        Task<Payment> InitiatePaymentAsync(Payment request);
    }
}
