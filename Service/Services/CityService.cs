﻿using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Cities;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CityService> _logger;

        public CityService(ICityRepository cityRepo,
                           IMapper mapper,
                           ICountryRepository countryRepo,
                           ILogger<CityService> logger)
        {
            _cityRepo = cityRepo;
            _mapper = mapper;
            _countryRepo = countryRepo;
            _logger = logger;
        }

        public async Task CreateAsync(CityCreateDto model)
        {
            var existCountry = await _countryRepo.GetById(model.CountryId);

            if (existCountry is null)
            {
                _logger.LogWarning($"Country is not found - {model.CountryId + "-" + DateTime.Now.ToString()}");
                throw new NotFoundException($"Id - {model.CountryId}  country notfound");
            }

            await _cityRepo.CreateAsync(_mapper.Map<City>(model));
        }

        public async Task EditAsync(int? id, CityEditDto model)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var city = await _cityRepo.GetById((int)id) ?? throw new NotFoundException("Data not found");

            var existCountry = await _countryRepo.GetById(model.CountryId);

            if (existCountry is null)
            {
                _logger.LogWarning($"Country is not found - {model.CountryId + "-" + DateTime.Now.ToString()}");
                throw new NotFoundException($"Id - {model.CountryId}  country notfound");
            }

            _mapper.Map(model, city);

            await _cityRepo.EditAsync(city);
        }

        public async Task DeleteAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var city = await _cityRepo.GetById((int)id) ?? throw new NotFoundException("Data not found");

            await _cityRepo.DeleteAsync(city);
        }

        public async Task<IEnumerable<CityDto>> FilterAsync(string name, string countryName)
        {
            return _mapper.Map<IEnumerable<CityDto>>(await _cityRepo.FilterAsync(name, countryName));
        }

        public async Task<PaginationResponse<CityDto>> GetPaginateAsync(int page, int take)
        {
            var cities = await _cityRepo.GetAllAsync();
            int totalPage = (int)Math.Ceiling((decimal)cities.Count() / take);

            var mappedDatas = _mapper.Map<IEnumerable<CityDto>>(await _cityRepo.GetPaginateDatasAsync(page, take));

            return new PaginationResponse<CityDto>(mappedDatas, totalPage, page);
        }

        public async Task<IEnumerable<CityDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CityDto>>(await _cityRepo.GetAllWithCountryAsync());
        }

        public async Task<CityDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CityDto>(_cityRepo.FindBy(m => m.Id == id, m => m.Country).FirstOrDefault());
        }

        public async Task<CityDto> GetByNameAsync(string name)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));

            return _mapper.Map<CityDto>(_cityRepo.FindBy(m => m.Name == name, m => m.Country).FirstOrDefault());
        }
    }
}
