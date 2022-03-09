using SDS.Application.Models;
using SDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(OrderCreateRequestViewModel model, int currentUserId);
        Task<Order> UpdateOrder(OrderUpdateRequestViewModel model);
        Task<IEnumerable<Order>> GetOrders();
        Task<IEnumerable<Order>> GetSalesManagerOrders(int currentUserId);
        Task<Order> GetOrder(int id);
        Task<Order> GetOrderByOrderNo(string id);
        Task<bool> DeleteOrder(int id);
        Task<IEnumerable<Order>> ApproveOrders(IEnumerable<Order> orders);
        Task<IEnumerable<Order>> RejectOrders(IEnumerable<Order> orders);
    }
}
