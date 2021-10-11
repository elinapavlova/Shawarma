using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.User;
using Models.ViewModels;

namespace Infrastructure.Profiles
{
    public class DtoToDtoProfile : Profile
    {
        public DtoToDtoProfile ()
        {
            CreateMap<OrderResponseDto, OrderRequestDto>().ForMember("Date", 
                opt => 
                    opt.MapFrom(o => o.Date));
            
            CreateMap<OrderRequestDto, OrderResponseDto>();

            CreateMap<ShawarmaImportViewModel, ShawarmaRequestDto>();
            
            CreateMap<OrderShawarmaResponseDto, OrderShawarmaRequestDto>();

        }
    }
}