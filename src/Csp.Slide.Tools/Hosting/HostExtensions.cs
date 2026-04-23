using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sqray.Slide.Common;

namespace Csp.Slide.Tools.Hosting
{
    /// <summary>
    /// 注册日志
    /// </summary>
    public static class HostExtensions
    {
        public static IHostBuilder RegisterLog(this IHostBuilder builder)
        {
            builder.ConfigureLogging(loggingBuilder =>
             {
                 loggingBuilder.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
             });
            builder.ConfigureServices(services =>
            {
                services.AddLogging();

                services.AddSingleton<ISlideLogger, SlideLogger>();
            });
            return builder;
        }
    }

    public class SlideLogger : ISlideLogger
    {
        private readonly ILogger<SlideLogger> _logger;
        public SlideLogger(ILogger<SlideLogger> logger)
        {
            _logger = logger;
        }
        public void Debug(string message)
        {
            _logger.LogDebug(message);
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        public void Warn(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
