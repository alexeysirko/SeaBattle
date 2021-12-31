using Aquality.Selenium.Browsers;
using SeaBattleTest.TestData;
using TechTalk.SpecFlow;

namespace SeaBattleTest.StepDefinitions
{
    [Binding]
    public class SeaBattleStepDefinition
    {
        [Given(@"I move to main page")]
        public void GivenIMoveToMainPage()
        {
            AqualityServices.Browser.GoTo(TestSettings.mainPageLink);
        }

        [Given(@"I choose random enemy")]
        public void GivenIChooseRandomEnemy()
        {
            throw new PendingStepException();
        }

        [Given(@"I randomly set ships")]
        public void GivenIRandomlySetShips()
        {
            throw new PendingStepException();
        }

        [Given(@"I click play button")]
        public void GivenIClickPlayButton()
        {
            throw new PendingStepException();
        }

        [Given(@"I wait untill enemy is loaded")]
        public void GivenIWaitUntillEnemyIsLoaded()
        {
            throw new PendingStepException();
        }

        [Given(@"I play seaWars")]
        public void GivenIPlaySeaWars()
        {
            throw new PendingStepException();
        }
    }
}
