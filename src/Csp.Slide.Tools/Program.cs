using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sqray.Slide.Converter;
using Csp.Slide.Tools.Hosting;

namespace Csp.Slide.Tools
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .RegisterLog()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ConverterFactoryProvider>();
                    services.AddSingleton<IConsoleMessage, ConsoleMessage>();
                })
                .ConfigureCocona(args, new[] { typeof(Commands) })
                .Build()
                .RunAsync();
        }
    }
}