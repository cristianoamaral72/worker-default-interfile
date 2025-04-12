using OpenQA.Selenium;

namespace Worker.Domain.Interfaces.Services.Chrome;

public interface IDriverFactoryService
{
    abstract IWebDriver Instance { get; }
    void SetInstance(IWebDriver driver);
    void Quit();
    IWebDriver StartDriver(string driverType = "chrome", DriverOptions opts = null);
}