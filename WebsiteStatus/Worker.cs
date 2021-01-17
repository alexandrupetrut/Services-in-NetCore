using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebsiteStatus
{
    // powershell example after publishing the solution to a folder
    // > sc.exe create WebsiteStatusService binpath= c:\publishedfolder\WebsiteStatus.exe start= auto
    // > sc.exe delete WebsiteStatusService

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            return base.StopAsync(cancellationToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync("https://www.iamtimcorey.com");
                if (result.IsSuccessStatusCode)
                    _logger.LogInformation("\"DotNet Worker Service\": The site is up. Status code is {statusCode}", result.StatusCode);
                else
                    _logger.LogError("\"DotNet Worker Service\": The site is down. Status code is {statusCode}", result.StatusCode);
                await Task.Delay(5 * 1000, stoppingToken);
            }
        }
    }
}
