using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.DTOs.Outputs;
using OrderManagementCore.Interfaces;
using OrderManagementEntity.Models;
using OrderManagementStorage;

namespace OrderManagementCore.Services;

public class OrderService(OrderContext context, IMapper mapper) : IOrderService
{
    private readonly OrderContext _context = context;
    private readonly IMapper _mapper = mapper;
    public async Task<IEnumerable<OrderDto>> GetOrdersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 0)   
            throw new ArgumentException("Page cannot be less than 0", nameof(page));
        if (pageSize <= 0)
            throw new ArgumentException("Page size cannot be less or equal 0", nameof(pageSize));
        return await _context.Orders
            .Select(o => _mapper.Map<OrderDto>(o))
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order == null)
            throw new NullReferenceException("Order not found");
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> AddOrderAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default)
    {
        var order = _mapper.Map<Order>(createOrderDto);
        if (createOrderDto.CustomerId.HasValue)
            order.Customer = await _context.Customers.FindAsync([createOrderDto.CustomerId], cancellationToken);
        
        ValidateOrder(order);
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> UpdateStatusForOrderAsync(Status status, int id, CancellationToken cancellationToken = default)
    {
        var updateOrder = _context.Orders.FirstOrDefault(o => o.Id == id);
        if (updateOrder == null)
            throw new NullReferenceException("Order not found");
        updateOrder.Status = status;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<OrderDto>(updateOrder);
    }

    public async Task<OrderDto> UpdateOrderAsync(OrderDto order, CancellationToken cancellationToken = default)
    {
        var updateOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);
        if (updateOrder == null)
            throw new NullReferenceException("Order not found");
        updateOrder.Status = order.Status;
        updateOrder.OrderDate = order.OrderDate;
        updateOrder.TotalAmount = order.TotalAmount;
        updateOrder.CustomerId = order.CustomerId;
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task DeleteOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order == null)
            throw new NullReferenceException("Order not found");
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void ValidateOrder(Order order)
    {
        if(order.TotalAmount < 0)
            throw new ArgumentException("Order amount cannot be less to 0", nameof(order.TotalAmount));
        if (order.OrderDate < DateTime.MinValue)
            throw new ArgumentException("Order date cannot be less to DateTime.MinValue", nameof(order.OrderDate));
    }
}