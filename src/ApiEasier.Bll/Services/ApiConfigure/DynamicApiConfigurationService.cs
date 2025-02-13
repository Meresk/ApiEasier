﻿using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.Converter;
using ApiEasier.Dal.Interfaces.Db;
using ApiEasier.Dal.Interfaces.FileStorage;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiConfigure
{
    public class DynamicApiConfigurationService : IDynamicApiConfigurationService
    {
        private readonly IFileApiServiceRepository _fileApiServiceRepository;
        private readonly IDbResourceRepository _dbResourceRepository;

        private readonly IConverter<ApiService, ApiServiceDto> _apiServiceToDtoConverter;
        private readonly IConverter<ApiServiceDto, ApiService> _dtoToApiServiceConverter;
        private readonly IConverter<ApiService, ApiServiceSummaryDto> _apiServiceToDtoSummaryConverter;

        public DynamicApiConfigurationService(
            IFileApiServiceRepository fileApiServiceRepository,
            IDbResourceRepository dbResourceRepository,

            IConverter<ApiService, ApiServiceDto> apiServiceToDtoConverter,
            IConverter<ApiServiceDto, ApiService> dtoToApiServiceConverter,
            IConverter<ApiService, ApiServiceSummaryDto> apiServiceToDtoSummaryConverter)
        {
            _fileApiServiceRepository = fileApiServiceRepository;
            _dbResourceRepository = dbResourceRepository;

            _apiServiceToDtoConverter = apiServiceToDtoConverter;
            _dtoToApiServiceConverter = dtoToApiServiceConverter;
            _apiServiceToDtoSummaryConverter = apiServiceToDtoSummaryConverter;
        }

        /// <summary>
        /// Изменение статуса активности api-сервиса в его файле конфигурации
        /// </summary>
        /// <param name="id">название api-сервиса</param>
        /// <param name="status">новый статус активности</param>
        /// <returns></returns>
        public async Task<bool> ChangeActiveStatusAsync(string id, bool status)
        {
            return await _fileApiServiceRepository.ChangeActiveStatusAsync(id, status);
        }

        /// <summary>
        /// Создание файла конфигурации api-сервиса 
        /// </summary>
        /// <param name="dto">данные для создания</param>
        /// <returns></returns>
        public async Task<ApiServiceDto?> CreateAsync(ApiServiceDto dto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(dto);

            var result = await _fileApiServiceRepository.CreateAsync(apiService);
            if (result == null)
                return null;

            return _apiServiceToDtoConverter.Convert(result);
        }

        /// <summary>
        /// Удаление файла конфигурации api-сервиса и ресурсов в бд с ним связанных
        /// </summary>
        /// <param name="id">название api-сервиса</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var result = _fileApiServiceRepository.Delete(id);
            if (!result)
                return false;

            await _dbResourceRepository.DeleteByApiNameAsync(id);

            return result;
        }

        /// <summary>
        /// Вывод всех api-сервисов без их сущностей
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApiServiceSummaryDto>> GetAsync()
        {
            var result = await _fileApiServiceRepository.GetAllAsync();
            return result.Select(_apiServiceToDtoSummaryConverter.Convert).ToList();
        }

        /// <summary>
        /// Вывод api-сервиса по названию
        /// </summary>
        /// <param name="id">название файла конфигурации</param>
        /// <returns></returns>
        public async Task<ApiServiceDto?> GetByIdAsync(string id)
        {
            var result = await _fileApiServiceRepository.GetByIdAsync(id);

            if (result == null)
                return null;

            return _apiServiceToDtoConverter.Convert(result);
        }

        /// <summary>
        /// Обновление данных у api-сервиса в файле конфигурации и ресурсов в бд с ним связанных
        /// </summary>
        /// <param name="id">название файла</param>
        /// <param name="apiServiceDto">данные</param>
        /// <returns></returns>
        public async Task<ApiServiceDto?> UpdateAsync(string id, ApiServiceDto apiServiceDto)
        {
            var apiService = _dtoToApiServiceConverter.Convert(apiServiceDto);

            var result = await _fileApiServiceRepository.UpdateAsync(id, apiService);

            if (result == null)
                return default;

            if (id != apiServiceDto.Name)
            {
                await _dbResourceRepository.UpdateByApiNameAsync(id, apiServiceDto.Name);
            }

            return _apiServiceToDtoConverter.Convert(result);
        }
    }
}
