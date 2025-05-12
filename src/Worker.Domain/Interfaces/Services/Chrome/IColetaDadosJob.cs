using OpenQA.Selenium;
using System;
using System.Threading.Tasks;

namespace Worker.Domain.Interfaces.Services.Chrome;

public interface IColetaDadosJob
{
    IWebDriver NovoDriver();
    IWebDriver NovoChromeDriverSelenium();

}