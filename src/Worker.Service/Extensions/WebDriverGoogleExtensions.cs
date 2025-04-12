using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using WebDriverManager.DriverConfigs.Impl;
using System.Threading.Tasks;


namespace Worker.Service.Extensions;

public static class WebDriverGoogleExtensions
{
    //public static ChromeDriver NovoDriver()
    //{
    //    try
    //    {
    //        // Configurações de ChromeOptions
    //        ChromeOptions options = new ChromeOptions();
    //        options.AddArguments("--log-level=3");
    //        options.AddArguments("--no-sandbox");
    //        options.AddArguments("--disable-dev-shm-usage");
    //        options.AddArguments("--start-maximized");
    //        options.AddArguments("--keep-alive");
    //        options.AddArguments("--disable-background-networking");
    //        options.AddArguments("--disable-renderer-backgrounding");

    //        options.AddArgument(@"user-data-dir=C:\Users\Cristiano da Silva R\AppData\Local\Google\Chrome\User Data");
    //        options.AddArgument("--profile-directory=Default");

    //        // Configurações de serviço
    //        string path = Path.Combine(Environment.CurrentDirectory, "Chrome");
    //        string arquivo = ListaArquivos(path);
    //        ChromeDriverService service = ChromeDriverService.CreateDefaultService(arquivo);
    //        service.HideCommandPromptWindow = true;
    //        service.EnableVerboseLogging = true;
    //        service.LogPath = "chromedriver.log";

    //        // Inicialização do ChromeDriver com timeout estendido
    //        var driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(10));
    //        driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(10);

    //        // Mantém a sessão ativa
    //        KeepSessionAlive(driver);

    //        return driver;
    //    }
    //    catch (WebDriverException ex)
    //    {
    //        Console.WriteLine($"Erro ao iniciar o ChromeDriver: {ex.Message}");
    //        throw;
    //    }
    //}

    public static void KeepSessionAlive(ChromeDriver driver)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    driver.ExecuteScript("return window.performance.timing"); // Comando simples.
                    Console.WriteLine("Sessão ativa.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao manter a sessão ativa: {ex.Message}");
                    break;
                }
                await Task.Delay(5000); // Executa a cada 30 segundos.
            }
        });
    }


    public static ChromeDriver NovoDriver()
    {
        try
        {
            System.Diagnostics.Process.Start("taskkill", "/F /IM chrome.exe /T");
            System.Diagnostics.Process.Start("taskkill", "/F /IM chromedriver.exe /T");


            // Configurações de ChromeOptions
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--log-level=3");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-dev-shm-usage"); // Evita problemas de memória em contêineres
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            // Garanta que a conexão seja mantida ativa
            options.AddArgument("--remote-allow-origins=*");
            options.AddArgument("--keep-alive");
            options.AddArgument("--disable-background-networking");
            options.AddArgument("--disable-background-timer-throttling");
            options.AddArgument("--disable-backgrounding-occluded-windows");
            options.AddArgument("--disable-renderer-backgrounding");
            options.AddArgument("--disable-extensions");

            // Removendo o argumento para iniciar minimizado e configurando para maximizar a janela
            options.AddArguments("--start-maximized");

            // Adicionando argumentos para tentar burlar verificacao de bot
            options.AddArguments("--disable-blink-features=AutomationControlled");

            options.AddArgument(@"user-data-dir=C:\Users\Cristiano da Silva R\AppData\Local\Google\Chrome\User Data");
            options.AddArgument("--profile-directory=Default");

            options.AddExcludedArgument("enable-automation");
            options.AddUserProfilePreference("useAutomationExtension", false);

            // Adicionando preferências adicionais para dificultar a detecção
            options.AddAdditionalOption("useAutomationExtension", false);
            options.AddExcludedArgument("enable-automation");

            // Setup do WebDriverManager para baixar e configurar o ChromeDriver
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());

            string path = Path.Combine(Environment.CurrentDirectory, "Chrome");
            string arquivo = ListaArquivos(path);

            // Cria o serviço do ChromeDriver (sem necessidade de especificar o caminho manualmente)
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(arquivo);
            service.HideCommandPromptWindow = true; // Oculta a janela do prompt de comando para evitar detecção

            // Define um timeout maior para garantir que o Chrome tenha tempo para iniciar
            Thread.Sleep(3000);

            // Inicialização do ChromeDriver com timeout estendido
            var driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(10));

            // Executa script para redefinir a propriedade navigator.webdriver
            driver.ExecuteScript("Object.defineProperty(navigator, 'webdriver', { get: () => undefined })");

            // Redefine algumas propriedades para dificultar a detecção
            driver.ExecuteScript("Object.defineProperty(navigator, 'plugins', { get: () => [1, 2, 3] });");
            driver.ExecuteScript("Object.defineProperty(navigator, 'languages', { get: () => ['en-US', 'en'] });");
            driver.ExecuteScript("window.navigator.permissions.query = (parameters) => parameters.name === 'notifications' ? Promise.resolve({ state: 'granted' }) : window.navigator.permissions.query(parameters);");

            // Inicialização do ChromeDriver com timeout estendido
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(10);

            // Mantém a sessão ativa (implemente o método KeepSessionAlive aqui)
            // O método KeepSessionAlive não foi fornecido, mas você pode implementá-lo aqui para manter a sessão ativa.

            // Retorna o ChromeDriver configurado
            return driver;
        }
        catch (WebDriverException ex)
        {
            Console.WriteLine($"Erro ao iniciar o ChromeDriver: {ex.Message}");
            // Aqui você pode registrar um log de erro, se necessário
            throw new Exception(ex.Message);
        }
    }


    private static string ListaArquivos(string path)
    {
        if (path == null)
        {
            return null;
        }

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string directory = Directory.GetDirectories(path).MaxBy(Directory.GetCreationTime);
        string file = $"{directory}\\X64\\";
        return file;
    }

    public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
    {
        int i = 0;
    Tentantiva:
        try
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
        catch (StaleElementReferenceException ex)
        {
            if (i == 3)
            {
                return null;
            }
            i++;
            goto Tentantiva;
        }
        catch (Exception ex)
        {
            if (i == 3)
            {
                return null;
            }
            i++;
            goto Tentantiva;
        }
    }
    public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
    {
        try
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
            }
        }
        catch (StaleElementReferenceException ex)
        {

        }
        return driver.FindElements(by);
    }
    public static void WaitDocumentLoad(this IWebDriver driver, int? timeoutInSeconds = 10)
    {
        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(timeoutInSeconds ?? 10));
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
    }
    public static void JSSetValue(this WebDriver driver, IWebElement webElement, string value)
    {
        driver.ExecuteScript("arguments[0].value='" + value + "'", webElement);
    }

    public static void JsClick(this WebDriver driver, IWebElement webElement)
    {
        driver.ExecuteScript("arguments[0].click()", webElement);
    }
    public static void JSSetFocus(this WebDriver driver, IWebElement webElement)
    {
        driver.ExecuteScript("arguments[0].focus()", webElement);
    }

    public static void ClearSendKey(this IWebElement webElement, string keys)
    {
        webElement.Clear();
        webElement.SendKeys(keys);
    }

    public static void AngularSelectSetValue(this WebDriver driver, IWebElement webElement, string value)
    {
        webElement.Click();
        Thread.Sleep(200);
        webElement.FindElement(By.XPath("//span[text() = '" + value + "']")).Click();
    }
}

public static class DriverExtensions
{
    public static ChromeDriver NovoDriver()
    {
        ChromeOptions options = new();
        options.AddArguments("--log-level=3");

        return new ChromeDriver(options);
    }

}