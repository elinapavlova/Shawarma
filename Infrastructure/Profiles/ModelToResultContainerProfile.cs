using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.Tokens;
using Models.User;

namespace Infrastructure.Profiles
{
    public class ModelToResultContainerProfile : Profile
    {
        public ModelToResultContainerProfile()
        {
            CreateMap<User, ResultContainer<UserResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(u => u));    
            CreateMap<ICollection<User>, ResultContainer<ICollection<UserResponseDto>>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(u => u));
            CreateMap<ICollection<User>, ResultContainer<ICollection<UserCredentialsDto>>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(u => u));
            
            CreateMap<int, ResultContainer<int>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(u => u));
            
            CreateMap<Order, ResultContainer<OrderResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(o => o));
            CreateMap<ICollection<Order>, ResultContainer<ICollection<OrderResponseDto>>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(o => o));
            CreateMap<ICollection<Order>, ResultContainer<ICollection<OrderDto>>>().ForMember("Data",
                opt
                    => opt.MapFrom(o => o));
            
            
            CreateMap<Shawarma, ResultContainer<ShawarmaResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<Shawarma>, ResultContainer<ICollection<ShawarmaResponseDto>>>().ForMember("Data", 
                opt 
                    => opt.MapFrom(s => s));
            
            
            CreateMap<OrderShawarma, ResultContainer<OrderShawarmaResponseDto>>().ForMember("Data", opt
                => opt.MapFrom(s => s));
            CreateMap<ICollection<OrderShawarma>, ResultContainer<ICollection<OrderShawarmaResponseDto>>>()
                .ForMember("Data", opt 
                    => opt.MapFrom(s => s));
            
            CreateMap<AccessToken, ResultContainer<AccessTokenDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
        }
    }
}