using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Worker.Domain.Interfaces.Repositories;
using Worker.Domain.Interfaces.Services;
using Worker.Domain.Interfaces.Services.Chrome;

namespace Worker.Service.Services.Chrome;

public class WorkerRoboCadastro : IWorkerRoboCadastro
{
    private readonly ILogger<WorkerRoboCadastro> _logger;
    WebDriver driver;
    protected readonly IDriverFactoryService _driverFactory;
    private readonly IColetaDadosJob _coletaDadosJob;
    private readonly IVivoCargaService _vivoCargaService;
    public WorkerRoboCadastro(ILogger<WorkerRoboCadastro> logger, 
        IDriverFactoryService driverFactory,
        IColetaDadosJob coletaDadosJob, IVivoCargaService vivoCargaService
        )
    {
        _logger = logger;
        _driverFactory = driverFactory;
        _coletaDadosJob = coletaDadosJob;
        _vivoCargaService = vivoCargaService;
    }

    public async Task RoboCadastroAync()
    {
        _logger.LogInformation($"Ideogram - Inicio: {DateTime.Now}");

        try
        {
            if (driver != null)
            {
                driver.Quit();
            }

            driver = (WebDriver)_coletaDadosJob.NovoChromeDriverSelenium() ?? (WebDriver)_coletaDadosJob.NovoChromeDriverSelenium();

            if (driver != null)
            {

                driver.Navigate().GoToUrl("https://studio.youtube.com/channel/UCc2LRRVqIUQVMYYu28V5Hsw");
                _logger.LogInformation("Inicio do Login YouTube ");

                var vivoCarga = await _vivoCargaService.GetAllAsync();

                foreach (var item in vivoCarga)
                {

                }
            }
            else
            {
                driver = (WebDriver)_coletaDadosJob.NovoDriver();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao executar o Ideogram: {ex.Message}");
        }

        Fim:
        if (driver != null)
        {
            _driverFactory?.Quit();
            _driverFactory?.SetInstance(null);

            System.Diagnostics.Process.Start("taskkill", "/F /IM chrome.exe /T");
            System.Diagnostics.Process.Start("taskkill", "/F /IM chromedriver.exe /T");
        }
    }
}