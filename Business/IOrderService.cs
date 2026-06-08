using Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IOrderService
    {
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        void UpdatePaymentInfo(int orderheaderId, Payment payment);
    }
}
