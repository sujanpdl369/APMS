
using APMS.Data;
using APMS.Services.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace APMS.Services.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            try
            {
                _logger.LogError(message, args);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "Innner Exception: " + ex.InnerException.Message, args);
            }
        }
    }
}
