using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
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
            CreateMap<OrderRequestDto, OrderResponseDto>();
            
            CreateMap<ShawarmaRequestDto, Shawarma>();
            CreateMap<Shawarma, ShawarmaResponseDto>();
            
            CreateMap<StatusRequestDto, Status>();
            CreateMap<Status, StatusResponseDto>();
            
            CreateMap<OrderShawarmaRequestDto, OrderShawarma>();
            CreateMap<OrderShawarma, OrderShawarmaResponseDto>();

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            CreateMap<OrderShawarmaResponseDto, OrderShawarmaRequestDto>();

            CreateMap<Order, ResultContainer<OrderResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(o => o));
            CreateMap<ICollection<Order>, ResultContainer<ICollection<OrderResponseDto>>>().ForMember("Data", 
                opt => 
                    opt.MapFrom(o => o));
            
            CreateMap<User, ResultContainer<UserResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(u => u));
            CreateMap<ICollection<User>, ResultContainer<ICollection<UserResponseDto>>>().ForMember("Data", 
                opt => 
                    opt.MapFrom(u => u));
            
            CreateMap<Role, ResultContainer<RoleResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(r => r));
            CreateMap<ICollection<Role>, ResultContainer<ICollection<RoleResponseDto>>>().ForMember("Data", 
                opt => 
                    opt.MapFrom(r => r));
            
            CreateMap<Status, ResultContainer<StatusResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<Status>, ResultContainer<ICollection<StatusResponseDto>>>().ForMember("Data", 
                opt => 
                    opt.MapFrom(s => s));
            
            CreateMap<Shawarma, ResultContainer<ShawarmaResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<Shawarma>, ResultContainer<ICollection<ShawarmaResponseDto>>>().ForMember("Data", 
                opt => 
                    opt.MapFrom(s => s));
            
            CreateMap<OrderShawarma, ResultContainer<OrderShawarmaResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<OrderShawarma>, ResultContainer<ICollection<OrderShawarmaResponseDto>>>()
                .ForMember("Data", opt 
                    => opt.MapFrom(s => s));
   /*         
            CreateMap<UserLoginResponseDto, ResultContainer<UserLoginResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<UserLoginResponseDto>, ResultContainer<ICollection<UserLoginResponseDto>>>()
                .ForMember("Data", opt 
                    => opt.MapFrom(s => s));
    */
        }
    }
}