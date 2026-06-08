using Business;

using Core;

using Data.IRepository;

using static Business.StringDefault;

using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderService orderService;

        public OrderController(IUnitOfWork unitOfWork, IOrderService orderService)
        {
            this.unitOfWork = unitOfWork;
            this.orderService = orderService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<OrderHeader> GetOrder(int id)
        {
            try
            {
                var result = unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id);

                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("api/order/savepayment")]
        public async Task<IActionResult> SavePayment([FromBody] Payment payment)
        {
            try
            {
                if (payment == null)
                    return BadRequest();

                var order = unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == payment.Id);
                orderService.UpdatePaymentInfo(order.Id, payment);
                await unitOfWork.Save();
                //return StatusCode(StatusCodes.Status201Created);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating Order record");
            }
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var orderHeaders = unitOfWork.OrderHeader.GetAll(includeProperties: "User, Payment");
            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.Payment!.Status == PaymentPending);
                    break;
                case "paid":
                    orderHeaders = orderHeaders.Where(u => u.Payment!.Status == PaymentPaid);
                    break;
                case "failed":
                    orderHeaders = orderHeaders.Where(u => u.Payment!.Status == PaymentRejected);
                    break;
                case "refunded":
                    orderHeaders = orderHeaders.Where(u => u.Payment!.Status == PaymentRefunded);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == OrderInProcess);
                    break;
                case "shipped":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == OrderShipped);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == OrderCompleted);
                    break;
                case "Cancelled":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == OrderCancelled);
                    break;
                default:
                    break;
            }
            var list = orderHeaders.Select(o => new
            {
                id = o.Id,
                name = o.User!.Name,
                orderStatus = o.OrderStatus,
                orderTotal = o.OrderTotal
            }).ToList();
            return new JsonResult(new { data = list });
        }

    }
}
