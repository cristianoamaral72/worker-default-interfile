using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using Worker.Domain.Interfaces.Services.Chrome;

namespace Worker.Service.Services.Chrome;

public class DriverFactoryService : IDriverFactoryService
{
    public IWebDriver Instance => _driver;
    private static IWebDriver _driver { get; set; }
    public DriverFactoryService() { }

    public IWebDriver StartDriver(string driverType = "chrome", DriverOptions opts = null)
    {
        if (_driver is null)
        {
            _driver = Instantiate(driverType, opts);

            // Executar JavaScript para desabilitar a detecção de automação
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined});");

            // Navegar para o site de login
            _driver.Navigate().GoToUrl("https://accounts.google.com/signin");

            // Aguarde para simular ações humanas
            System.Threading.Thread.Sleep(3000);

            // Interagir com os campos de login
            IWebElement emailField = _driver.FindElement(By.Id("identifierId"));
            emailField.SendKeys("cristianoadsen01@gmail.com");

            IWebElement nextButton = _driver.FindElement(By.Id("identifierNext"));
            nextButton.Click();

            System.Threading.Thread.Sleep(3000);

            // Localizar o campo de senha
            IWebElement passwordField = _driver.FindElement(By.Name("Passwd")); // Campo de senha geralmente usa o atributo name="password"
            passwordField.SendKeys("FatimA@342!@");

            // Localizar e clicar no botão "Próxima"
            IWebElement passwordNextButton = _driver.FindElement(By.Id("passwordNext"));
            passwordNextButton.Click();

            // Aguarde novamente para simular atraso humano
            Thread.Sleep(5000);

            // Você pode verificar se o login foi bem-sucedido
            Console.WriteLine("Login executado. Verifique se você foi autenticado com sucesso.");
        }
            
        return _driver;
    }

    public void SetInstance(IWebDriver driver) => _driver = driver;

    private IWebDriver Instantiate(string driverType, DriverOptions opts = null)
    {
        return driverType switch
        {
            "IE" => new InternetExplorerDriver(opts as InternetExplorerOptions),

            "chrome" =>
                // Configuração de porta fixa para o ChromeDriver
                CreateChromeDriverPort(opts as ChromeOptions),

            _ => throw new ArgumentException($"Driver '{driverType}' is not supported.")
        };
    }

    private IWebDriver CreateChromeDriverPort(ChromeOptions opts)
    {
        // Configura o serviço do ChromeDriver com uma porta fixa
        var chromeService = ChromeDriverService.CreateDefaultService();
        //chromeService.Port = 52364; // Porta fixa para o ChromeDriver

        // Configuração de tempo máximo de sessão ativa usando timeouts
        var commandTimeout = TimeSpan.FromMinutes(240); // Define um tempo de timeout mais longo
        // Retorna a instância do ChromeDriver
        return new ChromeDriver(chromeService, opts, commandTimeout);
    }

    private IWebDriver CreateChromeDriver(ChromeOptions opts)
    {
        // Configura o serviço do ChromeDriver com uma porta fixa
        var chromeService = ChromeDriverService.CreateDefaultService(); 
        chromeService.Port = 52364; // Porta fixa para o ChromeDriver

        // Configuração de tempo máximo de sessão ativa usando timeouts
        var commandTimeout = TimeSpan.FromMinutes(240); // Define um tempo de timeout mais longo
        // Retorna a instância do ChromeDriver
        return new ChromeDriver(chromeService, opts, commandTimeout); ;
    }

    public IWebDriver StartDriverHeyGen(string driverType = "chrome", DriverOptions opts = null)
    {
        if (_driver is null)
        {
            _driver = Instantiate(driverType, opts);

            // Executar JavaScript para desabilitar a detecção de automação
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined});");

            // Navegar para o site de login
            
            //// cristianoadsen01@gmail.com
            ////_driver.Navigate().GoToUrl("https://studio.youtube.com/channel/UCgT4rA8avsv42uVdgxYb_Qw/analytics/tab-overview/period-default");

            //// cristiano.ribeiro62@gmail.com
            //_driver.Navigate().GoToUrl("https://studio.youtube.com/channel/UCqVKv_jv5pcKTuR-CipBOHg/analytics/tab-overview/period-default");

            //// Aguarde para simular ações humanas
            //System.Threading.Thread.Sleep(3000);
        }

        return _driver;
    }

    public void RestartDriverInstance()
    {
        // Finaliza a instância atual
        _driver?.Quit();
        _driver = null;

        // Cria uma nova instância do WebDriver
        _driver = new ChromeDriver();
    }

    public IWebDriver StartDriverIdeogram(string driverType = "chrome", DriverOptions opts = null)
    {
        if (_driver is null)
        {
            _driver = Instantiate(driverType, opts);

            // Executar JavaScript para desabilitar a detecção de automação
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined});");

            // Navegar para o site de login
            _driver.Navigate().GoToUrl("https://accounts.google.com/signin");

            // Aguarde para simular ações humanas
            System.Threading.Thread.Sleep(3000);
            
            // Você pode verificar se o login foi bem-sucedido
            Console.WriteLine("Login executado. Verifique se você foi autenticado com sucesso.");

            System.Threading.Thread.Sleep(5000);
            //_driver.Navigate().GoToUrl("https://ideogram.ai/t/my-images/public");
        }

        return _driver;
    }

    //public IWebDriver StartDriverIdeogram(string driverType = "chrome", DriverOptions opts = null)
    //{
    //    if (_driver is null)
    //    {
    //        _driver = Instantiate(driverType, opts);

    //        // Executar JavaScript para desabilitar a detecção de automação
    //        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
    //        js.ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined});");

    //        // Navegar para o site de login
    //        _driver.Navigate().GoToUrl("https://ideogram.ai/t/my-images/public");

    //        // Aguarde para simular ações humanas
    //        System.Threading.Thread.Sleep(5000);
    //    }

    //    return _driver;
    //}

    public void Quit() => _driver?.Quit();
}