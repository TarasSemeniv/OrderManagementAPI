using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.DTOs.Outputs;
using OrderManagementCore.Interfaces;
using OrderManagementEntity.Models;
using OrderManagementStorage;

namespace OrderManagementCore.Services;

public class OrderService(OrderContext context) : IOrderService
{
    private readonly OrderContext _context = context;
    public IEnumerable<OrderDto> GetOrders()
    {
        return _context.Orders
            .Include(o => o.Customer)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                CustomerId = o.Customer.Id
            });
    }

    public OrderDto GetOrderById(int id)
    {
        var order = _context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new NullReferenceException("Order not found");
        var orderDto = new OrderDto()
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            CustomerId = order.Customer?.Id
        };
        return orderDto;
    }

    public OrderDto AddOrder(CreateOrderDto createOrderDto)
    {
        var order = new Order()
        {
            OrderDate = DateTime.Now,
            TotalAmount = createOrderDto.TotalAmount, 
        };
        if (createOrderDto.CustomerId.HasValue)
        {
            order.Customer = _context.Customers.Find(createOrderDto.CustomerId);
        }
        
        _context.Orders.Add(order);
        _context.SaveChanges();
        var orderDto = new OrderDto()
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            CustomerId = order.Customer?.Id 
        };
        
        return orderDto;
    }
}