using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SDS.Application.Interfaces;
using SDS.Application.Models;
using SDS.Domain.Entities;
using System.Security.Claims;

namespace SDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "OrderGeneration,OrderApproval")]
        [HttpGet("getorders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }

        [Authorize(Roles = "OrderGeneration,OrderApproval")]
        [HttpPost("getorder")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);
            return Ok(order);
        }

        [Authorize(Roles = "OrderGeneration,OrderApproval")]
        [HttpGet("getorderbyorderno/{id}")]
        public async Task<ActionResult<Order>> GetOrderByOrderNo(string id)
        {
            var order = await _orderService.GetOrderByOrderNo(id);
            return Ok(order);
        }

        [Authorize(Roles = "OrderGeneration")]
        [HttpPost("createorder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderCreateRequestViewModel order)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber).Value);

            var addedOrder = await _orderService.CreateOrder(order, currentUserId);
            return Ok(addedOrder);
        }

        [Authorize(Roles = "OrderGeneration")]
        [HttpPost("updateorder")]
        public async Task<ActionResult<Order>> UpdateOrder(OrderUpdateRequestViewModel order)
        {
            var updatedOrder = await _orderService.UpdateOrder(order);
            return Ok(updatedOrder);
        }

        [Authorize(Roles = "OrderGeneration")]
        [HttpDelete("deleteorder/{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var deletedOrder = await _orderService.DeleteOrder(id);
            return Ok(NoContent);
        }

        [Authorize(Roles = "OrderApproval")]
        [HttpGet("getsalesmanagerorders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetSalesManagerOrders()
        {
            var currentUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber).Value);
            var orders = await _orderService.GetSalesManagerOrders(currentUserId);
            return Ok(orders);
        }

        [Authorize(Roles = "OrderApproval")]
        [HttpPost("approveorders")]
        public async Task<ActionResult> ApproveOrders(IEnumerable<Order> orders)
        {
            var approvedOrders = await _orderService.ApproveOrders(orders);
            return Ok(approvedOrders);
        }

        [Authorize(Roles = "OrderApproval")]
        [HttpPost("rejectorders")]
        public async Task<ActionResult> RejectOrders(IEnumerable<Order> orders)
        {
            var approvedOrders = await _orderService.RejectOrders(orders);
            return Ok(approvedOrders);
        }
    }
}
