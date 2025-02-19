﻿using ApiEasier.Bll.Dto;
using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Logger.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEasier.Api.Controllers.ApiConfiguration
{
    /// <summary>
    /// Позволяет управлять сущностями API-сервиса.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEntityController(
        IDynamicEntityConfigurationService dynamicEntityConfigurationService,
        ILoggerService logger) : ControllerBase
    {
        private readonly IDynamicEntityConfigurationService _dynamicEntityConfigurationService = dynamicEntityConfigurationService;
        private readonly ILoggerService _logger = logger;

        /// <summary>
        /// Получить все сущности API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        [HttpGet("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiEntitySummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntitiesByApiName(string apiServiceName)
        {
            try
            {
                var apiEntities = await _dynamicEntityConfigurationService.GetAllAsync(apiServiceName);

                if (apiEntities == null)
                    return NotFound($"API-сервис {apiServiceName} не найден");

                return Ok(apiEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Возвращает сущность по имени
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса, которому пренадлежит сущность</param>
        /// <param name="entityName">Имя искаемой сущности</param>
        [HttpGet("{apiServiceName}/{entityName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiEntitySummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntityByName(string apiServiceName, string entityName)
        {
            try
            {
                var apiEntity = await _dynamicEntityConfigurationService.GetByIdAsync(apiServiceName, entityName);

                if (apiEntity == null)
                    return NotFound($"Сущность {entityName} в API-сервисе {apiServiceName} не найдена.");

                return Ok(apiEntity);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Добавляет новую сущность API-сервису
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса, к которому надо добавить сущность</param>
        /// <param name="apiEntityDto">Добавляемая сущность</param>
        [HttpPost("{apiServiceName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<List<ApiEntitySummaryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddEntity(string apiServiceName, [FromBody] ApiEntityDto apiEntityDto)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.CreateAsync(apiServiceName, apiEntityDto);

                if (result == null)
                    return Conflict($"Сущность с именем {apiEntityDto.Name} уже существует.");

                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Изменяет существующую сущность у API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с изменяемой сущностью</param>
        /// <param name="entityName">Имя изменяемой сущности</param>
        /// <param name="apiEntityDto">Новые данные сущности</param>
        [HttpPut("{apiServiceName}/{entityName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateEntity(string apiServiceName, string entityName, [FromBody] ApiEntityDto apiEntityDto)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.UpdateAsync(apiServiceName, entityName, apiEntityDto);

                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Удаляет сущность внутри API-сервиса
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса с удаляемой сущностью</param>
        /// <param name="entityName">Имя удаляемой сущности</param>
        [HttpDelete("{apiServiceName}/{entityName}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEntity(string apiServiceName, string entityName)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.DeleteAsync(apiServiceName, entityName);

                if (!result)
                    return NotFound($"Сущность {entityName} в API-сервисе {apiServiceName} не найдена.");

                return NoContent();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }

        /// <summary>
        /// Изменяет активность сущности у API-сервиса 
        /// </summary>
        /// <param name="isActive">True - активная сущность, false - неактивная сущность</param>
        /// <param name="apiServiceName"></param>
        /// <param name="apiEntityName"></param>
        /// <returns></returns>
        [HttpPatch("{apiServiceName}/{apiEntityName}/{isActive}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeActiveApiEntity(bool isActive, string apiServiceName, string apiEntityName)
        {
            try
            {
                var result = await _dynamicEntityConfigurationService.ChangeActiveStatusAsync(apiServiceName, apiEntityName, isActive);

                if (!result)
                    return NotFound($"статус у сущности {apiEntityName} у api-сервиса {apiServiceName} не был изменен");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}

