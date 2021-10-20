using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.Tokens;
using Models.User;
using Models.ViewModels;

namespace Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            // Dto to Dto
            CreateMap<OrderResponseDto, OrderRequestDto>().ForMember("Date", 
                opt => 
                    opt.MapFrom(o => o.Date));
            
            CreateMap<OrderRequestDto, OrderResponseDto>();
            CreateMap<ShawarmaImportViewModel, ShawarmaRequestDto>();
            CreateMap<OrderShawarmaResponseDto, OrderShawarmaRequestDto>();
            
            // Dto to Model 
            CreateMap<UserRequestDto, User>();
            CreateMap<OrderRequestDto, Order>();
            CreateMap<ShawarmaRequestDto, Shawarma>();
            CreateMap<OrderShawarmaRequestDto, OrderShawarma>();
            
            // Model to Dto
            CreateMap<User, UserResponseDto>();
            CreateMap<User, UserCredentialsDto>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<Shawarma, ShawarmaResponseDto>();
            CreateMap<OrderShawarma, OrderShawarmaResponseDto>();

            CreateMap<ResultContainer<UserResponseDto>, UserCredentialsDto>()
                .ForMember("Email", opt => 
                    opt.MapFrom(u => u.Data.Email))
                .ForMember("Password", opt => 
                    opt.MapFrom(u => u.Data.Password));
            
            CreateMap<AccessToken, AccessTokenDto>()
                .ForMember(a => a.AccessToken, 
                    opt => 
                        opt.MapFrom(a => a.Token))
                .ForMember(a => a.RefreshToken, 
                    opt => 
                        opt.MapFrom(a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration, 
                    opt => 
                        opt.MapFrom(a => a.Expiration));
            
            // Model to ResultContainer
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