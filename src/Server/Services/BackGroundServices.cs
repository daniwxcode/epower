using BlazorHero.CleanArchitecture.Application.Interfaces.Services;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Services
{
    public class BackGroundServices : BackgroundService
    {
        private readonly ILogger<BackGroundServices> _logger;
        private readonly ICeetService _ceetService;
        public BackGroundServices(ICeetService ceetService, ILogger<BackGroundServices> logger)
        {
            _ceetService = ceetService;
            _logger = logger;   
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Placez ici la logique de la tâche que vous souhaitez exécuter toutes les 6 heures
                _logger.LogInformation("Exécution de la tâche planifiée...");

                // Attendez 6 heures (en millisecondes)
                await Task.Delay(6*60*60*1000, run(stoppingToken));
            }
        }

        private CancellationToken run(CancellationToken cancellationToken)
        {
            try
            {
                _ceetService.EOS();
            }catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message, ex);
            }
           
            return cancellationToken;
        }
    }
}
