using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Web.Helpers.Logging
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IApplicationBuilder _appBuilder;

        public DbLoggerProvider(
            Func<string, LogLevel, bool> filter,
            IApplicationBuilder appBuilder)
        {
            _filter = filter;
            _appBuilder = appBuilder;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(categoryName, _filter, _appBuilder);
        }

        public void Dispose()
        {

        }
    }
}
