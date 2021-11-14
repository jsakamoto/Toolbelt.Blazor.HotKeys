using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Toolbelt.Blazor.HotKeys.E2ETest;

public static class WebDriverExtensions
{
    public static void GoToUrlAndWait(this IWebDriver driver, string url, string path = "")
    {
        driver.Navigate().GoToUrl(url.TrimEnd('/') + "/" + path.TrimStart('/'));
        driver.Wait(5000).Until(_ => driver.FindElement(By.CssSelector("a.navbar-brand")));
        driver.Wait(5000).Until(_ => driver.FindElements(By.CssSelector(".loading")).Count == 0);
        Thread.Sleep(200);
    }

    public static WebDriverWait Wait(this IWebDriver driver, int millisecondsTimeout)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(millisecondsTimeout));
        wait.PollingInterval = TimeSpan.FromMilliseconds(200);
        return wait;
    }

    public static void SendKeys(this IWebDriver driver, string keys, bool ctrl = false)
    {
        var action = new Actions(driver);
        if (ctrl) action = action.KeyDown(Keys.Control);
        action = action.SendKeys(keys);
        if (ctrl) action = action.KeyUp(Keys.Control);
        action.Perform();
    }

    public static bool IsContentsSelected(this IWebDriver driver)
    {
        return driver.ExecuteJavaScript<bool>("return getSelection().type === 'Range';");
    }

    public static void Url_Should_Be(this IWebDriver driver, string expectedUrlPath)
    {
        expectedUrlPath = '/' + expectedUrlPath.Trim('/');

        static string toPathAndQuery(string u)
        {
            var pathAndQuery = new Uri(u).PathAndQuery.TrimEnd('/');
            return pathAndQuery == "" ? "/" : pathAndQuery;
        }

        for (var i = 0; i < (3000 / 200); i++)
        {
            if (toPathAndQuery(driver.Url) == expectedUrlPath) break;
            Thread.Sleep(200);
        }
        Thread.Sleep(200);
        toPathAndQuery(driver.Url).Is(expectedUrlPath);
    }

    public static void Counter_Should_Be(this IWebDriver driver, int count)
    {
        var expectedCounterText = $"Current count: {count}";
        var counterElement = driver.Wait(1000).Until(_ => driver.FindElement(By.CssSelector("h1+p")));
        for (var i = 0; i < (3000 / 200); i++)
        {
            if (counterElement.Text == expectedCounterText) break;
            Thread.Sleep(200);
        }
        Thread.Sleep(200);
        counterElement.Text.Is(expectedCounterText);
    }
}
