using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PuppeteerSharp;
using Worker.Domain.Interfaces.Services.Chrome;

namespace Worker.Service.Services.Jobs;
public class ColetaDadosJob : IColetaDadosJob
{
    private INavigator _navigator { get; init; }
    public CancellationToken CancellationToken { get; init; }
    protected IDriverFactoryService _driverFactory { get; init; }
    private IConfiguration _configuration { get; init; }

    public ColetaDadosJob(ILogger<ColetaDadosJob> logger,
                               INavigator navigator,
                               IDriverFactoryService driverFactory,
                               IConfiguration configuration)
    {
        _navigator = navigator;
        _driverFactory = driverFactory;
        CancellationToken = new CancellationTokenSource().Token;
        _configuration = configuration;
    }

    public IWebDriver NovoDriver()
    {
        try
        {
            Directory.CreateDirectory(_configuration["AppSettings:DownloadPath"]);

            var opts = new ChromeOptions();
            opts.AddUserProfilePreference("download.default_directory", _configuration["AppSettings:DownloadPath"]);
            opts.AddUserProfilePreference("download.prompt_for_download", false);

            // Configurações para evitar identificação como robô
            opts.AddArgument("--disable-blink-features=AutomationControlled");
            opts.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
            opts.AddExcludedArgument("enable-automation");
            opts.AddAdditionalOption("useAutomationExtension", false);
            // Evita duplicação de argumentos
            opts.AddArgument("--disable-blink-features=AutomationControlled");
            // Configuração de perfil do usuário
            string userDataDir = @"C:\Users\Cristiano da Silva R\AppData\Local\Google\Chrome\User Data";
            opts.AddArgument($"user-data-dir={userDataDir}");
            opts.AddArgument("--profile-directory=Default");
            opts.AddArgument("--disable-extensions");
            opts.AddArgument("--allow-insecure-localhost"); // Permite acessar localhost mesmo sem certificado válido

            // adcionar a opção de maximizar a tela
            opts.AddArgument("--start-maximized");
            var driver = _driverFactory.StartDriver(opts: opts);
            return driver;
        }
        catch (WebDriverException ex)
        {
            if (ex.Message.Contains("HTTP request to the remote WebDriver server")
                || ex.Message.Contains("Unable to get browser")
                || ex.Message.Contains("chrome not reachable")
                || ex.Message.Contains("does not exist"))
            {
                _driverFactory?.Quit();
                _driverFactory?.SetInstance(null);

                System.Diagnostics.Process.Start("taskkill", "/F /IM chrome.exe /T");
                System.Diagnostics.Process.Start("taskkill", "/F /IM chromedriver.exe /T");
            }

            //LogError(ex);
        }
        catch (Exception ex)
        {
            _driverFactory?.Quit();
            _driverFactory?.SetInstance(null);

            //System.Diagnostics.Process.Start("taskkill", "/F /IM chrome.exe /T");
            //System.Diagnostics.Process.Start("taskkill", "/F /IM chromedriver.exe /T");
            return null;
        }

        return default;

    }

    public IWebDriver NovoChromeDriverSelenium()
    {
        try
        {
            // 1. Cria um diretório dedicado para o Selenium
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SeleniumDownloads");
            Directory.CreateDirectory(downloadPath);

            var opts = new ChromeOptions();

            // 2. Configuração de perfil isolado
            string seleniumProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SeleniumChromeProfile");
            opts.AddArgument($"--user-data-dir={seleniumProfilePath}");
            opts.AddArgument("--profile-directory=SeleniumProfile");

            // 3. Configurações de download
            opts.AddUserProfilePreference("download.default_directory", downloadPath);
            opts.AddUserProfilePreference("download.prompt_for_download", false);
            opts.AddUserProfilePreference("download.directory_upgrade", true);

            // 4. Configurações anti-detecção (ajustadas)
            opts.AddExcludedArgument("enable-automation");
            opts.AddAdditionalOption("useAutomationExtension", false);
            opts.AddArgument("--disable-blink-features=AutomationControlled");
            opts.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

            // 5. Isolamento de processos
            opts.AddArgument("--remote-allow-origins=*");
            opts.AddArgument("--no-default-browser-check");
            opts.AddArgument("--disable-component-update");
            opts.AddArgument("--disable-sync");

            // 6. Configurações de desempenho e estabilidade
            opts.AddArgument("--start-maximized");
            opts.AddArgument("--disable-gpu");
            opts.AddArgument("--disable-dev-shm-usage");
            opts.AddArgument("--no-sandbox");

            // 7. Criação segura do driver
            var driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), opts, TimeSpan.FromSeconds(60));
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            return driver;
        }
        catch (WebDriverException ex)
        {
            HandleDriverException(ex);
            return null;
        }
        catch (Exception ex)
        {
            LogError(ex);
            ForceCleanup();
            return null;
        }
    }

    private void LogError(Exception ex)
    {
        Console.WriteLine($"{ex.Message}");
    }

    private void HandleDriverException(WebDriverException ex)
    {
        if (ex.Message.Contains("session not created") ||
            ex.Message.Contains("chrome not reachable"))
        {
            ForceCleanup();
        }
        LogError(ex);
    }

    private void ForceCleanup()
    {
        try
        {
            _driverFactory?.Quit();
            _driverFactory?.SetInstance(null);
        }
        finally
        {
            Process.GetProcesses()
                .Where(p => p.ProcessName.ToLower()
                    .Contains("chromedriver")).ToList()
                .ForEach(p => { p.Kill(); });
        }
    }
}

