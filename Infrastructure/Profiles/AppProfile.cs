using AutoMapper;
using Models.Order;
using Models.OrderShawarma;
using Models.Role;
using Models.Shawarma;
using Models.Status;
using Models.User;

namespace Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile ()
        {
            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserResponseDto>();
            
            CreateMap<OrderRequestDto, Order>();
            CreateMap<Order, OrderResponseDto>();
            
            CreateMap<ShawarmaRequestDto, Shawarma>();
            CreateMap<Shawarma, ShawarmaResponseDto>();
            
            CreateMap<StatusRequestDto, Status>();
            CreateMap<Status, StatusResponseDto>();
            
            CreateMap<OrderShawarmaRequestDto, OrderShawarma>();
            CreateMap<OrderShawarma, OrderShawarmaResponseDto>();

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            CreateMap<OrderShawarmaResponseDto, OrderShawarmaRequestDto>();
        }
    }
}