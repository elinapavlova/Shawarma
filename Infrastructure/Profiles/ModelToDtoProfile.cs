using AutoMapper;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.User;

namespace Infrastructure.Profiles
{
    public class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<User, UserResponseDto>();
            
            CreateMap<User, UserCredentialsDto>();
            
            CreateMap<Order, OrderResponseDto>();
            
            CreateMap<Order, OrderDto>();
            
            CreateMap<Shawarma, ShawarmaResponseDto>();
            
            CreateMap<OrderShawarma, OrderShawarmaResponseDto>();
        }
    }
}