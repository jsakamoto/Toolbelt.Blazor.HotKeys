﻿using Microsoft.Playwright;
using NUnit.Framework;

namespace Toolbelt.Blazor.HotKeys.E2ETest.Internals;

public static class PlaywrightExtensions
{
    public static async ValueTask GotoAndWaitForReadyAsync(this IPage page, string url)
    {
        var waiter = page.WaitForBlazorHasBeenStarted();
        await page.GotoAsync(url);
        await waiter;
        await Task.Delay(200);
    }

    public static async ValueTask WaitForAsync(this IPage page, Func<IPage, ValueTask<bool>> predictAsync, bool throwOnTimeout = true)
    {
        var canceller = new CancellationTokenSource(millisecondsDelay: 5000);
        do
        {
            if (await predictAsync(page)) return;
            await Task.Delay(100);
        } while (!canceller.IsCancellationRequested);
        if (throwOnTimeout) throw new OperationCanceledException(canceller.Token);
    }

    public static async ValueTask WaitForBlazorHasBeenStarted(this IPage page)
    {
        await page.WaitForConsoleMessageAsync(new() { Predicate = message => message.Text == "Blazor has been started." });
    }

    public static async ValueTask AssertEqualsAsync<T>(this IPage page, Func<IPage, Task<T>> selector, T expectedValue)
    {
        var actualValue = default(T);
        await page.WaitForAsync(async p =>
        {
            actualValue = await selector.Invoke(p);
            return actualValue!.Equals(expectedValue);
        }, throwOnTimeout: false);
        actualValue.Is(expectedValue);
    }

    public static async ValueTask AssertEqualsAsync<T>(this IPage page, Func<IPage, Task<IEnumerable<T>>> selector, IEnumerable<T> expectedValue)
    {
        var actualValue = Enumerable.Empty<T>();
        await page.WaitForAsync(async p =>
        {
            actualValue = await selector.Invoke(p);
            return Enumerable.SequenceEqual(actualValue, expectedValue);
        }, throwOnTimeout: false);
        actualValue.Is(expectedValue);
    }

    public static async ValueTask AssertUrlIsAsync(this IPage page, string expectedUrl)
    {
        expectedUrl = expectedUrl.TrimEnd('/');
        await page.AssertEqualsAsync(async _ =>
        {
            var href = await _.EvaluateAsync<string>("window.location.href");
            return href.TrimEnd('/');
        }, expectedUrl);
    }

    public static async ValueTask AssertH1IsAsync(this IPage page, string expectedH1Text)
    {
        var h1 = page.Locator("h1");
        await page.AssertEqualsAsync(_ => h1.TextContentAsync(), expectedH1Text);
    }

    public static Task<bool> IsContentsSelectedAsync(this IPage page)
    {
        return page.EvaluateAsync<bool>("getSelection().type === 'Range'");
    }


    //public static void Counter_Should_Be(this IWebDriver driver, int count)
    //{
    //    var expectedCounterText = $"Current count: {count}";
    //    var counterElement = driver.Wait(1000).Until(_ => driver.FindElement(By.CssSelector("h1+p")));
    //    for (var i = 0; i < (3000 / 200); i++)
    //    {
    //        if (counterElement.Text == expectedCounterText) break;
    //        Thread.Sleep(200);
    //    }
    //    Thread.Sleep(200);
    //    counterElement.Text.Is(expectedCounterText);
    //}

}
