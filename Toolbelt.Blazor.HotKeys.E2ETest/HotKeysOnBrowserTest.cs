using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Toolbelt.Blazor.HotKeys.E2ETest
{
    public class HotKeysOnBrowserTest
    {
        public static IEnumerable<HostingModel> AllHostingModels { get; } = new[] {
            HostingModel.Wasm31,
            HostingModel.Wasm50,
            HostingModel.Server31,
            HostingModel.Server50,
        };

        public static IEnumerable<HostingModel> WasmHostingModels { get; } = new[] {
            HostingModel.Wasm31,
            HostingModel.Wasm50,
        };

        [Test]
        [TestCaseSource(typeof(HotKeysOnBrowserTest), nameof(AllHostingModels))]
        public void HotKey_on_Body_Test(HostingModel hostingModel)
        {
            var context = TestContext.Instance;
            context.StartHost(hostingModel);

            // Navigate to Home
            var driver = context.WebDriver;
            driver.GoToUrlAndWait(context.GetHostUrl(hostingModel));

            driver.Url_Should_Be("/");

            // Go to the "Fetch Data" page by entering "F" key.
            driver.SendKeys("f");
            driver.Url_Should_Be("/fetchdata");

            // Go to the "Home" page by entering "H" key.
            driver.SendKeys("h");
            driver.Url_Should_Be("/");

            // Go to the "Counter" page by entering "H" key.
            driver.SendKeys("c");
            driver.Url_Should_Be("/counter");

            // Increment the counter by entering "U" key.
            driver.Counter_Should_Be(0);
            driver.SendKeys("u");
            driver.Counter_Should_Be(1);
            driver.SendKeys("u");
            driver.Counter_Should_Be(2);

            // Show and hide the cheatSeetElement by entering "?" and ESC key.
            var cheatSeetElement = driver.FindElementByCssSelector(".popup-container");
            cheatSeetElement.Displayed.IsFalse();

            driver.SendKeys("?");
            Thread.Sleep(200);
            cheatSeetElement.Displayed.IsTrue();

            driver.SendKeys(Keys.Escape);
            Thread.Sleep(400);
            cheatSeetElement.Displayed.IsFalse();

            // Double hit of Ctrl key makes jump to the "Home" page.
            driver.Url_Should_Be("/counter");
            driver.SendKeys(Keys.Control);
            Thread.Sleep(100);
            driver.SendKeys(Keys.Control);
            driver.Url_Should_Be("/");
        }

        [Test]
        [TestCaseSource(typeof(HotKeysOnBrowserTest), nameof(AllHostingModels))]
        public void AllowIn_Input_Test(HostingModel hostingModel)
        {
            var context = TestContext.Instance;
            context.StartHost(hostingModel);

            // Navigate to the "Test All Keys" page,
            var driver = context.WebDriver;
            driver.GoToUrlAndWait(context.GetHostUrl(hostingModel), "/test/bykeys");
            driver.Url_Should_Be("/test/bykeys");

            // and shows cheat sheet.
            var cheatSeetElement = driver.FindElementByCssSelector(".popup-container");
            driver.SendKeys("?");
            Thread.Sleep(200);
            cheatSeetElement.Displayed.IsTrue();

            // Entering "C", "F", "Shift+H" in an input element has no effect.
            var inputElement = driver.FindElementByCssSelector(".hot-keys-cheat-sheet input[type=text]");
            inputElement.SendKeys("cfH");
            inputElement.GetAttribute("value").Is("cfH");
            Thread.Sleep(200);
            driver.Url_Should_Be("/test/bykeys");

            // Entering "H" key causes jumping to the "Home" page even though it happened in an input element.
            inputElement.SendKeys("h");
            driver.Url_Should_Be("/");
            inputElement.GetAttribute("value").Is("cfH"); // "H" key was captured, so input text has no change.
        }

        [Test]
        [TestCaseSource(typeof(HotKeysOnBrowserTest), nameof(AllHostingModels))]
        public void AllowIn_NonTextInput_Test(HostingModel hostingModel)
        {
            var context = TestContext.Instance;
            context.StartHost(hostingModel);

            // Navigate to the "Test All Keys" page,
            var driver = context.WebDriver;
            driver.GoToUrlAndWait(context.GetHostUrl(hostingModel), "/test/bykeys");
            driver.Url_Should_Be("/test/bykeys");

            // and shows cheat sheet.
            var cheatSeetElement = driver.FindElementByCssSelector(".popup-container");
            driver.SendKeys("?");
            Thread.Sleep(200);
            cheatSeetElement.Displayed.IsTrue();

            // Entering "F" key in an input element has no effect.
            var inputElement = driver.FindElementByCssSelector(".hot-keys-cheat-sheet input[type=checkbox]");
            inputElement.Click();
            Thread.Sleep(200);
            inputElement.SendKeys("f");
            Thread.Sleep(200);
            driver.Url_Should_Be("/test/bykeys");

            // Entering "C" key causes jumping to the "Counter" page even though it happened in an input element.
            inputElement.SendKeys("c");
            driver.Url_Should_Be("/counter");

            // Entering "H" key causes jumping to the "Counter" page even though it happened in an input element.
            inputElement.SendKeys("h");
            driver.Url_Should_Be("/");
        }

        [Test]
        [TestCaseSource(typeof(HotKeysOnBrowserTest), nameof(WasmHostingModels))]
        public void PreventDefault_Test(HostingModel hostingModel)
        {
            var context = TestContext.Instance;
            context.StartHost(hostingModel);

            // Navigate to the "Counter" page,
            var driver = context.WebDriver;
            driver.GoToUrlAndWait(context.GetHostUrl(hostingModel), "/counter");
            driver.Url_Should_Be("/counter");

            // Ctrl+A does not no effect at the "Counter" page because it is prevented default.
            driver.IsContentsSelected().IsFalse();
            driver.SendKeys("a", ctrl: true);
            Thread.Sleep(200);
            driver.IsContentsSelected().IsFalse();

            // Navigate to the "Fetch Data" page,
            driver.SendKeys("f");
            driver.Url_Should_Be("/fetchdata");

            // Ctrl+A causes select all of contents.
            driver.IsContentsSelected().IsFalse();
            Thread.Sleep(200);
            driver.SendKeys("a", ctrl: true);
            Thread.Sleep(200);
            driver.IsContentsSelected().IsTrue();
        }
    }
}
