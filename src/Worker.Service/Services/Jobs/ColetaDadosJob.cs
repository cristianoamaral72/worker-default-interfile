using System;
using System.Collections.Generic;
using System.IO;
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
    private readonly object _demandaLock = new object();
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

    public async Task ExecuteChrome()
    {
        try
        {
            Directory.CreateDirectory(_configuration["AppSettings:DownloadPath"]);

            var opts = new ChromeOptions();
            opts.AddUserProfilePreference("download.default_directory", _configuration["AppSettings:DownloadPath"]);
            opts.AddUserProfilePreference("download.prompt_for_download", false);

            opts.AddArgument("--disable-blink-features=AutomationControlled");
            opts.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
            opts.AddExcludedArgument("enable-automation");
            opts.AddAdditionalOption("useAutomationExtension", false);

            // Adicionando argumentos para tentar burlar verificacao de bot
            opts.AddArguments("--disable-blink-features=AutomationControlled");

            opts.AddArgument(@"user-data-dir=C:\Users\Cristiano da Silva R\AppData\Local\Google\Chrome\User Data");
            opts.AddArgument("--profile-directory=Default");

            _driverFactory.StartDriver(opts: opts);

            await Task.Delay(TimeSpan.FromSeconds(60));
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
            //LogError(ex);
        }
        finally
        {

        }
    }
}

