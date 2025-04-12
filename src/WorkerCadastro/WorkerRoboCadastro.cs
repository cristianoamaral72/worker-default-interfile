using System.Diagnostics;
using Worker.Domain.Interfaces.Services.Chrome;

namespace WorkerRoboCadastros
{
    public class WorkerRoboCadastro : BackgroundService
    {
        private readonly ILogger<WorkerRoboCadastro> _logger;
        private readonly IWorkerRoboCadastro _processar;

        public WorkerRoboCadastro(ILogger<WorkerRoboCadastro> logger, IWorkerRoboCadastro processar)
        {
            _logger = logger;
            _processar = processar;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Express�o CRON para cada 10 segundos
            string cronExpression = "*/10 * * * * *";
            var schedule = Cronos.CronExpression.Parse(cronExpression, Cronos.CronFormat.IncludeSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Obter a pr�xima ocorr�ncia com base na CRON
                DateTime? nextOccurrence = schedule.GetNextOccurrence(DateTime.UtcNow);

                // Calcula o intervalo de tempo para aguardar at� a pr�xima execu��o
                TimeSpan delay = nextOccurrence.HasValue
                    ? nextOccurrence.Value - DateTime.UtcNow
                    : TimeSpan.FromSeconds(10); // fallback caso n�o retorne valor

                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                      await _processar.RoboCadastroAync();
                    _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar o WORK: {message}", ex.Message);
                }
                finally
                {
                    // Fechar servi�os
                    FecharServicos();
                    await Task.Delay(delay, stoppingToken);
                }
            }

        }

        public void FecharServicos()
        {
            try
            {
                // Comando para finalizar o processo chromedriver.exe
                ProcessStartInfo stopChromeDriver = new ProcessStartInfo("cmd", "/c taskkill /f /im chromedriver.exe")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(stopChromeDriver);
                _logger.LogInformation("Servi�o chromedriver.exe finalizado.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao finalizar os servi�os: {ex.Message}");
            }
        }

    }
}