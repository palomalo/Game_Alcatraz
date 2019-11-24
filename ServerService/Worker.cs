using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerService.Interface;

namespace ServerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IServerService _serverService;

        public Worker(ILogger<Worker> logger, IServerService serverService)
        {
            _logger = logger;
            _serverService = serverService;
            _serverService.InitializeServerService(NotifyConsole);
        }

        private void NotifyConsole(string message)
        {
            Console.WriteLine(message);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Server running at: {time}", DateTimeOffset.Now);

            //while (!stoppingToken.IsCancellationRequested)
            //{
                _serverService.StartAccepting();

                await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}
