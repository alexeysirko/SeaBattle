using Aquality.Selenium.Browsers;
using TechTalk.SpecFlow;

namespace SeaBattleTest.Hooks
{
    [Binding]
    public sealed class BaseTest
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            AqualityServices.Browser.Maximize();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            AqualityServices.Browser.Quit();
        }
    }
}