using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using static System.TimeSpan;

namespace Worker.Service.Pages;
public static class SeleniumExtensions
{
    public static WebDriverWait WaitTime(this IWebDriver driver, int seconds = 10)
        => new(driver, FromSeconds(seconds));
    public static IWebElement WaitElement(this IWebDriver driver, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, FromSeconds(seconds));
            var element = wait.Until(d => d.FindElement(by));
            return element;
        }
        catch
        {
            return null;
        }
    }

    public static IWebElement WaitElementModal(this IWebDriver driver, By modalBy, By elementBy, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

            // Aguardar o modal estar presente
            wait.Until(d =>
            {
                var modal = d.FindElement(modalBy);
                return modal.Displayed && modal.Enabled;
            });

            // Aguardar o elemento dentro do modal
            var element = wait.Until(d =>
            {
                var modal = d.FindElement(modalBy);
                return modal.FindElement(elementBy);
            });

            return element;
        }
        catch
        {
            return null;
        }
    }

    public static IWebElement WaitElementInModal(this IWebDriver driver, By elementSelector, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

            // Aguardar o elemento estar presente e dentro de um modal ativo
            var element = wait.Until(d =>
            {
                var elementInModal = d.FindElement(elementSelector);

                // Verificar se o elemento está visível e habilitado
                if (elementInModal.Displayed && elementInModal.Enabled)
                {
                    // Validar se o elemento está dentro de um modal
                    var modalParent = elementInModal.FindElement(By.XPath("ancestor-or-self::*[contains(@class, 'rc-dialog-content')]"));
                    if (modalParent != null && modalParent.Displayed)
                    {
                        return elementInModal;
                    }
                }

                return null;
            });

            return element;
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine($"Elemento '{elementSelector}' não encontrado no modal.");
            return null;
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine($"Timeout ao aguardar o elemento '{elementSelector}' no modal.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado ao aguardar o elemento no modal: {ex.Message}");
            return null;
        }
    }


    public static IWebElement WaitElement(this IWebElement webElement, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(webElement.GetWebDriver(), FromSeconds(seconds));
            var element = wait.Until(d => d.FindElement(by));
            return element;
        }
        catch
        {
            return null;
        }
    }

    public static IEnumerable<IWebElement> WaitElements(this IWebDriver driver, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, FromSeconds(seconds));
            var elements = wait.Until(d => d.FindElements(by));
            return elements;
        }
        catch
        {
            return default;
        }
    }

    public static IEnumerable<IWebElement> WaitElements(this IWebElement webElement, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(webElement.GetWebDriver(), FromSeconds(seconds));
            var elements = wait.Until(d => d.FindElements(by));
            return elements;
        }
        catch
        {
            return default;
        }
    }

    private static IWebDriver GetWebDriver(this IWebElement webElement) => (webElement as IWrapsDriver).WrappedDriver;

    public static string HasAlert(IWebDriver driver)
    {
        try
        {
            var alert = driver.SwitchTo().Alert().Text;
            driver.SwitchTo().Alert().Accept();

            return alert;
        }
        catch
        {
            System.Console.WriteLine("Validacao de Popup: Não encontrado.");
        }
        return null;
    }

    public static string ValidatePopup(IWebDriver driver)
    {
        try
        {
            driver.SwitchTo().DefaultContent();
            var element = driver.WaitElement(By.Id("divAviso"), seconds: 1);

            if (element?.Displayed is not null)
                return element?.Text;
        }
        catch
        {
            System.Console.WriteLine("Validacao de Popup: Não encontrado.");
        }
        return null;
    }

    public static string GetInnerText(this IWebDriver driver, string selector, int waitSeconds = 5)
    {
        var element = driver.WaitElement(By.CssSelector(selector), waitSeconds);
        if (element is null) return null;
        var text = (string)(driver as IJavaScriptExecutor)?.ExecuteScript("return arguments[0].innerText", element);
        return text?.Trim();
    }

    public static string GetTextValue(this IWebDriver driver, string selector)
    {
        var element = driver.WaitElement(By.CssSelector(selector), 5);
        if (element is null) return null;
        var text = (string)(driver as IJavaScriptExecutor)?.ExecuteScript("return arguments[0].value", element);
        return text?.Trim();
    }

    public static string GetInnerText(this IWebElement element, string selector, int waitSeconds = 3)
    {
        var targetElement = element.WaitElement(By.CssSelector(selector), waitSeconds);
        if (element is null) return null;
        var text = (string)(element.GetWebDriver() as IJavaScriptExecutor)?.ExecuteScript("return arguments[0].innerText", targetElement);
        return text?.Trim();
    }

    public static void SetTextValue(this IWebDriver driver, string selector, string value)
    {
        var element = driver.WaitElement(By.CssSelector(selector), 5);
        (driver as IJavaScriptExecutor)?.ExecuteScript($"arguments[0].value='{value}'", element);
    }

    public static IReadOnlyCollection<IWebElement> WaitHtmlElements(this IWebDriver driver, By by, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(drv => drv.FindElements(by));
    }

    public static void WaitForPageLoad(this IWebDriver driver, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(webDriver => ((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState").Equals("complete"));
    }

    #region Novos metodo selenium

    public static IWebElement FindElementIfExists(this IWebDriver driver, By by)
    {
        var elements = driver.FindElements(by);
        return (elements.Count >= 1) ? elements.First() : null;
    }

    public static IWebElement FindElementIfExists(this IWebElement driver, By by)
    {
        var elements = driver.FindElements(by);
        return (elements.Count >= 1) ? elements.First() : null;
    }

    public static void LoadPage(this IWebDriver webDriver,
        TimeSpan timeToWait, string url)
    {
        webDriver.Manage().Timeouts().PageLoad.Add(timeToWait);
        webDriver.Navigate().GoToUrl(url);
    }

    public static string GetText(this IWebDriver webDriver, By by)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by);

        if (webElement == null)
        {
            return string.Empty;
        }

        return webElement.Text;
    }

    public static string GetText(this IWebDriver webDriver, By by, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement == null)
        {
            return string.Empty;
        }

        return webElement.Text;
    }

    public static string GetInputText(this IWebDriver webDriver, By by, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement == null)
        {
            return string.Empty;
        }

        return webElement.GetAttribute("value");
    }

    public static void Click(this IWebDriver webDriver, By by, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement != null)
        {
            webElement.Click();
        }
    }

    public static void MoveClick(this IWebDriver webDriver, By by, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement != null)
        {
            new Actions(webDriver).MoveToElement(webElement).Click().Perform();
        }
    }

    public static void MoveClick(this IWebElement webDriver, IWebDriver driver, By by, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement != null)
        {
            new Actions(driver).MoveToElement(webElement).Click().Perform();
        }
    }

    public static void SetText(this IWebDriver webDriver,
        By by, string text)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by);

        if (webElement != null && text != null)
        {
            new Actions(webDriver).MoveToElement(webElement).Click().Perform();
            new Actions(webDriver).MoveToElement(webElement).SendKeys(text).Perform();
        }
    }

    public static void SetText(this IWebDriver webDriver,
        By by, string text, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement != null && text != null)
        {
            new Actions(webDriver).MoveToElement(webElement).Click().Perform();
            new Actions(webDriver).MoveToElement(webElement).SendKeys(text).Perform();
        }
    }

    public static void Clear(this IWebDriver webDriver, By by, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement != null)
        {
            webElement.Clear();
        }
    }

    public static void SetTextWithoutMove(this IWebDriver webDriver,
        By by, string text, int timeOut)
    {
        IWebElement webElement = webDriver.FindElementIfExists(by, timeOut);

        if (webElement != null)
        {
            webElement.SendKeys(text);
        }
    }

    public static IWebElement FindElementIfExists(this IWebElement driver, By by, int timeOut)
    {
        for (int i = 0; i < timeOut; i++)
        {
            var elements = driver.FindElements(by);

            if (elements.Count >= 1)
            {
                return elements.First();
            }

            Thread.Sleep(1000);
        }

        return null;
    }

    public static IWebElement FindElementIfExists(this IWebDriver driver, By by, int timeOut)
    {
        for (int i = 0; i < timeOut; i++)
        {
            var elements = driver.FindElements(by);

            if (elements.Count >= 1)
            {
                return elements.First();
            }

            Thread.Sleep(1000);
        }

        return null;
    }

    public static ICollection<IWebElement> FindElementsIfExists(this IWebDriver driver, By by, int timeOut)
    {
        for (int i = 0; i < timeOut; i++)
        {
            var elements = driver.FindElements(by);

            if (elements.Count > 0)
            {
                return elements;
            }

            Thread.Sleep(1000);
        }

        return null;
    }

    public static ICollection<IWebElement> FindElementsIfExists(this IWebElement driver, By by, int timeOut)
    {
        for (int i = 0; i < timeOut; i++)
        {
            if (driver != null)
            {
                var elements = driver.FindElements(by);

                if (elements.Count > 0)
                {
                    return elements;
                }
            }

            Thread.Sleep(1000);
        }

        return null;
    }
    
    private static Image ScreenshotToImage(Screenshot screenshot)
    {
        Image screenshotImage;
        using (var memStream = new MemoryStream(screenshot.AsByteArray))
        {
            screenshotImage = Image.FromStream(memStream);
        }

        return screenshotImage;
    }

    public static bool CheckDowloadFile(string path, string file, int timeOut, bool regex = true, string extension = ".pdf", bool contains = false)
    {
        for (int i = 0; i < timeOut; i++)
        {
            var directory = new DirectoryInfo(path);
            var fileNameDir = directory.GetFiles().Where(x => x.Name.ToLower().Contains(extension));

            if (fileNameDir.Count() == 0)
            {
                if (file.ToLower().Contains(extension))
                {
                    fileNameDir = directory.GetFiles();
                }
            }

            foreach (var item in fileNameDir)
            {
                if (regex)
                {
                    if (contains)
                    {
                        if (Regex.Replace(file.Replace(".pdf", ""), @"\s+", "").Contains(Regex.Replace(item.Name.Replace(".pdf", ""), @"\s+", "")))
                        {
                            long size = 0;

                            while (size == 0)
                            {
                                FileInfo sizeFile = new FileInfo(@$"{directory.FullName}\{item.Name}");

                                size = sizeFile.Length;

                                if (sizeFile.Length > 0)
                                {
                                    return true;
                                }
                            }

                            return true;
                        }
                    }
                    else
                    {
                        if (Regex.Replace(item.Name, @"\s+", "") == Regex.Replace(file, @"\s+", ""))
                        {
                            long size = 0;

                            while (size == 0)
                            {
                                FileInfo sizeFile = new FileInfo(@$"{directory.FullName}\{item.Name}");

                                size = sizeFile.Length;

                                if (sizeFile.Length > 0)
                                {
                                    return true;
                                }
                            }

                            return true;
                        }
                    }
                }
                else
                {
                    if (item.Name == file)
                    {
                        return true;
                    }
                }
            }

            Thread.Sleep(1000);
        }

        return false;
    }
}



public class SelectModelElement
{
    public int Index { get; set; }

    public string Text { get; set; }
}

    #endregion
