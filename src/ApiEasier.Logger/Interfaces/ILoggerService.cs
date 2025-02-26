﻿using Microsoft.AspNetCore.Http;

namespace ApiEasier.Logger.Interfaces
{
    /// <summary>
    /// Позволяет логгировать сообщения от приложении, изменяя поведение логов от их вида 
    /// </summary>
    public interface ILoggerService
    {
        public void LogHttp(HttpContext context, string requestBody, string responseBody);
        public void LogInfo(string message);
        public void LogWarn(string message);
        public void LogError(Exception ex, string message);
        public void LogDebug(string message);
        public void LogFatal(Exception ex, string message);
    }
}
