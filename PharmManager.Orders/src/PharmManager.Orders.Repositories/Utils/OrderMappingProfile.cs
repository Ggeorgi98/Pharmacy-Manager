using AutoMapper;
using PharmManager.Orders.Domain.Dtos;
using PharmManager.Orders.Repositories.Entities;

namespace PharmManager.Orders.Repositories.Utils
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderSpecDto, Order>();

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemSpecDto, OrderItem>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
