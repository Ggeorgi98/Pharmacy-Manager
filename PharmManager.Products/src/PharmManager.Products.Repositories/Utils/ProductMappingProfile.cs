using AutoMapper;
using PharmManager.Products.Domain.Dtos;
using PharmManager.Products.Repositories.Entities;

namespace BookingApp.Users.DAL.Utils
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
