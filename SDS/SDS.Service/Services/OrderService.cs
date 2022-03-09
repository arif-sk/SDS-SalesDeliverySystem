using Caretaskr.Data;
using Caretaskr.Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using SDS.Application.Interfaces;
using SDS.Application.Models;
using SDS.Common.Utils;
using SDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static SDS.Domain.Enum.EnumExtensions;

namespace SDS.Service.Services
{
    public class OrderService : ParentService, IOrderService
    {
        private readonly ApplicationContext _applicationContext;
        public OrderService(IGenericUnitOfWork genericUnitOfWork, ApplicationContext applicationContext) :
            base(genericUnitOfWork)
        {
            _applicationContext = applicationContext;
        }

        public async Task<IEnumerable<Order>> ApproveOrders(IEnumerable<Order> models)
        {
            var selectedOrderIds = models.Select(m => m.Id).ToList();
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var orderApprovalChainRepo = _genericUnitOfWork.GetRepository<OrderApprovalChain>();
            var selectedOrders = await orderRepo.GetAll(x => selectedOrderIds.Contains(x.Id)).ToListAsync();

            var orders = new List<Order>();
            using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var or in selectedOrders)
                {
                    or.OrderStatus = OrderStatusEnum.Approved;
                    or.ApprovalNote = models.FirstOrDefault().ApprovalNote;
                    orderRepo.Update(or);
                    orders.Add(or);
                    var orderApprovalChain = await orderApprovalChainRepo.FirstOrDefault(x => x.Order.Id == or.Id && x.Level == 2);
                    if (orderApprovalChain != null)
                    {
                        orderApprovalChain.OrderStatus = OrderStatusEnum.Approved;
                        orderApprovalChain.Notes = or.ApprovalNote;
                        orderApprovalChainRepo.Update(orderApprovalChain);

                    }
                }
                await _genericUnitOfWork.SaveChangesAsync();
                tran.Complete();
            }
            return orders;
        }

        public async Task<IEnumerable<Order>> RejectOrders(IEnumerable<Order> models)
        {
            var selectedOrderIds = models.Select(m => m.Id).ToList();
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var orderApprovalChainRepo = _genericUnitOfWork.GetRepository<OrderApprovalChain>();
            var selectedOrders = await orderRepo.GetAll(x => selectedOrderIds.Contains(x.Id)).ToListAsync();

            var orders = new List<Order>();
            using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var or in selectedOrders)
                {
                    or.OrderStatus = OrderStatusEnum.Rejected;
                    or.ApprovalNote = models.FirstOrDefault().ApprovalNote;
                    orderRepo.Update(or);
                    orders.Add(or);
                    var orderApprovalChain = await orderApprovalChainRepo.FirstOrDefault(x => x.Order.Id == or.Id && x.Level == 2);
                    if (orderApprovalChain != null)
                    {
                        orderApprovalChain.OrderStatus = OrderStatusEnum.Rejected;
                        orderApprovalChain.Notes = or.ApprovalNote;
                        orderApprovalChainRepo.Add(orderApprovalChain);
                    }
                }
                await _genericUnitOfWork.SaveChangesAsync();
                tran.Complete();
            }
            return orders;
        }

        public async Task<Order> CreateOrder(OrderCreateRequestViewModel model, int currentUserId)
        {
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var productRepo = _genericUnitOfWork.GetRepository<Product>();
            var orderApprovalRepo = _genericUnitOfWork.GetRepository<OrderApprovalChain>();
            var appUserRepo = _genericUnitOfWork.GetRepository<AppUser>();
            var currentUser = appUserRepo.GetById(currentUserId);
            var orders = orderRepo.GetAll();
            var maxOrderNo = Convert.ToInt32(orders.Max(x => x.OrderNo.Substring(6)));
            var orderNo = NumberGenerator.GenerateOrderNo(maxOrderNo + 1, 6);
            var product = new Product();
            productRepo.Add(product);
            var order = new Order
            {
                Client = model.Client,
                WarrantyInformation = model.WarrantyInformation,
                ProductSpecification = model.ProductSpecification,
                OrderDate = model.OrderDate,
                DeliveryDate = model.DeliveryDate,
                Budget = model.Budget,
                OrderStatus = OrderStatusEnum.Open,
                OrderNo = orderNo,
                Product = product
            };
            orderRepo.Add(order);
            var oa2 = new OrderApprovalChain
            {
                Level = 2,
                Order = order,
                RoleGroupId = 2,
                OrderStatus = OrderStatusEnum.Pending,
                CreatedAt = DateTime.Now,
                AppUser = currentUser
            };
            orderApprovalRepo.Add(oa2);
            var oa3 = new OrderApprovalChain
            {
                Level = 3,
                Order = order,
                RoleGroupId = 3,
                OrderStatus = OrderStatusEnum.Due,
                CreatedAt = DateTime.Now,
                AppUser = currentUser
            };
            orderApprovalRepo.Add(oa3);
            var oa4 = new OrderApprovalChain
            {
                Level = 4,
                Order = order,
                RoleGroupId = 4,
                OrderStatus = OrderStatusEnum.Due,
                CreatedAt = DateTime.Now,
                AppUser = currentUser
            };
            orderApprovalRepo.Add(oa4);
            var oa5 = new OrderApprovalChain
            {
                Level = 5,
                Order = order,
                RoleGroupId = 5,
                OrderStatus = OrderStatusEnum.Due,
                CreatedAt = DateTime.Now,
                AppUser = currentUser
            };
            orderApprovalRepo.Add(oa5);
            var oa6 = new OrderApprovalChain
            {
                Level = 6,
                Order = order,
                RoleGroupId = 6,
                OrderStatus = OrderStatusEnum.Due,
                CreatedAt = DateTime.Now,
                AppUser = currentUser
            };
            orderApprovalRepo.Add(oa6);
            await _genericUnitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var order = orderRepo.GetById(id);
            order.IsDisabled = true;

            orderRepo.Update(order);
            await _genericUnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Order> GetOrder(int id)
        {
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var order = orderRepo.GetById(id);
            return order;
        }

        public async Task<Order> GetOrderByOrderNo(string id)
        {
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var order = await orderRepo.FirstOrDefault(x => x.OrderNo == id);
            return order;
        }


        public async Task<IEnumerable<Order>> GetOrders()
        {
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var orders = orderRepo.GetAll(x => x.IsDisabled == false);
            return orders;
        }

        public async Task<IEnumerable<Order>> GetSalesManagerOrders(int currentUserId)
        {
            var userRole = await (from ap in _applicationContext.Users
                                  join ur in _applicationContext.UserRoles on ap.Id equals ur.AppUser.Id
                                  join r in _applicationContext.Roles on ur.Role.Id equals r.Id
                                  where (ap.Id == currentUserId)
                                  select r).FirstAsync();

            return await (from ap in _applicationContext.OrderApprovalChains
                         join or in _applicationContext.Orders on ap.OrderId equals or.Id
                         where (ap.OrderStatus == OrderStatusEnum.Pending && ap.Level == userRole.Level)
                         select or).ToListAsync();
        }

        public async Task<Order> UpdateOrder(OrderUpdateRequestViewModel model)
        {
            var orderRepo = _genericUnitOfWork.GetRepository<Order>();
            var order = orderRepo.GetById(model.Id);
            order.OrderDate = model.OrderDate;
            order.DeliveryDate = model.DeliveryDate;
            order.Budget = model.Budget;
            order.Client = model.Client;
            order.ProductSpecification = model.ProductSpecification;
            order.WarrantyInformation = model.WarrantyInformation;
            orderRepo.Update(order);
            await _genericUnitOfWork.SaveChangesAsync();
            return order;
        }
    }
}
