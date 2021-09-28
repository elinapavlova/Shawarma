using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Dtos;
using Services.Contracts;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly IMapper _mapper;
        private readonly int _pageSize;
        private readonly int _page;
        
        public ReportService
        (
            IReportRepository repository,
            IMapper mapper,
            IConfiguration configuration
        )
        {
            _repository = repository;
            _mapper = mapper;
            _pageSize = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPageSize"]);
            _page = Convert.ToInt32(configuration["AppSettingsConfiguration:DefaultPage"]);
        }

        public async Task<ReportDto> Create(ReportDto reportDto)
        {
            var report = _mapper.Map<Report>(reportDto);
            var result = _mapper.Map<ReportDto>(await _repository.Create(report));
            return result;
        }
        
        public async Task<ReportDto> GetById(int id)
        {
            var report = _mapper.Map<ReportDto>(await _repository.GetById(id));
            return report;
        }

        public async Task<ReportDto> GetByDate(DateTime date)
        {
            var report = _mapper.Map<ReportDto>(await _repository.GetByDate(date));
            return report;
        }

        public async Task<ICollection<ReportDto>> GetList()
        {
            var reports = _mapper.Map<ICollection<ReportDto>>(await _repository.GetList());
            return reports;
        }

        public async Task<ICollection<ReportDto>> GetListByPage(int pageSize, int page)
        {
            // Transfer default pageSize and page values from appsettings.json
            var reports = _mapper.Map<ICollection<ReportDto>>(await _repository.GetListByPage(_pageSize, _page));
            return reports;
        }

        public async Task<ReportDto> Edit(ReportDto editedReport)
        {
            var report = _mapper.Map<Report>(editedReport);
            var result = _mapper.Map<ReportDto>(await _repository.Edit(report));
            return result;
        }

        public async Task<ReportDto> Delete(int id)
        {
            var report = _mapper.Map<ReportDto>(await _repository.Delete(id));
            return report;
        }
    }
}