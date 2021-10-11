using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
using Models.Order;
using Models.OrderShawarma;
using Models.Role;
using Models.Shawarma;
using Models.Status;
using Models.User;
using Models.ViewModels;

namespace Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile ()
        {
            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserResponseDto>();
            CreateMap<User, ResultContainer<UserResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(u => u));    
            CreateMap<ICollection<User>, ResultContainer<ICollection<UserResponseDto>>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(u => u));
            
            
            CreateMap<OrderRequestDto, Order>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderResponseDto, OrderRequestDto>().ForMember("Date", 
                opt => opt.MapFrom(o => o.Date));
            CreateMap<OrderRequestDto, OrderResponseDto>();
            CreateMap<Order, ResultContainer<OrderResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(o => o));
            CreateMap<ICollection<Order>, ResultContainer<ICollection<OrderResponseDto>>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(o => o));
            CreateMap<ICollection<Order>, ResultContainer<ICollection<OrderDto>>>().ForMember("Data",
                opt
                    => opt.MapFrom(o => o));
            
            
            CreateMap<ShawarmaRequestDto, Shawarma>();
            CreateMap<Shawarma, ShawarmaResponseDto>();
            CreateMap<Shawarma, ResultContainer<ShawarmaResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<Shawarma>, ResultContainer<ICollection<ShawarmaResponseDto>>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(s => s));
            CreateMap<ShawarmaImportViewModel, ShawarmaRequestDto>();
            
            
            CreateMap<StatusRequestDto, Status>();
            CreateMap<Status, StatusResponseDto>();
            CreateMap<Status, ResultContainer<StatusResponseDto>>().ForMember("Data", opt
                 => opt.MapFrom(s => s));
            CreateMap<ICollection<Status>, ResultContainer<ICollection<StatusResponseDto>>>().ForMember("Data", 
                 opt 
                     => opt.MapFrom(s => s));      
            
            
            CreateMap<OrderShawarmaRequestDto, OrderShawarma>();
            CreateMap<OrderShawarma, OrderShawarmaResponseDto>();
            CreateMap<OrderShawarmaResponseDto, OrderShawarmaRequestDto>();
            CreateMap<OrderShawarma, ResultContainer<OrderShawarmaResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<OrderShawarma>, ResultContainer<ICollection<OrderShawarmaResponseDto>>>()
                .ForMember("Data", opt 
                    => opt.MapFrom(s => s));
        }
    }
}