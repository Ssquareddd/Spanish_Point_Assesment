using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;

namespace HelloWorld;

public static class HelloSelenium
{
    public static void Main()
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1920,1080");

        var driver = new ChromeDriver(options);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            driver.Navigate().GoToUrl("https://matchingengine.com");

            var modulesLink = driver.FindElement(By.LinkText("Modules"));
            var actions = new Actions(driver);

            actions.MoveToElement(modulesLink).Perform();
            driver.FindElement(By.LinkText("Repertoire Management Module")).Click();

            var additionalFeatures = driver.FindElement(By.XPath("//h2[text()=\"Additional Features\"]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", additionalFeatures);
            Thread.Sleep(1000);

            driver.FindElement(By.LinkText("Products Supported")).Click();
            Thread.Sleep(1000);

            var h3Header = wait.Until(d => d.FindElement(By.XPath("//h3[text()='There are several types of Product Supported:']")));
            Assert.That(h3Header.Displayed, Is.True, "Expected h3 header was not visible");

            var expectedItems = new List<string>
            {
                "Cue Sheet / AV Work",
                "Recording",
                "Bundle",
                "Advertisement"
            };

            foreach (var expectedItem in expectedItems)
            {
                var listItem = wait.Until(d => d.FindElement(By.XPath($"//li/span[contains(text(),'{expectedItem}')]")));
                Assert.Multiple(() =>
                {
                    Assert.That(listItem.Displayed, Is.True, $"List item '{expectedItem}' is not visible");
                    Assert.That(listItem.Text.Trim(), Is.EqualTo(expectedItem), $"List item text doesn't match '{expectedItem}'");
                });
            }
        }
        finally
        {
            driver.Quit();
        }
    }
}