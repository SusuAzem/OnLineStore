using Core;

using Data.IRepository;

using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class OrderService: IOrderService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
                this.unitOfWork = unitOfWork;
        }
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            var paymentFromDb = unitOfWork.Payment.GetFirstOrDefault(u => u.OrderHeaderId == id);
            if (orderFromDb != null && paymentStatus != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                paymentFromDb.Status = paymentStatus;
            }
        }

        public void UpdatePaymentInfo(int orderheaderId, Payment payment)
        {
            var orderFromDb = unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderheaderId);
            if (orderFromDb != null && payment != null)
            {
                var paymentFromDb = unitOfWork.Payment.GetFirstOrDefault(u => u.OrderHeaderId == orderheaderId);
                paymentFromDb.Id = payment.Id;
                paymentFromDb.Date = payment.Date!;
                paymentFromDb.Status = payment.Status;
                paymentFromDb.Fee = payment.Fee;
            }
        }
    }
}
