using AutoMapper;
using Models;
using Models.Dtos;
using Models.ViewModels;

namespace Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<ReportDto, Report>();
            CreateMap<Report, ReportDto>();
            
            CreateMap<ReportOrderDto, ReportOrder>();
            CreateMap<ReportOrder, ReportOrderDto>();
            
            CreateMap<ShawarmaImportViewModel, ShawarmaDto>();
        }
    }
}