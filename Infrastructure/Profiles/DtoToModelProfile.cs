using AutoMapper;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.User;

namespace Infrastructure.Profiles
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<UserRequestDto, User>();
            
            CreateMap<OrderRequestDto, Order>();
            
            CreateMap<ShawarmaRequestDto, Shawarma>();
            
            CreateMap<OrderShawarmaRequestDto, OrderShawarma>();
        }
    }
}