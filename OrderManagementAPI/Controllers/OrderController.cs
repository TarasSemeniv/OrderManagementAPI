using Microsoft.AspNetCore.Mvc;
using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.DTOs.Outputs;
using OrderManagementCore.Interfaces;

namespace OrderManagementAPI.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public ActionResult<IEnumerable<OrderDto>> GetAllOrders()
    {
        return Ok(_orderService.GetOrders());
    }

    [HttpGet("{id}")]
    public ActionResult<OrderDto> GetOrderById(int id)
    {
        return Ok(_orderService.GetOrderById(id));
    }

    [HttpPost]
    public ActionResult<OrderDto> CreateOrder(CreateOrderDto createOrderDto)
    {
        var taskDto = _orderService.AddOrder(createOrderDto);
        return Created("", taskDto);
    }
}