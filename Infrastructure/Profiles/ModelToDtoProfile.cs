using AutoMapper;
using Infrastructure.Result;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.Tokens;
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
        }
    }
}