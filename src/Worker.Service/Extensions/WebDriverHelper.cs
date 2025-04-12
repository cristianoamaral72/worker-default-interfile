using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace Worker.Service.Extensions;

public class WebDriverHelper
{
    private readonly string _driverUrl;
    private readonly ChromeOptions _options;

    public WebDriverHelper(string driverUrl, ChromeOptions options = null)
    {
        _driverUrl = driverUrl;
        _options = options ?? new ChromeOptions();
    }

    /// <summary>
    /// Reconecta ao WebDriver caso a sessão seja perdida.
    /// </summary>
    /// <param name="driver">Instância atual do WebDriver.</param>
    /// <returns>Uma instância ativa do WebDriver.</returns>
    public IWebDriver ReconnectDriver(IWebDriver driver)
    {
        try
        {
            if (IsSessionActive(driver))
            {
                return driver;
            }
        }
        catch (WebDriverException)
        {
            // Ignorar erros de verificação da sessão e continuar a reconexão
        }

        Console.WriteLine("Sessão perdida. Reconectando ao WebDriver...");
        driver?.Quit();
        return InitializeNewDriver();
    }

    /// <summary>
    /// Verifica se a sessão atual do WebDriver está ativa.
    /// </summary>
    /// <param name="driver">Instância atual do WebDriver.</param>
    /// <returns>True se a sessão estiver ativa; caso contrário, False.</returns>
    public bool IsSessionActive(IWebDriver driver)
    {
        try
        {
            return driver != null && driver.WindowHandles.Count > 0;
        }
        catch (WebDriverException)
        {
            return false;
        }
    }

    /// <summary>
    /// Inicializa uma nova instância do WebDriver.
    /// </summary>
    /// <returns>Uma nova instância do WebDriver.</returns>
    private IWebDriver InitializeNewDriver()
    {
        try
        {
            return new RemoteWebDriver(new Uri(_driverUrl), _options);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao inicializar uma nova instância do WebDriver.", ex);
        }
    }
}
