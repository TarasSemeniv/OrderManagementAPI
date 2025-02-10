using AutoMapper;
using OrderManagementCore.DTOs.Inputs;
using OrderManagementCore.DTOs.Outputs;
using OrderManagementEntity.Models;

namespace OrderManagementAPI.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateOrderDto, Order>();
        CreateMap<Order, OrderDto>()
            .ForMember(x => x.CustomerId, cgf => 
                cgf.MapFrom(s => s.CustomerId))
            .ReverseMap();
    }
}