using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Models;
using Models.Dtos;
using Services.Contracts;

namespace Services
{
    public class ReportOrderService : IReportOrderService
    {
        private readonly IReportOrderRepository _repository;
        private readonly IMapper _mapper;
        
        public ReportOrderService
        (
            IReportOrderRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReportOrderDto> Create(ReportOrderDto reportDto)
        {
            var report = _mapper.Map<ReportOrder>(reportDto);
            var result = _mapper.Map<ReportOrderDto>(await _repository.Create(report));
            return result;
        }
        
        public async Task<ReportOrderDto> GetById(int id)
        {
            var report = _mapper.Map<ReportOrderDto>(await _repository.GetById(id));
            return report;
        }

        public async Task<ICollection<ReportOrderDto>> GetList()
        {
            var reports = _mapper.Map<ICollection<ReportOrderDto>>(await _repository.GetList());
            return reports;
        }

        public async Task<ReportOrderDto> Edit(ReportOrderDto editedReport)
        {
            var report = _mapper.Map<ReportOrder>(editedReport);
            var result = _mapper.Map<ReportOrderDto>(await _repository.Edit(report));
            return result;
        }

        public async Task<ReportOrderDto> Delete(int id)
        {
            var report = _mapper.Map<ReportOrderDto>(await _repository.Delete(id));
            return report;
        }
    }
}