﻿using ApiEasier.Dm.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiEasier.Bll.Dto
{
    /// <summary>
    /// DTO для представления эндпоинта
    /// </summary>
    public class ApiEndpointDto
    {
        /// <summary>
        /// Маршрут для действия API.
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Имя может содержать только буквы, цифры.")]
        public required string Route { get; set; }

        /// <summary>
        /// Тип ответа, ожидаемого от действия API.
        /// </summary>
        [Required]
        public TypeResponse Type { get; set; }

        /// <summary>
        /// Указывает, активно ли действие API.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }
    }
}
