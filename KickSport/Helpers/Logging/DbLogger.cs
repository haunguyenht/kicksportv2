using KickSport.Data;
using KickSport.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace KickSport.Web.Helpers.Logging
{
    public class DbLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IApplicationBuilder _appBuilder;

        public DbLogger(
            string categoryName,
            Func<string, LogLevel, bool> filter,
            IApplicationBuilder appBuilder)
        {
            _categoryName = categoryName;
            _filter = filter;
            _appBuilder = appBuilder;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                using var serviceScope = _appBuilder.ApplicationServices.CreateScope();
                var context = serviceScope.ServiceProvider.GetService<KickSportDbContext>();
                context.Logs.Add(new Log()
                {
                    EventId = eventId.Id,
                    EventName = eventId.Name,
                    LogLevel = logLevel.ToString(),
                    StackTrace = exception?.StackTrace,
                    Message = exception?.Message,
                    CreatedTime = DateTime.Now
                });

                context.SaveChanges();
            }
        }
    }
}
